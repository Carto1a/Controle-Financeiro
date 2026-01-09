using ControleFinanceiro.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Infrastructure.Controllers;

public class ResumosController : ControllerBase
{
    [HttpGet("financeiro/total")]
    public async Task<IActionResult> FinanceiroTotal(
        [FromServices] ObterResumoFinanceiroTotalQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);
        if (result.IsFailed)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("financeiro/pessoas")]
    public async Task<IActionResult> FinanceiroPessoas(
        [FromServices] ObterResumoFinanceiroPorPessoaQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);
        if (result.IsFailed)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("financeiro/categorias")]
    public async Task<IActionResult> FinanceiroCategorias(
        [FromServices] ObterResumoFinanceiroPorCategoriaQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);
        if (result.IsFailed)
            return BadRequest(result);

        return Ok(result);
    }
}
