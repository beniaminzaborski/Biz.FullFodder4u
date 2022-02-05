using Biz.FullFodder4u.Orders.API.DTOs;
using Biz.FullFodder4u.Orders.API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Biz.FullFodder4u.Orders.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private static readonly IList<OrderDto> orders = new List<OrderDto>();

    [HttpGet("at-day/{day}")]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IEnumerable<OrderDto>> GetOrdersAsync(DateTime day)
    {
        return orders;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetOrderAsync(Guid id)
    {
        var order = orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateOrderResponseDto), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateOrderAsync(CreateOrderRequestDto request)
    {
        var order = new OrderDto
        {
            Id = Guid.NewGuid(),
            Day = DateTime.UtcNow.Date,
            ManagedBy = "admin@email.com",
            ManagerIBAN = request.IBAN,
            ManagerPhoneNumber = request.PhoneNumber,
            PaymentMethod = request.PaymentMethod,
            Restaurant = request.Restaurant
        };

        orders.Add(order);

        return CreatedAtAction(nameof(GetOrderAsync), new { id = order.Id }, order);
    }

    [HttpPost("{orderId:Guid}/line")]
    [ProducesResponseType(/*typeof(CreateOrderLineResponseDto), */(int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateOrderLineAsync(Guid orderId, CreateOrderLineRequestDto request)
    {
        var order = orders.FirstOrDefault(o => o.Id == orderId);
        if (order != null)
        {
            order.Lines.Add(new OrderLineDto
            { 
                Id = Guid.NewGuid(),
                Name = request.Name,
                Quantity = request.Quantity,
                Price = request.Price
            });
        }

        return Created("", null);
    }

    [HttpPut("{orderId:Guid}/line/{orderLineId:Guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateOrderLineAsync(Guid orderId, Guid orderLineId, UpdateOrderLineRequestDto payload)
    {
        var order = orders.FirstOrDefault(o => o.Id == orderId);
        if (order is null)
        {
            throw new NotFoundException("Order not exists");
        }

        var orderLine = order.Lines.FirstOrDefault(l => l.Id == orderLineId);
        if (orderLine is null)
        {
            throw new NotFoundException("Order line not exists");
        }

        orderLine.Name = payload.Name;
        orderLine.Quantity = payload.Quantity;
        orderLine.Price = payload.Price;

        return NoContent();
    }

    [HttpDelete("{orderId:Guid}/line/{orderLineId:Guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteOrderLineAsync(Guid orderId, Guid orderLineId)
    {
        var order = orders.FirstOrDefault(o => o.Id == orderId);
        if (order is null)
        {
            throw new NotFoundException("Order not exists");
        }

        var orderLine = order.Lines.FirstOrDefault(l => l.Id == orderLineId);
        if (orderLine is null)
        {
            throw new NotFoundException("Order line not exists");
        }

        order.Lines.Add(orderLine);

        return NoContent();
    }
}
