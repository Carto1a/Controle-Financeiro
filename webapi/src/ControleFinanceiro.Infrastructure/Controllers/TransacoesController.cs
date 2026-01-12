using ControleFinanceiro.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Infrastructure.Controllers;

[ApiController]
[Area("Transacao")]
[Route("[controller]")]
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
            return BadRequest(result.Errors);

        return Created();
    }

    [HttpGet("detalhado")]
    public async Task<IActionResult> ListarDetalhada(
        [FromServices] ObterListaTransacoesDetalhadaQueryHandler handler,
        [FromQuery] ObterListaTransacoesDetalhadaQuery query,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Errors);
    }
}
