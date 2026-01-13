namespace ControleFinanceiro.Domain;

/// <summary>
/// Classe base para todos os agregados do domínio.
///
/// Mantém uma lista de eventos de domínio que podem ser disparados pelo agregado.
/// </summary>
public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _events = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events;

    protected void AddDomainEvent(IDomainEvent evt)
        => _events.Add(evt);

    public void ClearEvents()
        => _events.Clear();
}
