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
        if (viewModel.ManagerId.HasValue)
        {
            var manager = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == viewModel.ManagerId.Value);

            if (manager == null)
            {
                throw new InvalidOperationException("O gerente informado não existe.");
            }
        }

        if (viewModel.ManagerId.HasValue)
        {
            var manager = await _context.Departaments
                .FirstOrDefaultAsync(d => d.ManagerId == viewModel.ManagerId.Value);
            
            if (manager != null)
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

        var departament = new Departament
        {
            Name = viewModel.Name,
            ManagerId = viewModel.ManagerId,
            HigherDepartamentId = viewModel.HigherDepartamentId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
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
