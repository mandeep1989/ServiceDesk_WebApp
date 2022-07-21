using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.ViewModel;

using RouteAttribute = System.Web.Http.RouteAttribute;

namespace ServiceDesk_WebApp.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        IApplicationUserService _applicationUserService;
        public ErrorController(IApplicationUserService applicationUserService)
		{
            _applicationUserService = applicationUserService;
        }
        [AllowAnonymous]
        
        public IActionResult PageNotFound()
        {
            HttpContext.Response.StatusCode = 404;
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        [Route("Exception")]
        public async Task<IActionResult> Exception()
        {
            var error = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            int statusCode = error is HttpResponseException ? (error as HttpResponseException).Status : HttpContext.Response.StatusCode;


            if (statusCode == 500)
            {
                ViewBag.ErrorCode = (await LogErrorAsync(error)).ToString().ToUpper();
                HttpContext.Response.StatusCode = 500;
            }

            return statusCode switch
            {
                404 => RedirectToAction(nameof(PageNotFound)),
                500 => View("InternalServerError"),
                _ => View("InternalServerPage")
            };
        }
        public async Task<long> LogErrorAsync(Exception error = null)
        {
            ErrorRequest request = new()
            {
                Information = $"Exception Message: {error.Message}, Exception Stack Trace: {error.StackTrace}",
                UserId = User.GetUserId()
            };
            return (await _applicationUserService.CreateErrorLog(request)).Data;
        }
   
    }

}
