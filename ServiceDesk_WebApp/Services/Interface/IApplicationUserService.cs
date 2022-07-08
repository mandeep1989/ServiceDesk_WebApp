using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.ViewModel;

namespace ServiceDesk_WebApp.Services.Interface
{
    public interface IApplicationUserService
    {
        public Task<ServiceResult<LoginResponse>> LogInAsync(LoginRequest loginRequest);
    }
}
