using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.ViewModel;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;

namespace ServiceDesk_WebApp.Controllers
{
    [Authorize(Roles = "2")]
    public class VendorController : BaseController
    {

        private readonly IVendorService _vendorService;
        private readonly Microsoft.Extensions.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly string authToken;
        public string input_data = @"{
    ""list_info"": {
        ""row_count"": 100,
        ""start_index"": 1,
        ""sort_field"": ""id"",
        ""sort_order"": ""asc"",
        ""get_total_count"": true
        }
    }";
        private int startIndex = 1;
        //    public static object paramse = new Dictionary<string, string>
        //        {
        //            "input_data",
        //            input_data
        //};


        /// <summary>
        /// VendorController
        /// </summary>
        /// <param name="vendorService"></param>
        public VendorController(IVendorService vendorService, Microsoft.Extensions.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _vendorService = vendorService;
            _hostingEnvironment = hostingEnvironment;
            authToken = configuration.GetValue<string>("Authtoken");
        }
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// PaymentRequest
        /// </summary>
        /// <returns></returns>
        public IActionResult PaymentRequest()
        {
            return View();
        }
        /// <summary>
        /// GetPaymentRequest
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetPaymentRequest()
        {
            return GetResult(await _vendorService.GetPaymentRequests(User.GetUserId()));
        }
        /// <summary>
        /// AddEscalationForm
        /// </summary>
        /// <param name="escalationFormRequest"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddEscalationForm(EscalationFormRequest escalationFormRequest)
        {
            return GetResult(await _vendorService.AddEscalationForm(escalationFormRequest, User.GetUserId()));
        }
        /// <summary>
        /// CheckEscalationForm
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> CheckEscalationForm()
        {
            return GetResult(await _vendorService.CheckEscalationForm(User.GetUserId()));
        }
        /// <summary>
        /// AddRequestForm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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
            data.TryGetValue("VATAmount", out var VATAmount);
            data.TryGetValue("Details", out var Details);
            data.TryGetValue("PaymentMode", out var PaymentMode);
            data.TryGetValue("BankName", out var BankName);
            data.TryGetValue("IBAN", out var IBAN);
            data.TryGetValue("AccountName", out var AccountName);
            data.TryGetValue("AccountNumber", out var AccountNumber);
            data.TryGetValue("SwiftCode", out var SwiftCode);
            data.TryGetValue("Branch", out var Branch);
            data.TryGetValue("Contract", out var Contract);
            model.ContractTitle = ContractTitle;
            //model.StartDate =ConvertDateTimeToDate(StartDate, CultureInfo.CurrentCulture.ToString());
            model.StartDate = Convert.ToDateTime(StartDate, CultureInfo.CreateSpecificCulture("en-US")).ToString("dd,MM,yyyy", CultureInfo.CreateSpecificCulture("en-US"));
            // model.StartDate = Convert.ToDateTime(StartDate).ToString("dd,MM,yyyy");
            model.EndDate = Convert.ToDateTime(EndDate, CultureInfo.CreateSpecificCulture("en-US")).ToString("dd,MM,yyyy", CultureInfo.CreateSpecificCulture("en-US"));
            // var bb = aa.ToString("dd-MM-yyyy", CultureInfo.CreateSpecificCulture("en-US"));       
            //model.EndDate = Convert.ToDateTime(EndDate);       
            model.Department = Department;
            model.InvoiceNo = InvoiceNumber;
            model.InvoiceAmount = InvoiceAmount;
            model.Vatamount= VATAmount;
            model.ProjectName = ProjectName;
            model.Classification = Classification;
            model.ContractRefType = ContractRefType;
            model.Details = Details;
            //// model.InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("dd,MM,yyyy");
            model.InvoiceDate = Convert.ToDateTime(InvoiceDate, CultureInfo.CreateSpecificCulture("en-US")).ToString("dd,MM,yyyy", CultureInfo.CreateSpecificCulture("en-US"));
            model.ApplicationName = ApplicationName;
            model.BankName = BankName;
            model.PaymentMode = PaymentMode;
            model.Iban = IBAN;
            model.AccountName = AccountName;
            model.AccountNumber = AccountNumber;
            model.SwiftCode = SwiftCode;
            model.Branch = Branch;
            model.Contract = Contract;

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
                        switch (file.Name)
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
            return GetResult(await _vendorService.AddPaymentForm(model, User.GetUserId()));


        }
        private string ConvertDateTimeToDate(string dateTimeString, String langCulture)
        {

            CultureInfo culture = new CultureInfo(langCulture);
            DateTime dt = DateTime.MinValue;

            if (DateTime.TryParse(dateTimeString, out dt))
            {
                return dt.ToString("d", culture);
            }
            return dateTimeString;
        }
        /// <summary>
        /// Fetching details from api
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetCsv()
        {
            //var fileName = "Book1.csv";
            //string contentRootPath = _hostingEnvironment.ContentRootPath+"\\wwwroot\\CSVFile\\";
            //return PhysicalFile(Path.Combine(contentRootPath, fileName), "application/csv", fileName);
            var userdomain = User.GetEmail().Split('@')[1];
            //return GetResult(await _vendorService.GetContracts(userdomain));
            return GetResult(await _vendorService.GetContractsForDropdown(userdomain));
        }
        [HttpGet]
        public async Task<JsonResult> GetContractById(int id)
        {
            //return GetResult(await _vendorService.GetContractById(id));
            return GetResult(await _vendorService.GetContractById(id));
        } 
        public async Task<JsonResult> GetBankDetails()
        {
            return GetResult(await _vendorService.GetBankDetails(User.GetUserId()));
        }
    }
}
