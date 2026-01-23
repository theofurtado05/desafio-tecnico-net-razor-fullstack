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

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("api/employees")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<Models.Employee>>> GetAllApi([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _employeeService.GetAllEmployeesAsync(page, pageSize);
        return Ok(result);
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

            return BadRequest(new { message = "Erro ao criar o colaborador." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar colaborador via API");
            return StatusCode(500, new { message = "Ocorreu um erro ao criar o colaborador." });
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
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar colaborador via API");
            return StatusCode(500, new { message = "Ocorreu um erro ao atualizar o colaborador." });
        }
    }

    [HttpDelete("api/employees/{id}")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> DeleteApi(int id)
    {
        try
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao deletar colaborador");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar colaborador via API");
            return StatusCode(500, new { message = "Ocorreu um erro ao deletar o colaborador." });
        }
    }

}
