using Microsoft.EntityFrameworkCore;
using desafio_tecnico.Data;
using desafio_tecnico.Models;
using desafio_tecnico.ViewModels;

namespace desafio_tecnico.Services;

public class DepartamentService : IDepartamentService
{
    private readonly ApplicationDbContext _context;

    public DepartamentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Departament?> CreateDepartamentAsync(CreateDepartamentViewModel viewModel)
    {
        var existingName = await _context.Departaments
            .FirstOrDefaultAsync(d => d.Name == viewModel.Name);

        if (existingName != null)
        {
            throw new InvalidOperationException("Já existe um departamento com este nome.");
        }


        int? managerId = viewModel.ManagerId.HasValue && viewModel.ManagerId.Value > 0 
            ? viewModel.ManagerId.Value 
            : null;

        if (managerId.HasValue)
        {
            var manager = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == managerId.Value);

            if (manager == null)
            {
                throw new InvalidOperationException("O gerente informado não existe.");
            }

            var existingDepartament = await _context.Departaments
                .FirstOrDefaultAsync(d => d.ManagerId == managerId.Value);
            
            if (existingDepartament != null)
            {
                throw new InvalidOperationException("O gerente informado já é gerente de outro departamento.");
            }
        }

        if (viewModel.HigherDepartamentId.HasValue)
        {
            var higherDepartament = await _context.Departaments
                .FirstOrDefaultAsync(d => d.Id == viewModel.HigherDepartamentId.Value);

            if (higherDepartament == null)
            {
                throw new InvalidOperationException("O departamento superior informado não existe.");
            }
        }

        var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        
        var departament = new Departament
        {
            Name = viewModel.Name,
            ManagerId = managerId,
            HigherDepartamentId = viewModel.HigherDepartamentId,
            CreatedAt = now,
            UpdatedAt = now
        };

        _context.Departaments.Add(departament);
        await _context.SaveChangesAsync();

        return await _context.Departaments
            .Include(d => d.Manager)
            .Include(d => d.HigherDepartament)
            .FirstOrDefaultAsync(d => d.Id == departament.Id);
    }

    /*
        - Só é possivel atualizar o responsavel do departamento caso o responsavel ja esteja nesse departamento
    */
    public async Task<Departament?> UpdateDepartamentAsync(int id, CreateDepartamentViewModel viewModel)
    {
        var departament = await _context.Departaments.FindAsync(id);

        if (departament == null)
        {
            return null;
        }

        var existingName = await _context.Departaments
            .FirstOrDefaultAsync(d => d.Name == viewModel.Name && d.Id != id);

        if (existingName != null)
        {
            throw new InvalidOperationException("Já existe um departamento com este nome.");
        }

        int? managerId = viewModel.ManagerId.HasValue && viewModel.ManagerId.Value > 0 
            ? viewModel.ManagerId.Value 
            : null;

        if (managerId != departament.ManagerId)
        {
            if (managerId.HasValue)
            {
                var manager = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == managerId.Value);

                if (manager == null)
                {
                    throw new InvalidOperationException("O gerente informado não existe.");
                }

                if (manager.DepartmentId != id)
                {
                    throw new InvalidOperationException("O gerente informado deve pertencer a este departamento.");
                }

                var existingDepartament = await _context.Departaments
                    .FirstOrDefaultAsync(d => d.ManagerId == managerId.Value && d.Id != id);
                
                if (existingDepartament != null)
                {
                    throw new InvalidOperationException("O gerente informado já é gerente de outro departamento.");
                }
            }
        }

        if (viewModel.HigherDepartamentId.HasValue)
        {
            if (viewModel.HigherDepartamentId.Value == id)
            {
                throw new InvalidOperationException("Um departamento não pode ser superior a si mesmo.");
            }

            var higherDepartament = await _context.Departaments
                .FirstOrDefaultAsync(d => d.Id == viewModel.HigherDepartamentId.Value);

            if (higherDepartament == null)
            {
                throw new InvalidOperationException("O departamento superior informado não existe.");
            }
        }

        departament.Name = viewModel.Name;
        departament.ManagerId = managerId;
        departament.HigherDepartamentId = viewModel.HigherDepartamentId;
        departament.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();

        return await _context.Departaments
            .Include(d => d.Manager)
            .Include(d => d.HigherDepartament)
            .FirstOrDefaultAsync(d => d.Id == departament.Id);
    }

    public async Task<Departament?> GetDepartamentByIdAsync(int id)
    {
        var departament = await _context.Departaments
            .Include(d => d.Manager)
            .Include(d => d.HigherDepartament)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (departament == null)
        {
            return null;
        }

        var employees = await _context.Employees
            .Where(e => e.DepartmentId == id)
            .ToListAsync();

        departament.Employees = employees;

        return departament;
    }

    public async Task<List<Departament>> GetAllDepartamentsAsync()
    {
        return await _context.Departaments
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<bool> ValidateManagerBelongsToDepartamentAsync(int managerId, int? departmentId)
    {
        if (!departmentId.HasValue)
            return true;

        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == managerId);

        return employee != null && employee.DepartmentId == departmentId.Value;
    }

    public async Task<bool> DeleteDepartamentAsync(int id)
    {
        var departament = await _context.Departaments.FirstOrDefaultAsync(d => d.Id == id && (d.IsDeleted == null || d.IsDeleted == false));

        if(departament == null)
        {
            return false;
        }

        departament.IsDeleted = true;
        departament.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        departament.ManagerId = null;

        var employees = await _context.Employees
            .Where(e => e.DepartmentId == id && (e.IsDeleted == null || e.IsDeleted == false)).ToListAsync();
        

        foreach(var employee in employees)
        {
            employee.DepartmentId = null;
            employee.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        }

        await _context.SaveChangesAsync(); 
        return true;
    }
}
