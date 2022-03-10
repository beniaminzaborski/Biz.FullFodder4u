namespace Biz.FullFodder4u.Identity.API.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public string PasshowrdHash { get; set; }
}
