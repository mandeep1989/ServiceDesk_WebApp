using javax.xml.crypto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.Models;
using ServiceDesk_WebApp.Services;
using ServiceDesk_WebApp.Services.Interface;
using ServiceDesk_WebApp.ViewModel;

namespace ServiceDesk_WebApp.Controllers
{
    
    public class AdminController : BaseController
    {
        private readonly IApplicationUserService _applicationUserService;
        public AdminController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }
        /// <summary>
        /// AdminDashBoard
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="1")]
        public IActionResult AdminDashBoard()
        {
          return View();
         
        }
        /// <summary>
        /// AdminViewRequest
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public IActionResult AdminViewRequest()
        {
            return View();

        }
        /// <summary>
        /// AddVendor
        /// </summary>
        /// <param name="vendorViewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> AddVendor(VendorViewModel vendorViewModel)
        {
            string link = Request.GetEncodedUrl().Replace(Request.Path.ToUriComponent(),"/");
            return GetResult(await _applicationUserService.AddVendor(vendorViewModel, User.GetUserId(),link));
        }
        /// <summary>
        /// GetAllVendors
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> GetAllVendors()
        {
            return GetResult(await _applicationUserService.GetAllVendors());
        }
        /// <summary>
        /// GetVendorById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> GetVendorById(int id)
        {
            return GetResult(await _applicationUserService.GetVendorById(id));
        }
        /// <summary>
        /// UpdateVendor
        /// </summary>
        /// <param name="vendorViewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> UpdateVendor(VendorViewModel vendorViewModel)
        {
            return GetResult(await _applicationUserService.UpdateVendor(vendorViewModel, User.GetUserId()));
        }
        /// <summary>
        /// GetVendorCountByDate
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> GetVendorCountByDate()
        {
            return GetResult(await _applicationUserService.GetVendorsCountByDate());
        }
        /// <summary>
        /// GetRequestCountByDate
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> GetRequestCountByDate()
        {
            return GetResult(await _applicationUserService.GetRequestCountByDate());
        }
        /// <summary>
        /// RemoveVendor
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> RemoveVendor(int Id)
        {
            return GetResult(await _applicationUserService.RemoveVendor(Id, User.GetUserId()));
        }
        /// <summary>
        /// Vendor
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public IActionResult Vendor()
        {
            return View();
        }
        /// <summary>
        /// ChangePasswordRequest
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public async Task<JsonResult> ChangePasswordRequest(string Email)
        {
            return GetResult(await _applicationUserService.ChangePasswordRequest(Email));
        }
        /// <summary>
        /// PasswordRequest
        /// </summary>
        /// <returns></returns>
        public IActionResult PasswordRequest()
        {
            return View();
        }
        /// <summary>
        /// PasswordRequestList
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> PasswordRequestList()
        {
            return GetResult(await _applicationUserService.GetPasswordRequests());
        }
        /// <summary>
        /// PasswordRequestList
        /// </summary>
        /// <param name="changePasswordRequestModel"></param>
        /// <returns></returns>
        public async Task<JsonResult> UpdatePassword(ChangePasswordRequestModel changePasswordRequestModel)
        {
            string link = Request.GetEncodedUrl().Replace(Request.Path.ToUriComponent(), "/");
            return GetResult(await _applicationUserService.UpdatePassword(changePasswordRequestModel,User.GetUserId(),link));
        }
        /// <summary>
        /// RemoveRequest
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public async Task<JsonResult> RemoveRequest(string Id)
        {
            return GetResult(await _applicationUserService.RemoveRequest(Id, User.GetUserId()));
        }
        /// <summary>
        /// GetPaymentRequest
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetPaymentRequest()
        {
            return GetResult(await _applicationUserService.GetPaymentRequests());
        }

		#region Contracts
		public IActionResult Contracts()
		{
			return View();
		}
		[Authorize(Roles = "1")]
        [HttpPost]
		public async Task<JsonResult> AddContract(IFormCollection data)
		{
			data.TryGetValue("ParentContractId", out var txtParentContractId);
			data.TryGetValue("ContractId", out var txtContractId);
			data.TryGetValue("Name", out var txtName);
			data.TryGetValue("ContractType", out var txtContractType);
			data.TryGetValue("ContractClassification", out var txtContractClassification);
			data.TryGetValue("Description", out var txtDescription);
			data.TryGetValue("StartDate", out var txtStartDate);
			data.TryGetValue("EndDate", out var txtEndDate);
			data.TryGetValue("Vendor", out var txtVendor);
			data.TryGetValue("MemoReference", out var txtMemoReference);
			data.TryGetValue("BudgetAmount", out var txtBudgetAmount);
			data.TryGetValue("Pid", out var txtPid);
			data.TryGetValue("ProjectName", out var txtProjectName);
			data.TryGetValue("CostCenter", out var txtCostCenter);
			data.TryGetValue("CostCenter2", out var txtCostCenter2);
			data.TryGetValue("Pid2", out var txtPid2);
			data.TryGetValue("CostCenter3", out var txtCostCenter3);
			data.TryGetValue("Pid3", out var txtPid3);
            data.TryGetValue("CostCenter4", out var txtCostCenter4);
			data.TryGetValue("Pid4", out var txtPid4);
			data.TryGetValue("Department", out var txtDepartment);
			data.TryGetValue("Currency", out var txtCurrency);
			data.TryGetValue("YearlyContractCostWithoutVat", out var txtYearlyContractCostWithoutVat);
			data.TryGetValue("CostBreakdown", out var txtCostBreakdown);
			var model = new ContractViewModel();
			model.ParentContractId = Convert.ToInt64(txtParentContractId);
			model.ContractId = Convert.ToInt64(txtContractId);
			model.Name = txtName;
			model.ContractType = txtContractType;
			model.ContractClassification = txtContractClassification;
			model.Description = txtDescription;
			model.StartDate = Convert.ToDateTime(txtStartDate, CultureInfo.CreateSpecificCulture("en-US")).ToString("dd,MM,yyyy", CultureInfo.CreateSpecificCulture("en-US"));
			model.EndDate = Convert.ToDateTime(txtEndDate, CultureInfo.CreateSpecificCulture("en-US")).ToString("dd,MM,yyyy", CultureInfo.CreateSpecificCulture("en-US"));
			model.Vendor = Convert.ToInt64(txtVendor);
			model.MemoReference = txtMemoReference;
			model.BudgetAmount = Convert.ToInt64(txtBudgetAmount);
            long.TryParse(txtPid, out var longtxtPid);
            model.Pid = longtxtPid;
			model.ProjectName = txtProjectName;
			model.CostCenter = txtCostCenter;
			model.CostCenter2 = txtCostCenter2;
			long.TryParse(txtPid2, out var longtxtPid2);
            model.Pid2 = longtxtPid2;
			model.CostCenter3 = txtCostCenter3; 
            long.TryParse(txtPid3, out var longtxtPid3);
			model.Pid3 = longtxtPid3;
            model.CostCenter4 = txtCostCenter4[0];
            
			long.TryParse(txtPid4[0], out var longtxtPid4);
			model.Pid4 = longtxtPid4;
			model.Department = Convert.ToInt64(txtDepartment);
			model.Currency = txtCurrency;
			model.YearlyContractCostWithoutVat = Convert.ToDouble(txtYearlyContractCostWithoutVat);
			model.CostBreakdown = txtCostBreakdown;

			
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
							case "Budgetattachment":
								model.BudgetAttchment = bytes;
								break;
							case "Contractattachment":
								model.ContractAttachment = bytes;
								break;
							case "OtherAttachments":
								model.OtherAttchment = bytes;
                                break;
                        }
                    }
                }
            }
            return GetResult( await _applicationUserService.AddContract(model,User.GetUserId()));
		}

        [Authorize(Roles = "1")]
        public async Task<JsonResult> GetAllContracts()
        {
            return GetResult(await _applicationUserService.GetAllContracts());
        } 
        public async Task<JsonResult> RemoveContract(long Id)
        {
            return GetResult(await _applicationUserService.DeleteContract(Id));
        }
        public async Task<JsonResult> UpdateContract(ContractViewModel ViewModel)
        {
            return GetResult(await _applicationUserService.UpdateContract(ViewModel, Convert.ToInt64(User.GetUserId())));
        }
        public async Task<JsonResult> GetContractById(long Id)
        {
            return GetResult(await _applicationUserService.GetContractById(Id));
        }
		#endregion
		#region Department
		[Authorize(Roles = "1")]
		public async Task<JsonResult> GetDepartments()
        {
			return GetResult(await _applicationUserService.GetDepartments());
		}
		#endregion
	}
}
