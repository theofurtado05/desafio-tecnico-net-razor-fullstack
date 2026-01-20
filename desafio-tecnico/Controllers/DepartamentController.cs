using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("api/departaments")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Models.Departament>>> GetAll()
    {
        var departaments = await _departamentService.GetAllDepartamentsAsync();
        return Ok(departaments);
    }

    [HttpPost("api/departaments")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Models.Departament>> CreateApi([FromBody] CreateDepartamentViewModel viewModel)
    {
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
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar departamento via API");
            return StatusCode(500, "Ocorreu um erro ao criar o departamento.");
        }
    }

    [HttpGet("api/departaments/{id}")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Models.Departament>> GetById(int id)
    {
        var departament = await _departamentService.GetAllDepartamentsAsync();
        var result = departament.FirstOrDefault(d => d.Id == id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

}
