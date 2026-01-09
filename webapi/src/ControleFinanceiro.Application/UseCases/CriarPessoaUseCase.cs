using ControleFinanceiro.Application.DataLayer;
using ControleFinanceiro.Domain.Pessoas;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record CriarPessoaCommand(string Nome, DateTime DataNascimento);

public class CriarPessoaCommandHandler(IPessoaRepository pessoaRepository, IUnitOfWork unitOfWork)
{
    private readonly IPessoaRepository _pessoaRepository = pessoaRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(CriarPessoaCommand request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = Pessoa.Criar(request.Nome, request.DataNascimento.ToUniversalTime());
        if (result.IsFailed) return Result.Fail(result.Errors);

        _pessoaRepository.Criar(result.Value);

        await _unitOfWork.SaveAsync(cancellationToken);

        return result.Value.Id;
    }
}
