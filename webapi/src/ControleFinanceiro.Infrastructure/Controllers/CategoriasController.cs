using ControleFinanceiro.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Infrastructure.Controllers;

[ApiController]
[Area("Categorias")]
[Route("[controller]")]
public class CategoriasController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Criar(
        [FromServices] CriarCategoriaCommandHandler handler,
        [FromBody] CriarCategoriaCommand request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailed)
            return BadRequest(result);

        return Created();
    }

    [HttpGet]
    public async Task<IActionResult> ListarBasico(
        [FromServices] ObterListaCategoriasBasicaQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);
        if (result.IsFailed)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("detalhado")]
    public async Task<IActionResult> ListarDetalhada(
        [FromServices] ObterListaCategoriasDetalhadaQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);
        if (result.IsFailed)
            return BadRequest(result);

        return Ok(result);
    }
}
