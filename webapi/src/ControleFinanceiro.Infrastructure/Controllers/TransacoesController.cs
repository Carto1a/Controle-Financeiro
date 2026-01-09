using ControleFinanceiro.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Infrastructure.Controllers;

public class TransacoesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Criar(
        [FromServices] CriarTransacaoCommandHandler handler,
        [FromBody] CriarTransacaoCommand request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailed)
            return BadRequest(result);

        return Created();
    }

    [HttpGet("detalhado")]
    public async Task<IActionResult> ListarDetalhada(
        [FromServices] ObterListaTransacoesDetalhadaQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);
        if (result.IsFailed)
            return BadRequest(result);

        return Ok(result);
    }
}
