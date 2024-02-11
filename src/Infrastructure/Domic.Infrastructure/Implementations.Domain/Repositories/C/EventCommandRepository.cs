using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Entities;
using Domic.Core.Domain.Enumerations;
using Domic.Persistence.Contexts.C;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.C;

//Config
public partial class EventCommandRepository : IEventCommandRepository
{
    private readonly SQLContext _sqlContext;

    public EventCommandRepository(SQLContext sqlContext) => _sqlContext = sqlContext;
}

//Transaction
public partial class EventCommandRepository
{
    public void Add(Event entity) => _sqlContext.Events.Add(entity);

    public async Task AddAsync(Event entity, CancellationToken cancellationToken) 
        => await _sqlContext.Events.AddAsync(entity, cancellationToken);

    public void Change(Event entity) => _sqlContext.Events.Update(entity);

    public void Remove(Event entity) => _sqlContext.Events.Remove(entity);
}

//Query
public partial class EventCommandRepository
{
    public IEnumerable<Event> FindAll() => _sqlContext.Events.ToList();

    public IEnumerable<Event> FindAllWithOrdering(Order order, bool accending = true)
    {
        var entity = _sqlContext.Events;

        return order switch {
            Order.Date => entity.OrderBy(@event => @event.CreatedAt_EnglishDate).ToList(),
            Order.Id   => entity.OrderBy(@event => @event.Id).ToList(),
            _ => null
        };
    }
}