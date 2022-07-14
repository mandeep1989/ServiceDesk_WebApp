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

        public IActionResult AdminDashBoard()
        {
          return View();
         
        }

    
        public async Task<JsonResult> AddVendor(VendorViewModel vendorViewModel)
        {
            string link = Request.GetEncodedUrl().Replace(Request.Path.ToUriComponent(), "Home/Index");
            return GetResult(await _applicationUserService.AddVendor(vendorViewModel, User.GetUserId(),link));
        }
        public async Task<JsonResult> GetAllVendors()
        {
            return GetResult(await _applicationUserService.GetAllVendors());
        }
        public async Task<JsonResult> GetVendorById(int id)
        {
            return GetResult(await _applicationUserService.GetVendorById(id));
        }
        public async Task<JsonResult> UpdateVendor(VendorViewModel vendorViewModel)
        {
            return GetResult(await _applicationUserService.UpdateVendor(vendorViewModel, User.GetUserId()));
        }
        public async Task<JsonResult> RemoveVendor(int Id)
        {
            return GetResult(await _applicationUserService.RemoveVendor(Id, User.GetUserId()));
        }
        public IActionResult Vendor()
        {
            return View();
        }

    
    }
}
