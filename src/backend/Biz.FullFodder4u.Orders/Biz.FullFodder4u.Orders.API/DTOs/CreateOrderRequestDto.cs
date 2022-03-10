namespace Biz.FullFodder4u.Orders.API.DTOs;

public class CreateOrderRequestDto
{
    public string Restaurant { get; set; }

    public int PaymentMethod { get; set; }

    public string? PhoneNumber { get; set; }

    public string? IBAN { get; set; }
}
