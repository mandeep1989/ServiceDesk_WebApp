using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService _notyf;
        
        public AccountController(IApplicationUserService applicationUserService, INotyfService notyf)
        {
            _applicationUserService = applicationUserService;
            _notyf = notyf;
        }
        public IActionResult Login()
        {
             return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var loginResponse = await _applicationUserService.LogInAsync(loginRequest);
            if (loginResponse.Data != null)
            {

                var data = loginResponse.Data;
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(AppClaimTypes.UserId, data.Id.ToString()));
                identity.AddClaim(new Claim(AppClaimTypes.UserName, data.Email));
                identity.AddClaim(new Claim(AppClaimTypes.Role, data.UserRole.ToString()));
                identity.AddClaim(new Claim(AppClaimTypes.Name, data.Name));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                //var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                //identity.AddClaim(new Claim(AppClaimTypes.UserId, data.Id.ToString()));
                //identity.AddClaim(new Claim(AppClaimTypes.UserName, data.Name));
                //identity.AddClaim(new Claim(AppClaimTypes.Role, data.UserRole.ToString()));
                //identity.AddClaim(new Claim(AppClaimTypes.Name, data.Name));
                //var principal = new ClaimsPrincipal(identity);
                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                if (data.UserRole == 0)
                {
                    _notyf.Success(loginResponse.Message,10);
                    return Json(new { isSuccess = true, url = "Admin/AdminDashBoard", message = loginResponse.Message });
                }

                else if (data.UserRole == (int)UserRole.Vendor)
                {
                    _notyf.Success(loginResponse.Message);
                    return Json(new { isSuccess = true, url = "Vendor/VendorDashboard", message = loginResponse.Message });
                }
                return Json(new { isSuccess = true, url = "Home/Index", message = loginResponse.Message });


            }
            else
            {
                _notyf.Error(loginResponse.Message);
                return Json(new { isSuccess = false, message = loginResponse.Message });
  
            }

        }

        public async Task<IActionResult> LogOut()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page    
            _notyf.Success("Logout !");
            return RedirectToAction("Login", "Account");
        }

    }
}
