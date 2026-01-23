using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using desafio_tecnico.Services;
using desafio_tecnico.ViewModels;

namespace desafio_tecnico.Controllers;

public class DepartamentController : Controller
{
    private readonly IDepartamentService _departamentService;
    private readonly ILogger<DepartamentController> _logger;

    public DepartamentController(
        IDepartamentService departamentService,
        ILogger<DepartamentController> logger)
    {
        _departamentService = departamentService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Details(int id)
    {
        ViewBag.DepartamentId = id;
        return View();
    }

    [HttpGet("api/departaments")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<Models.Departament>>> GetAll(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] string? name = null,
        [FromQuery] int? managerId = null,
        [FromQuery] int? higherDepartamentId = null)
    {
        var filters = new DepartamentFilterViewModel
        {
            Name = name,
            ManagerId = managerId,
            HigherDepartamentId = higherDepartamentId
        };

        var result = await _departamentService.GetAllDepartamentsAsync(page, pageSize, filters);
        return Ok(result);
    }

    [HttpPost("api/departaments")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Models.Departament>> CreateApi([FromBody] CreateDepartamentViewModel viewModel)
    {
        if (viewModel == null)
        {
            return BadRequest("O corpo da requisição não pode ser nulo.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var departament = await _departamentService.CreateDepartamentAsync(viewModel);

            if (departament != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = departament.Id }, departament);
            }

            return BadRequest("Erro ao criar o departamento.");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar departamento");
            return BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro de banco de dados ao criar departamento");
            return BadRequest(new { message = "Erro ao salvar no banco de dados. Verifique se os dados estão corretos." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar departamento via API");
            return StatusCode(500, new { message = "Ocorreu um erro ao criar o departamento." });
        }
    }

    [HttpGet("api/departaments/{id}")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Models.Departament>> GetById(int id)
    {
        var departament = await _departamentService.GetDepartamentByIdAsync(id);

        if (departament == null)
        {
            return NotFound();
        }

        return Ok(departament);
    }

    [HttpPut("api/departaments/{id}")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Models.Departament>> UpdateApi(int id, [FromBody] CreateDepartamentViewModel viewModel)
    {
        if (viewModel == null)
        {
            return BadRequest("O corpo da requisição não pode ser nulo.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var departament = await _departamentService.UpdateDepartamentAsync(id, viewModel);

            if (departament == null)
            {
                return NotFound();
            }

            return Ok(departament);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar departamento");
            return BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro de banco de dados ao atualizar departamento");
            return BadRequest(new { message = "Erro ao salvar no banco de dados. Verifique se os dados estão corretos." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar departamento via API");
            return StatusCode(500, new { message = "Ocorreu um erro ao atualizar o departamento." });
        }
    }

    [HttpDelete("api/departaments/{id}")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> DeleteApi(int id)
    {
        var result = await _departamentService.DeleteDepartamentAsync(id);

        if (!result)
        {
            return NotFound();
        }

        return Ok(result);
    }

}
