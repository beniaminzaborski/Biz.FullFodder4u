namespace Biz.FullFodder4u.Orders.API.DTOs;

public class OrderLineDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}
