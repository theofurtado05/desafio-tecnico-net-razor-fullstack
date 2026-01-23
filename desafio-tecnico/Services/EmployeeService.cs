using Microsoft.EntityFrameworkCore;
using desafio_tecnico.Data;
using desafio_tecnico.Models;
using desafio_tecnico.ViewModels;

namespace desafio_tecnico.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _context;

    public EmployeeService(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<PagedResponse<Employee>> GetAllEmployeesAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Employees
            .Where(e => e.IsDeleted == null || e.IsDeleted == false)
            .Include(e => e.Departament)
            .OrderBy(e => e.Name);

        var totalRecords = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        foreach (var employee in data)
        {
            if (employee.Departament != null)
            {
                employee.Departament.Employees = new List<Employee>();
            }
        }

        var itemsInPage = data.Count;

        return new PagedResponse<Employee>
        {
            Data = data,
            CurrentPage = page,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            ItemsInPage = itemsInPage
        };
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _context.Employees
            .Where(e => e.Id == id && (e.IsDeleted == null || e.IsDeleted == false))
            .Include(e => e.Departament)
            .AsSplitQuery()
            .FirstOrDefaultAsync();

        if (employee?.Departament != null)
        {
            employee.Departament.Employees = new List<Employee>();
        }
        
        return employee;
    }

    public async Task<Employee?> CreateEmployeeAsync(CreateEmployeeViewModel viewModel)
    {
        var departament = await _context.Departaments
            .FirstOrDefaultAsync(d => d.Id == viewModel.DepartmentId && (d.IsDeleted == null || d.IsDeleted == false));

        if (departament == null)
        {
            throw new InvalidOperationException("O departamento informado não existe.");
        }

        var existingCpf = await _context.Employees
            .FirstOrDefaultAsync(e => e.CPF == viewModel.CPF && (e.IsDeleted == null || e.IsDeleted == false));

        if (existingCpf != null)
        {
            throw new InvalidOperationException("Já existe um colaborador com este CPF.");
        }

        if (!string.IsNullOrWhiteSpace(viewModel.Rg))
        {
            var existingRg = await _context.Employees
                .FirstOrDefaultAsync(e => e.Rg == viewModel.Rg && (e.IsDeleted == null || e.IsDeleted == false));

            if (existingRg != null)
            {
                throw new InvalidOperationException("Já existe um colaborador com este RG.");
            }
        }

        var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        
        var employee = new Employee
        {
            Name = viewModel.Name,
            CPF = viewModel.CPF,
            Rg = viewModel.Rg,
            DepartmentId = viewModel.DepartmentId,
            CreatedAt = now,
            UpdatedAt = now
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return await GetEmployeeByIdAsync(employee.Id);
    }

    public async Task<Employee?> UpdateEmployeeAsync(int id, CreateEmployeeViewModel viewModel)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id && (e.IsDeleted == null || e.IsDeleted == false));

        if (employee == null)
        {
            return null;
        }

        var departament = await _context.Departaments
            .FirstOrDefaultAsync(d => d.Id == viewModel.DepartmentId && (d.IsDeleted == null || d.IsDeleted == false));

        if (departament == null)
        {
            throw new InvalidOperationException("O departamento informado não existe.");
        }

        var existingCpf = await _context.Employees
            .FirstOrDefaultAsync(e => e.CPF == viewModel.CPF && e.Id != id && (e.IsDeleted == null || e.IsDeleted == false));

        if (existingCpf != null)
        {
            throw new InvalidOperationException("Já existe um colaborador com este CPF.");
        }

        if (!string.IsNullOrWhiteSpace(viewModel.Rg))
        {
            var existingRg = await _context.Employees
                .FirstOrDefaultAsync(e => e.Rg == viewModel.Rg && e.Id != id && (e.IsDeleted == null || e.IsDeleted == false));

            if (existingRg != null)
            {
                throw new InvalidOperationException("Já existe um colaborador com este RG.");
            }
        }

        employee.Name = viewModel.Name;
        employee.CPF = viewModel.CPF;
        employee.Rg = viewModel.Rg;
        employee.DepartmentId = viewModel.DepartmentId;
        employee.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();

        return await GetEmployeeByIdAsync(employee.Id);
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id && (e.IsDeleted == null || e.IsDeleted == false));

        if (employee == null)
        {
            return false;
        }

        var managedDepartament = await _context.Departaments
            .FirstOrDefaultAsync(d => d.ManagerId == id && (d.IsDeleted == null || d.IsDeleted == false));

        if (managedDepartament != null)
        {
            throw new InvalidOperationException("Não é possível excluir um colaborador que é gerente de um departamento. Remova-o como gerente primeiro.");
        }

        employee.IsDeleted = true;
        employee.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        
        await _context.SaveChangesAsync();

        return true;
    }
}