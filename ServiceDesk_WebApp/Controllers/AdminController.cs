using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.Models;
using ServiceDesk_WebApp.Services.Interface;
using ServiceDesk_WebApp.ViewModel;

namespace ServiceDesk_WebApp.Controllers
{
    
    public class AdminController : BaseController
    {
        private readonly IApplicationUserService _applicationUserService;
        public AdminController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }
        /// <summary>
        /// AdminDashBoard
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="1")]
        public IActionResult AdminDashBoard()
        {
          return View();
         
        }
        /// <summary>
        /// AdminViewRequest
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public IActionResult AdminViewRequest()
        {
            return View();

        }
        /// <summary>
        /// AddVendor
        /// </summary>
        /// <param name="vendorViewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> AddVendor(VendorViewModel vendorViewModel)
        {
            string link = Request.GetEncodedUrl().Replace(Request.Path.ToUriComponent(),"/");
            return GetResult(await _applicationUserService.AddVendor(vendorViewModel, User.GetUserId(),link));
        }
        /// <summary>
        /// GetAllVendors
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> GetAllVendors()
        {
            return GetResult(await _applicationUserService.GetAllVendors());
        }
        /// <summary>
        /// GetVendorById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> GetVendorById(int id)
        {
            return GetResult(await _applicationUserService.GetVendorById(id));
        }
        /// <summary>
        /// UpdateVendor
        /// </summary>
        /// <param name="vendorViewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> UpdateVendor(VendorViewModel vendorViewModel)
        {
            return GetResult(await _applicationUserService.UpdateVendor(vendorViewModel, User.GetUserId()));
        }
        /// <summary>
        /// GetVendorCountByDate
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> GetVendorCountByDate()
        {
            return GetResult(await _applicationUserService.GetVendorsCountByDate());
        }
        /// <summary>
        /// RemoveVendor
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> RemoveVendor(int Id)
        {
            return GetResult(await _applicationUserService.RemoveVendor(Id, User.GetUserId()));
        }
        /// <summary>
        /// Vendor
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public IActionResult Vendor()
        {
            return View();
        }
        /// <summary>
        /// ChangePasswordRequest
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public async Task<JsonResult> ChangePasswordRequest(string Email)
        {
            return GetResult(await _applicationUserService.ChangePasswordRequest(Email));
        }
        /// <summary>
        /// PasswordRequest
        /// </summary>
        /// <returns></returns>
        public IActionResult PasswordRequest()
        {
            return View();
        }
        /// <summary>
        /// PasswordRequestList
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> PasswordRequestList()
        {
            return GetResult(await _applicationUserService.GetPasswordRequests());
        }
        /// <summary>
        /// PasswordRequestList
        /// </summary>
        /// <param name="changePasswordRequestModel"></param>
        /// <returns></returns>
        public async Task<JsonResult> UpdatePassword(ChangePasswordRequestModel changePasswordRequestModel)
        {
            string link = Request.GetEncodedUrl().Replace(Request.Path.ToUriComponent(), "/");
            return GetResult(await _applicationUserService.UpdatePassword(changePasswordRequestModel,User.GetUserId(),link));
        }
        /// <summary>
        /// RemoveRequest
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> RemoveRequest(string Id)
        {
            return GetResult(await _applicationUserService.RemoveRequest(Id, User.GetUserId()));
        }
        /// <summary>
        /// GetPaymentRequest
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetPaymentRequest()
        {
            return GetResult(await _applicationUserService.GetPaymentRequests());
        }
    }
}
