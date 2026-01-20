using desafio_tecnico.Models;
using desafio_tecnico.ViewModels;

namespace desafio_tecnico.Services;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task<Employee?> CreateEmployeeAsync(CreateEmployeeViewModel viewModel);
    Task<Employee?> UpdateEmployeeAsync(int id, CreateEmployeeViewModel viewModel);
}
