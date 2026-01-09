namespace ControleFinanceiro.Domain.Pessoas;

public sealed record PessoaDeletadaEvent(Guid PessoaId) : IDomainEvent;
