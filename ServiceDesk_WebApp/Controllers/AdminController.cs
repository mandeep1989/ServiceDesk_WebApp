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
      //  [Authorize(Roles ="1")]
        public IActionResult AdminDashBoard()
        {
          return View();
         
        }

        [Authorize(Roles = "1,2")]
        public async Task<JsonResult> AddVendor(VendorViewModel vendorViewModel)
        {
            string link = Request.GetEncodedUrl().Replace(Request.Path.ToUriComponent(),"");
            return GetResult(await _applicationUserService.AddVendor(vendorViewModel, User.GetUserId(),link));
        }
        [Authorize(Roles = "1")]
        public async Task<JsonResult> GetAllVendors()
        {
            return GetResult(await _applicationUserService.GetAllVendors());
        }
        [Authorize(Roles = "1")]
        public async Task<JsonResult> GetVendorById(int id)
        {
            return GetResult(await _applicationUserService.GetVendorById(id));
        }
        [Authorize(Roles = "1")]
        public async Task<JsonResult> UpdateVendor(VendorViewModel vendorViewModel)
        {
            return GetResult(await _applicationUserService.UpdateVendor(vendorViewModel, User.GetUserId()));
        }
        [Authorize(Roles = "1")]
        public async Task<JsonResult> GetVendorCountByDate()
        {
            return GetResult(await _applicationUserService.GetVendorsCountByDate());
        }
        [Authorize(Roles = "1")]
        public async Task<JsonResult> RemoveVendor(int Id)
        {
            return GetResult(await _applicationUserService.RemoveVendor(Id, User.GetUserId()));
        }
        [Authorize(Roles = "1")]
        public IActionResult Vendor()
        {
            return View();
        }
        public async Task<JsonResult> ChangePasswordRequest(string Email)
        {
            return GetResult(await _applicationUserService.ChangePasswordRequest(Email));
        }
        public IActionResult PasswordRequest()
        {
            return View();
        }
        public async Task<JsonResult> ChangePasswordRequest(string Email)
        {
            return GetResult(await _applicationUserService.ChangePasswordRequest(Email));
        }
        public async Task<JsonResult> PasswordRequestList()
        {
            return GetResult(await _applicationUserService.GetPasswordRequests());
        }

    }
}
