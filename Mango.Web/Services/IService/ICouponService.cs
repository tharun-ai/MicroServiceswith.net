using Mango.Web.Models;
using Mango.Web.Models.Dto;

namespace Mango.Web.Services.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponAsync(string CouponCode);
        Task<ResponseDto?> GetAllCouponsAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> CreateCouponsAsync(CouponDto couponDto);
        Task<ResponseDto?> UpdateCouponsAsync(CouponDto couponDto);
        Task<ResponseDto?> DeleteCouponsAsync(int id);

    }
}
