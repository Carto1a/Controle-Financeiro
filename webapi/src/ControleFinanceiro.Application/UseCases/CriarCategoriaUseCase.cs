using ControleFinanceiro.Application.DataLayer;
using ControleFinanceiro.Domain.Transacoes;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record CriarCategoriaCommand(string Nome, string Descricao, Finalidade Finalidade);

public class CriarCategoriaCommandHandler(ICategoriaRepository categoriaRepository, IUnitOfWork unitOfWork)
{
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(CriarCategoriaCommand request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = Categoria.Criar(request.Nome, request.Descricao, request.Finalidade);
        if (result.IsFailed) return Result.Fail(result.Errors);

        _categoriaRepository.Criar(result.Value);
        await _unitOfWork.SaveAsync(cancellationToken);

        return result.Value.Id;
    }
}

