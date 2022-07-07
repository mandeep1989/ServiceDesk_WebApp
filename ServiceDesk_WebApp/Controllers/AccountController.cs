using Microsoft.AspNetCore.Mvc;

namespace ServiceDesk_WebApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
