using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.Models;
using ServiceDesk_WebApp.ViewModel;

namespace ServiceDesk_WebApp.Services.Interface
{
    public interface IApplicationUserService
    {
        /// <summary>
        /// LogInAsync
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        Task<ServiceResult<LoginResponse>> LogInAsync(LoginRequest loginRequest);
        /// <summary>
        /// AddVendor
        /// </summary>
        /// <param name="vendorViewModel"></param>
        /// <param name="createdBy"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        Task<ServiceResult<User>> AddVendor(VendorViewModel vendorViewModel, int createdBy, string link);
        /// <summary>
        /// GetAllVendors
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<VendorViewModel>>> GetAllVendors();
        /// <summary>
        /// GetVendorById
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ServiceResult<VendorViewModel>> GetVendorById(int Id);
        /// <summary>
        /// UpdateVendor
        /// </summary>
        /// <param name="vendorViewModel"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> UpdateVendor(VendorViewModel vendorViewModel, int modifiedBy);
        /// <summary>
        /// RemoveVendor
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> RemoveVendor(int Id, int modifiedBy);
        /// <summary>
        /// GetVendorsCountByDate
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<GetVendorCount>> GetVendorsCountByDate();
        /// <summary>
        /// ChangePasswordRequest
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> ChangePasswordRequest(string Email);
        /// <summary>
        /// GetPasswordRequests
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<PasswordResetRequest>>> GetPasswordRequests();
        /// <summary>
        /// UpdatePassword
        /// </summary>
        /// <param name="changePasswordRequestModel"></param>
        /// <param name="modifiedBy"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> UpdatePassword(ChangePasswordRequestModel changePasswordRequestModel, int modifiedBy, string link);
        /// <summary>
        /// RemoveRequest
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> RemoveRequest(string Id, int modifiedBy);
        /// <summary>
        /// CreateErrorLog
        /// </summary>
        /// <param name="errorRequest"></param>
        /// <returns></returns>
        Task<ServiceResult<string>> CreateErrorLog(ErrorRequest errorRequest);
        /// <summary>
        /// GetPaymentRequests
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<PaymentRequestModel>>> GetPaymentRequests();
    }
}
