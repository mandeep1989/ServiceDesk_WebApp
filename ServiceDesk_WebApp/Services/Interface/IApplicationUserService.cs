using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.Models;
using ServiceDesk_WebApp.ViewModel;

namespace ServiceDesk_WebApp.Services.Interface
{
    public interface IApplicationUserService
    {
         Task<ServiceResult<LoginResponse>> LogInAsync(LoginRequest loginRequest);
        Task<ServiceResult<User>> AddVendor(VendorViewModel vendorViewModel, int createdBy);
    }
}
