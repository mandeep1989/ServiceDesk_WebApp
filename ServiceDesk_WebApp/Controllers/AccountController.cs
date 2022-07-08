using Microsoft.AspNetCore.Mvc;
using ServiceDesk_WebApp.Data;

namespace ServiceDesk_WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ServiceDesk_WebAppContext _context;
        public AccountController(ServiceDesk_WebAppContext context)
        {
            _context=context;
        }
        public IActionResult Login()
        {
            return View();
        }


    }
}
