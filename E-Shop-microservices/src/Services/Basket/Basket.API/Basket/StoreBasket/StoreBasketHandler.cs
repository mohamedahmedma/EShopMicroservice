﻿namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);
    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    public class StoreBasketHandler 
        (IBasketRepository repository , DiscountProtoService.DiscountProtoServiceClient discount)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await DeductDiscount(command.Cart, cancellationToken);

            //Store basket in database (use Marten upsert - if exist = update , if not exist create)
            await repository.StoreBasket(command.Cart, cancellationToken);


            return new StoreBasketResult(command.Cart.UserName);
        }
        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            //communicate with Discount.Grpc and calculate lastest prices of products into cart
            foreach (var item in cart.Items)
            {
                var coupon = await discount.GetDiscoutAsync(new GetDiscoutRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }
        }
    }
    
}
