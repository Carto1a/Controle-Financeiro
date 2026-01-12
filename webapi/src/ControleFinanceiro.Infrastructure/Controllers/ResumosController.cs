using ControleFinanceiro.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Infrastructure.Controllers;

[ApiController]
[Area("Resumos")]
[Route("[controller]")]
public class ResumosController : ControllerBase
{
    [HttpGet("financeiro/total")]
    public async Task<IActionResult> FinanceiroTotal(
        [FromServices] ObterResumoFinanceiroTotalQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("financeiro/pessoas")]
    public async Task<IActionResult> FinanceiroPessoas(
        [FromServices] ObterResumoFinanceiroPorPessoaQueryHandler handler,
        [FromQuery] ObterResumoFinanceiroPaginatedQuery query,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("financeiro/categorias")]
    public async Task<IActionResult> FinanceiroCategorias(
        [FromServices] ObterResumoFinanceiroPorCategoriaQueryHandler handler,
        [FromQuery] ObterResumoFinanceiroPaginatedQuery query,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}
