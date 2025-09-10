﻿namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(OrderDto order) : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.order.OrderName).NotEmpty().WithMessage("Name is requried");
        RuleFor(x => x.order.CustomerId).NotNull().WithMessage("CustomerId is required");
        RuleFor(x => x.order.OrderItems).NotEmpty().WithMessage("OrderItems should not be empty");
    }
}