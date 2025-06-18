using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService
        (DiscountContext dbcontext , ILogger<DiscountService> logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscout(GetDiscoutRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext
                .Coupons
                .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (coupon == null)
            {
                coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            }
            logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amont : {amount}", coupon.ProductName , coupon.Amount);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public async override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupone = request.Coupon.Adapt<Coupon>();
            if (coupone is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
            }
            dbcontext.Coupons.Add(coupone);
            await dbcontext.SaveChangesAsync();
            logger.LogInformation("Discount is successfully created. ProductName : {productname}", coupone.ProductName);
            var coponModel = coupone.Adapt<CouponModel>();
            return coponModel;
        }

        public async override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupone = request.Coupon.Adapt<Coupon>();
            if (coupone is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
            }
            dbcontext.Coupons.Update(coupone);
            await dbcontext.SaveChangesAsync();
            logger.LogInformation("Discount is successfully Updated. ProductName : {productname}", coupone.ProductName);
            var coponModel = coupone.Adapt<CouponModel>();
            return coponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext
                .Coupons
                .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));

            dbcontext.Coupons.Remove(coupon);
            await dbcontext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully deleted. productName : {productname}", request.ProductName);

            return new DeleteDiscountResponse { Success = true };
        }
    }
}
