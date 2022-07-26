using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.ViewModel;
using System.Net.Http.Headers;

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

        public IActionResult PaymentRequest()
        {
            return View();
        }
        public async Task<JsonResult> GetPaymentRequest()
        {
            return GetResult(await _vendorService.GetPaymentRequests(User.GetUserId()));
        }
        public async Task<JsonResult> AddEscalationForm(EscalationFormRequest escalationFormRequest)
        {
            return GetResult(await _vendorService.AddEscalationForm(escalationFormRequest, User.GetUserId()));
        }
        public async Task<JsonResult> CheckEscalationForm()
        {
            return GetResult(await _vendorService.CheckEscalationForm( User.GetUserId()));
        }
        [HttpPost]
        public async Task<JsonResult> AddRequestForm(IFormCollection data/*PaymentRequestModel paymentRequest*/)
        
        {
            var model = new PaymentRequestModel();


            data.TryGetValue("ContractTitle", out var ContractTitle);
            data.TryGetValue("StartDate", out var StartDate);
            data.TryGetValue("EndDate", out var EndDate); 
            data.TryGetValue("Department", out var Department); 
            data.TryGetValue("Classification", out var Classification); 
            data.TryGetValue("ApplicationName", out var ApplicationName); 
            data.TryGetValue("ContractRefType", out var ContractRefType); 
            data.TryGetValue("ProjectName", out var ProjectName); 
            data.TryGetValue("InvoiceNumber", out var InvoiceNumber); 
            data.TryGetValue("InvoiceAmount", out var InvoiceAmount); 
            data.TryGetValue("InvoiceDate", out var InvoiceDate); 
            data.TryGetValue("Details", out var Details); 
            data.TryGetValue("PaymentMode", out var PaymentMode); 
            data.TryGetValue("BankName", out var BankName); 
            data.TryGetValue("IBAN", out var IBAN); 
            data.TryGetValue("AccountName", out var AccountName); 
            data.TryGetValue("AccountNumber", out var AccountNumber); 
            data.TryGetValue("SwiftCode", out var SwiftCode); 
            data.TryGetValue("Branch", out var Branch);
            model.ContractTitle = ContractTitle;
            model.StartDate = Convert.ToDateTime(StartDate).ToString("dd,MM,yyyy");
            model.EndDate = Convert.ToDateTime(EndDate).ToString("dd,MM,yyyy");       
            model.Department = Department;
            model.InvoiceNo = InvoiceNumber;
            model.InvoiceAmount = InvoiceAmount;
            model.ProjectName = ProjectName;
            model.Classification = Classification;
            model.ContractRefType = ContractRefType;
            model.Details = Details;
            model.InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("dd,MM,yyyy"); 
            model.ApplicationName = ApplicationName;
            model.BankName = BankName;
            model.PaymentMode = PaymentMode;
            model.Iban = IBAN;
            model.AccountName = AccountName;
            model.AccountNumber = AccountNumber;
            model.SwiftCode = SwiftCode;
            model.Branch = Branch;
          
            var files = data.Files;
           
                foreach (var file in files)
                {
                if (file.Length > 0)
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        string contentAsString = reader.ReadToEnd();
                        byte[] bytes = new byte[contentAsString.Length * sizeof(char)];
                        System.Buffer.BlockCopy(contentAsString.ToCharArray(), 0, bytes, 0, bytes.Length);
                        switch(file.Name)
                        {

                            case "orginalInvoice":
                             model.OrginalInvoice = bytes;
                                break;
                            case "CopyOfApproval":
                                model.OrginalInvoice = bytes;
                                break;
                            case "ServiceConfirmation":
                                model.OrginalInvoice = bytes;
                                break;
                        }

                    }
                }
            }
            return GetResult(await _vendorService.AddPaymentForm(model,User.GetUserId()));

            
        }
    }
}
