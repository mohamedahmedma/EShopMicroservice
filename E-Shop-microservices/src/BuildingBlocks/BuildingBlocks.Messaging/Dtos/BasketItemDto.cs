namespace BuildingBlocks.Messaging.Dtos;

public record BasketItemDto(Guid ProductId, int Quantity, decimal Price , string ProductName , string Color);
