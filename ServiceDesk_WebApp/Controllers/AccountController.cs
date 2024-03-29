﻿using AspNetCoreHero.ToastNotification.Abstractions;
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
        /// <summary>
        /// LogIn Request
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost]
       
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var loginResponse = await _applicationUserService.LogInAsync(loginRequest);
            if (loginResponse.Data != null)
            {

                var data = loginResponse.Data;
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(AppClaimTypes.UserId, data.Id.ToString()));
                identity.AddClaim(new Claim(AppClaimTypes.Email, data.Email));
                identity.AddClaim(new Claim(AppClaimTypes.Role, data.UserRole.ToString()));
                identity.AddClaim(new Claim(AppClaimTypes.Name, data.Name));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (data.UserRole == (int)UserRole.Admin)
                {

                    return Json(new { isSuccess = true, url = "/Admin/AdminDashBoard", message = loginResponse.Message });
                }

                else if (data.UserRole == (int)UserRole.Vendor)
                {

                    return Json(new { isSuccess = true, url = "/Vendor/Index", message = loginResponse.Message });
                }
                return Json(new { isSuccess = false, message = loginResponse.Message });


            }
            else
            {
                _notyf.Error(loginResponse.Message);
                return Json(new { isSuccess = false, message = loginResponse.Message });

            }

        }
        /// <summary>
        /// Account LogOut
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");

        }
        /// <summary>
        /// ForgetPassword
        /// </summary>
        /// <returns></returns>
        public IActionResult ForgetPassWord()
        {
            return View();
        }

     

    }
}
