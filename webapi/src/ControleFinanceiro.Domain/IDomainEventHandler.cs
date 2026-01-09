namespace ControleFinanceiro.Domain;

public interface IDomainEvent { }

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T evt, CancellationToken cancellationToken = default);
}

