namespace ControleFinanceiro.Domain;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _events = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events;

    protected void AddDomainEvent(IDomainEvent evt)
        => _events.Add(evt);

    public void ClearEvents()
        => _events.Clear();
}
