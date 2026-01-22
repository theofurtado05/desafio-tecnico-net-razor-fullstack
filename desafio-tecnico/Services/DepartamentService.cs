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
}
