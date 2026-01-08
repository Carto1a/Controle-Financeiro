namespace ControleFinanceiro.Domain.Pessoas;

public interface IPessoaRepository
{
    void Criar(Pessoa pessoa);
    void Deletar(Pessoa pessoa);
}
