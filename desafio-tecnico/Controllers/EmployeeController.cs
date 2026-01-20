using Microsoft.AspNetCore.Mvc;
using desafio_tecnico.Services;
using desafio_tecnico.ViewModels;

namespace desafio_tecnico.Controllers;

public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartamentService _departamentService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(
        IEmployeeService employeeService,
        IDepartamentService departamentService,
        ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _departamentService = departamentService;
        _logger = logger;
    }

    [HttpGet("api/employees")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Models.Employee>>> GetAllApi()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("api/employees/{id}")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Models.Employee>> GetById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpPost("api/employees")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Models.Employee>> CreateApi([FromBody] CreateEmployeeViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var employee = await _employeeService.CreateEmployeeAsync(viewModel);

            if (employee != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
            }

            return BadRequest("Erro ao criar o colaborador.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar colaborador via API");
            return StatusCode(500, "Ocorreu um erro ao criar o colaborador.");
        }
    }

    [HttpPut("api/employees/{id}")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Models.Employee>> UpdateApi(int id, [FromBody] CreateEmployeeViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var employee = await _employeeService.UpdateEmployeeAsync(id, viewModel);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar colaborador via API");
            return StatusCode(500, "Ocorreu um erro ao atualizar o colaborador.");
        }
    }

}
