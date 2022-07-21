using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.Models;
using ServiceDesk_WebApp.ViewModel;

namespace ServiceDesk_WebApp.Services.Interface
{
    public interface IApplicationUserService
    {
        Task<ServiceResult<LoginResponse>> LogInAsync(LoginRequest loginRequest);
        Task<ServiceResult<User>> AddVendor(VendorViewModel vendorViewModel, int createdBy, string link);
        Task<ServiceResult<IEnumerable<VendorViewModel>>> GetAllVendors();
        Task<ServiceResult<VendorViewModel>> GetVendorById(int Id);
        Task<ServiceResult<bool>> UpdateVendor(VendorViewModel vendorViewModel, int modifiedBy);
        Task<ServiceResult<bool>> RemoveVendor(int Id, int modifiedBy);
        Task<ServiceResult<GetVendorCount>> GetVendorsCountByDate();
        Task<ServiceResult<bool>> ChangePasswordRequest(string Email);
        Task<ServiceResult<IEnumerable<PasswordResetRequest>>> GetPasswordRequests();
        Task<ServiceResult<bool>> UpdatePassword(ChangePasswordRequestModel changePasswordRequestModel, int modifiedBy, string link);
        Task<ServiceResult<bool>> RemoveRequest(string Id, int modifiedBy);
        Task<ServiceResult<long>> CreateErrorLog(ErrorRequest errorRequest);

    }
}
