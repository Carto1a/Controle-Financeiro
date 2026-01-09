using ControleFinanceiro.Application.DataLayer;
using ControleFinanceiro.Domain.Pessoas;
using ControleFinanceiro.Domain.Transacoes;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record CriarTransacaoCommand(string Descricao, decimal Valor, TipoTransacao Tipo, Guid PessoaId, Guid CategoriaId, DateTime? Data);

public class CriarTransacaoCommandHandler(
    ITransacaoRepository transacaoRepository,
    IPessoaRepository pessoaRepository,
    ICategoriaRepository categoriaRepository,
    IUnitOfWork unitOfWork)
{
    private readonly ITransacaoRepository _transacaoRepository = transacaoRepository;
    private readonly IPessoaRepository _pessoaRepository = pessoaRepository;
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(CriarTransacaoCommand request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var pessoa = await _pessoaRepository.BuscarPorIdAsync(request.PessoaId, cancellationToken);
        if (pessoa is null) return Result.Fail("não foi possivel achar o usuario solicitado");

        var categoria = await _categoriaRepository.BuscarPorIdAsync(request.CategoriaId, cancellationToken);
        if (categoria is null) return Result.Fail("não foi possivel achar a categoria solicitada");

        var result = CadastrarTransacaoService.Cadastrar(pessoa, categoria, request.Descricao, request.Valor, request.Tipo, request.Data?.ToUniversalTime());
        if (result.IsFailed) return Result.Fail(result.Errors);

        _transacaoRepository.Criar(result.Value);
        await _unitOfWork.SaveAsync(cancellationToken);

        return result.Value.Id;
    }
}
