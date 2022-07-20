using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.ViewModel;

namespace ServiceDesk_WebApp.Controllers
{
    [Authorize(Roles = "2")]
    public class VendorController : BaseController
    {

        private readonly IVendorService _vendorService;
       
        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }
    
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> AddEscalationForm(EscalationFormRequest escalationFormRequest)
        {
            return GetResult(await _vendorService.AddEscalationForm(escalationFormRequest, User.GetUserId()));
        }
        public async Task<JsonResult> CheckEscalationForm()
        {
            return GetResult(await _vendorService.CheckEscalationForm( User.GetUserId()));
        }
    }
}
