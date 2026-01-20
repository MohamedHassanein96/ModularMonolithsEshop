using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Shared.DDD;


namespace Shared.Data.Interceptors;

public class DispatchDomainEventInterceptor(IMediator mediator) :SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }
    public  override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    private async Task DispatchDomainEvents(DbContext context)
    {
        if (context is null)
            return;

        var aggregates = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity);


        var domainEvents = aggregates
            .SelectMany(a => a.ClearDomainEvents())
            .ToList();
  

        foreach (var domainEvent in domainEvents)
        {
            //Synchronous Communication (In-Process)
            //Event / Notification
            //0 or More Handlers
            //No Response
            await mediator.Publish(domainEvent);
        }

    }
}
