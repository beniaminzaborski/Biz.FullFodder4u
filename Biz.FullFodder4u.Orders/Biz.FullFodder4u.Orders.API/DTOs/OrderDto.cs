namespace Biz.FullFodder4u.Orders.API.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }

    public DateTime Day { get; set; }

    public string ManagedBy { get; set; }

    public int PaymentMethod { get; set; }

    public string? ManagerPhoneNumber { get; set; }

    public string? ManagerIBAN { get; set; }

    public string Restaurant { get; set; }

    public IList<OrderLineDto> Lines { get; set; } = new List<OrderLineDto>();
}
