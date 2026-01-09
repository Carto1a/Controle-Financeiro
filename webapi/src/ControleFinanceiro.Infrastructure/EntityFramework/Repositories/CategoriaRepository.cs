using ControleFinanceiro.Domain.Transacoes;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.EntityFramework.Repositories;

public class CategoriaRepository(AppDbContext context) : ICategoriaRepository
{
    private readonly AppDbContext _context = context;

    public void Criar(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
    }

    public Task<Categoria?> BuscarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Categorias.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }
}
