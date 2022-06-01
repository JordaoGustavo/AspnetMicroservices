using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories.Interfaces;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger)
        {
            _discountRepository = discountRepository;
            _logger = logger;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepository.Get(request.ProductName);

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} not found!"));
            }

            _logger.LogInformation("Discount is retrived for productName {productName}", coupon.ProductName);

            return new CouponModel()
            {
                Id = coupon.Id,
                Amount = coupon.Amount,
                ProductName = coupon.ProductName,
                Description = coupon.Description
            };  
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = new Coupon()
            {
                Amount = request.Coupon.Amount,
                ProductName = request.Coupon.ProductName,
                Description = request.Coupon.Description
            };

            await _discountRepository.Create(coupon);

            var createdCoupon = await _discountRepository.Get(request.Coupon.ProductName);

            if (createdCoupon is null)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Erro to insert Discount with ProductName={request.Coupon.ProductName}!"));
            }

            _logger.LogInformation("Discount is successfully created. productName {productName}", coupon.ProductName);

            return new CouponModel()
            {
                Id = coupon.Id,
                Amount = coupon.Amount,
                ProductName = coupon.ProductName,
                Description = coupon.Description
            };
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = new Coupon()
            {
                Id = request.Coupon.Id,
                Amount = request.Coupon.Amount,
                ProductName = request.Coupon.ProductName,
                Description = request.Coupon.Description
            };

            await _discountRepository.Update(coupon);

            _logger.LogInformation("Discount is successfully updated. productName {productName}", coupon.ProductName);

            return new CouponModel()
            {
                Id = coupon.Id,
                Amount = coupon.Amount,
                ProductName = coupon.ProductName,
                Description = coupon.Description
            };
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _discountRepository.Delete(request.ProductName);

            _logger.LogInformation("Discount is successfully deleted. productName {productName}", request.ProductName);
            var response = new DeleteDiscountResponse()
            {
                Success = deleted
            };

            return response;
        }
    }
}
