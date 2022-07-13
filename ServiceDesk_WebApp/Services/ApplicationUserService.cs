using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.Models;
using ServiceDesk_WebApp.RepositoryLayer;
using ServiceDesk_WebApp.Services.Interface;
using ServiceDesk_WebApp.ViewModel;

namespace ServiceDesk_WebApp.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IRepository _applictionUserRepo;
        public ApplicationUserService(IRepository applictionUserRepo)
        {
            _applictionUserRepo = applictionUserRepo;
        }
        public async Task<ServiceResult<LoginResponse>> LogInAsync(LoginRequest loginRequest)
        {
            try
            {
                var applicationUsersList = await _applictionUserRepo.GetAllAsync<User>(x => x.Email.ToLower().Equals(loginRequest.Email.ToLower()));


                if (applicationUsersList.Any())
                {
                    var applicationUser = applicationUsersList.FirstOrDefault(x => x.Password.Equals(loginRequest.Password));
                    if (applicationUser != null)
                    {
                        if (applicationUser.IsDeleted == 1)
                            return new ServiceResult<LoginResponse>(null, "Your Account is blocked, please contact admin.", true);

                        LoginResponse loginResponse = new();

                        loginResponse.UserRole = applicationUser.UserRole;
                        loginResponse.Id = applicationUser.Id;
                        loginResponse.Name = applicationUser.Name;
                        loginResponse.Email = applicationUser.Email;

                        return new ServiceResult<LoginResponse>(loginResponse, "User Details");

                    }
                    else
                        return new ServiceResult<LoginResponse>(null, "Invalid Credentials!");
                }
                else
                    return new ServiceResult<LoginResponse>(null, "User not found, please contact admin.");
            }
            catch (Exception ex)
            {
                return new ServiceResult<LoginResponse>(ex, ex.Message);
            }
        }

        public async Task<ServiceResult<User>> AddVendor(VendorViewModel vendorViewModel, int createdBy)
        {
            try
            {
                var contextModel = new User
                {
                    Name = vendorViewModel.VendorName,
                    Email = vendorViewModel.Email,
                    Password = vendorViewModel.Password,
                    UserRole = (int)UserRole.Vendor,
                    IsDeleted = 0
                };

                // Checks email for duplicate
                if (await _applictionUserRepo.IsExistsAsync<User>(au => au.Email.Equals(vendorViewModel.Email)))
                    return new ServiceResult<User>(contextModel, "Email Already Exist!", true);

                await _applictionUserRepo.AddAsync(contextModel, createdBy);
                var vendorModel = new Vendor
                {
                    UserId=contextModel.Id,
                    VendorName = vendorViewModel.VendorName,
                    VendorNo = vendorViewModel.VendorNo,
                    ResidencyStatus = vendorViewModel.ResidencyStatus,
                    Poremarks = vendorViewModel.PORemarks,
                    Currency = vendorViewModel.Currency,
                };
                await _applictionUserRepo.AddAsync(vendorModel, createdBy);


                return new ServiceResult<User>(contextModel, "Vendor added!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<User>(ex, ex.Message);
            }
        }

    }
}
