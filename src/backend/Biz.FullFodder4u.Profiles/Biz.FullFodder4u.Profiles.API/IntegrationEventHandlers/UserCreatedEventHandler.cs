using Biz.FullFodder4u.IntegrationEvents;
using MassTransit;

namespace Biz.FullFodder4u.Profiles.API.IntegrationEventHandlers;

public class UserCreatedEventHandler : IConsumer<UserCreatedEvent>
{
    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        Console.WriteLine("User created: {0} ({1})", context.Message.Email, context.Message.UserId);
    }
}
