using desafio_tecnico.Models;
using desafio_tecnico.ViewModels;

namespace desafio_tecnico.Services;

public interface IDepartamentService
{
    Task<Departament?> CreateDepartamentAsync(CreateDepartamentViewModel viewModel);
    Task<Departament?> UpdateDepartamentAsync(int id, CreateDepartamentViewModel viewModel);
    Task<Departament?> GetDepartamentByIdAsync(int id);
    Task<PagedResponse<Departament>> GetAllDepartamentsAsync(int page, int pageSize, DepartamentFilterViewModel? filters = null);
    Task<bool> ValidateManagerBelongsToDepartamentAsync(int managerId, int? departmentId);
    Task<bool> DeleteDepartamentAsync(int id);
}
