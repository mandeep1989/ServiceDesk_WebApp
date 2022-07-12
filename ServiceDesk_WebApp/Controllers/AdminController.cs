using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceDesk_WebApp.Models;

namespace ServiceDesk_WebApp.Controllers
{
    
    public class AdminController : Controller
    {
       
        public IActionResult AdminDashBoard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddVendor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddVendor(Vendor vendor)
        {
            return View();
        }

        public IActionResult Vendor()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetVendor()
        {
            List<Vendor> vendors = new List<Vendor>();
            vendors.Add(new Vendor
            {
                Id=1,
                VendorNo="1",
                VendorName="Jhon",
                ResidencyStatus="ParkStreet",
                PORemarks="marked",
                Currency="Dollar",
                Email="Jhon@gmail.com"

            });
            return Json(vendors);
        }
    }
}
