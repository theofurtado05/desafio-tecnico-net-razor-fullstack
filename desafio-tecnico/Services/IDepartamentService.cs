using desafio_tecnico.Models;
using desafio_tecnico.ViewModels;

namespace desafio_tecnico.Services;

public interface IDepartamentService
{
    Task<Departament?> CreateDepartamentAsync(CreateDepartamentViewModel viewModel);
    Task<Departament?> UpdateDepartamentAsync(int id, CreateDepartamentViewModel viewModel);
    Task<Departament?> GetDepartamentByIdAsync(int id);
    Task<List<Departament>> GetAllDepartamentsAsync();
    Task<bool> ValidateManagerBelongsToDepartamentAsync(int managerId, int? departmentId);
}
