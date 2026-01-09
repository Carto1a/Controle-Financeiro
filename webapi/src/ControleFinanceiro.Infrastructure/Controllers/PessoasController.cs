using ControleFinanceiro.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Infrastructure.Controllers;

[ApiController]
[Area("Pessoas")]
[Route("[controller]")]
public class PessoaController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Criar(
        [FromServices] CriarPessoaCommandHandler handler,
        [FromBody] CriarPessoaCommand request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailed)
            return BadRequest(result);

        return Created();
    }

    [HttpDelete(":Id")]
    public async Task<IActionResult> Deletar(
        [FromServices] DeletarPessoaCommandHandler handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, cancellationToken);
        if (result.IsFailed)
            return BadRequest(result);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ListarBasico(
        [FromServices] ObterListaPessoasBasicaQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);
        if (result.IsFailed)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("detalhado")]
    public async Task<IActionResult> ListarDetalhada(
        [FromServices] ObterListaPessoasDetalhadaQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);
        if (result.IsFailed)
            return BadRequest(result);

        return Ok(result);
    }
}
