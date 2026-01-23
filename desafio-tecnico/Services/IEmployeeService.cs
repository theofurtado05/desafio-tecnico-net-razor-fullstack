using desafio_tecnico.Models;
using desafio_tecnico.ViewModels;

namespace desafio_tecnico.Services;

public interface IEmployeeService
{
    Task<PagedResponse<Employee>> GetAllEmployeesAsync(int page, int pageSize);
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task<Employee?> CreateEmployeeAsync(CreateEmployeeViewModel viewModel);
    Task<Employee?> UpdateEmployeeAsync(int id, CreateEmployeeViewModel viewModel);
    Task<bool> DeleteEmployeeAsync(int id);
}
