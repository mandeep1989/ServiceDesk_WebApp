using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.Data;
using ServiceDesk_WebApp.Services.Interface;
using ServiceDesk_WebApp.ViewModel;
using System.Security.Claims;

namespace ServiceDesk_WebApp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IApplicationUserService _applicationUserService;
        public AccountController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var loginResponse = await _applicationUserService.LogIn(loginRequest);
            if (loginResponse.Data != null)
            {

                var data = loginResponse.Data;
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(AppClaimTypes.UserId, data.Id.ToString()));
                identity.AddClaim(new Claim(AppClaimTypes.UserName, data.Name));
                identity.AddClaim(new Claim(AppClaimTypes.Role, data.UserRole.ToString()));
                identity.AddClaim(new Claim(AppClaimTypes.Name, data.Name));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (data.UserRole == (int)UserRole.Admin)
                {
                    return Json(new { isSuccess = true, url = "Home/AdminDashboard", message = loginResponse.Message });

                }

                else if (data.UserRole == (int)UserRole.Vendor)
                {
                    return Json(new { isSuccess = true, url = "Home/VendorDashboard", message = loginResponse.Message });
                }
                return Json(new { isSuccess = true, url = "Home/Index", message = loginResponse.Message });


            }
            else
            {
                return Json(new { isSuccess = false, message = loginResponse.Message });
            }

        }


    }
}
