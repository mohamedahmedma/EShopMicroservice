﻿using BuildingBlocks.Messaging.Events;
using Mapster;
using MassTransit;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class BasketCheckoutEventHandler
    (ISender sender , ILogger<BasketCheckoutEventHandler> logger)
    : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        //TODO: Create new order and start order fullfillment process
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);
        var command = MapToCreateOrderCommand(context.Message);
        await sender.Send(command);
        throw new NotImplementedException();
    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        var orderId = Guid.NewGuid();
        var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine, message.Country, message.State, message.ZipCode);
        var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV, message.PaymentMethod);
        var orderitems = message.Items
                .Select(item => new OrderItemDto(orderId, item.ProductId, item.Quantity, item.Price))
                .ToList();

        // Use Adapt to map directly from the integration event to OrderDto
        //var orderDto = message.Adapt<OrderDto>() with
        //{
        //    Id = orderId,
        //    CustomerId = message.CustomerId,
        //    OrderName = message.EmailAddress,
        //    ShippingAddress = 
        //    Status = Ordering.Domain.Enums.OrderStatus.Pending,
        //    OrderItems = message.Items
        //        .Select(item => new OrderItemDto(orderId, item.ProductId, item.Quantity, item.Price))
        //        .ToList()
        //};

        var orderDto = new OrderDto(
           Id: orderId,
           CustomerId: message.CustomerId,
           OrderName: message.UserName,
           ShippingAddress: addressDto,
           BillingAddress: addressDto,
           Payment: paymentDto,
           Status: Ordering.Domain.Enums.OrderStatus.Pending,
           OrderItems:orderitems
           );
        return new CreateOrderCommand(orderDto);
    }
}
