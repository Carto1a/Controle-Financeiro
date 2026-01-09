using ControleFinanceiro.Domain.Pessoas;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.EntityFramework.Repositories;

public class PessoaRepository(AppDbContext context) : IPessoaRepository
{
    private readonly AppDbContext _context = context;

    public void Criar(Pessoa pessoa)
    {
        _context.Pessoas.Add(pessoa);
    }

    public Task<Pessoa?> BuscarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Pessoas.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public void Deletar(Pessoa pessoa)
    {
        _context.Pessoas.Remove(pessoa);
    }
}
