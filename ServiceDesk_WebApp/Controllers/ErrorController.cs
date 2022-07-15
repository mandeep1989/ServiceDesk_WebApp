using Microsoft.AspNetCore.Mvc;

namespace ServiceDesk_WebApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/NotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
