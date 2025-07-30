using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            logger.LogInformation("Creating discount for product: {ProductName}", request.Coupon.ProductName);

            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                logger.LogError("Invalid coupon data for product: {ProductName}", request.Coupon.ProductName);
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon data"));
            }
            await dbContext.Coupons.AddAsync(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount created successfully for product: {ProductName}", request.Coupon.ProductName);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            logger.LogInformation("Deleting discount for product: {ProductName}", request.ProductName);

            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if (coupon == null)
            {
                logger.LogError("Invalid coupon data for product: {ProductName}", request.ProductName);
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }

            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount deleted successfully for product: {ProductName}", request.ProductName);
            return new DeleteDiscountResponse { Success = true };
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            logger.LogInformation("Retrieving discount for product: {ProductName}", request.ProductName);
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if (coupon == null)
            {
                logger.LogWarning("No discount found for product: {ProductName}", request.ProductName);

                coupon = new Coupon
                {
                    ProductName = "No Discount",
                    Description = "No Discount Description",
                    Amount = 0
                };

            }

            logger.LogInformation("Discount retrieved successfully for product: {ProductName}", request.ProductName);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;

        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            logger.LogInformation("Updating discount for product: {ProductName}", request.Coupon.ProductName);

            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                logger.LogError("Invalid coupon data for product: {ProductName}", request.Coupon.ProductName);
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon data"));
            }

            dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount updated successfully for product: {ProductName}", request.Coupon.ProductName);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
    }
}
