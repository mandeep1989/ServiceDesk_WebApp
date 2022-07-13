using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        public async Task<JsonResult> AddVendor(VendorViewModel vendorViewModel)
        {
            return GetResult(await _applicationUserService.AddVendor(vendorViewModel, User.GetUserId()));
        }


        public IActionResult Vendor()
        {
            return View();
        }

    
    }
}
