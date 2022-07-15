using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceDesk_WebApp.Controllers
{
    public class VendorController : BaseController
    {
        [Authorize(Roles="2")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
