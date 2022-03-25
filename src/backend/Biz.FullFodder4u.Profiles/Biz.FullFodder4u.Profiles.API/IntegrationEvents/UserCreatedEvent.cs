namespace Biz.FullFodder4u.IntegrationEvents;

public class UserCreatedEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
}
