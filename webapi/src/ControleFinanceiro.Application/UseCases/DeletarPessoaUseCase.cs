using ControleFinanceiro.Application.DataLayer;
using ControleFinanceiro.Domain.Pessoas;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public class DeletarPessoaCommandHandler(IPessoaRepository pessoaRepository, IUnitOfWork unitOfWork)
{
    private readonly IPessoaRepository _pessoaRepository = pessoaRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(Guid pessoaId, CancellationToken cancellationToken = default)
    {
        var pessoa = await _pessoaRepository.BuscarPorIdAsync(pessoaId, cancellationToken);
        if (pessoa is null) return Result.Fail("Pessoa não encontrada");

        pessoa.MarcarDeletada();
        _pessoaRepository.Deletar(pessoa);

        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
