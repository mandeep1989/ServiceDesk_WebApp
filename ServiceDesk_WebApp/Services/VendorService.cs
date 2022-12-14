using com.sun.tools.corba.se.idl.constExpr;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using RestSharp;
using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.ViewModel;
using System.Globalization;

namespace ServiceDesk_WebApp.Services
{
    public class VendorService : IVendorService
    {
        private readonly IRepository _applictionUserRepo;
        private readonly string authToken;
        private readonly string From;
        private readonly string SenderPassword;
        private readonly string Host;
        private readonly int Port;
        private readonly int PORTALID;
        private readonly ServiceDesk_WebAppContext _context;
        public string input_data = @"{
    ""list_info"": {
        ""row_count"": 100,
        ""start_index"": 0,
        ""sort_field"": ""id"",
        ""sort_order"": ""asc"",
        ""get_total_count"": true
        }
    }";
        private int startIndex = 1;
        public VendorService(IRepository applictionUserRepo, IConfiguration configuration, ServiceDesk_WebAppContext context)
        {
            _applictionUserRepo = applictionUserRepo;
            authToken = configuration.GetValue<string>("Authtoken");
            From = configuration.GetValue<string>("EmailSettings:From");
            SenderPassword = configuration.GetValue<string>("EmailSettings:Password");
            Host = configuration.GetValue<string>("EmailSettings:Host");
            Port = configuration.GetValue<int>("EmailSettings:Port");
            PORTALID = configuration.GetValue<int>("PORTALID");
            _context = context;

        }
        /// <summary>
        /// Add Escalation Form
        /// </summary>
        /// <param name="escalationForm"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> AddEscalationForm(EscalationFormRequest escalationForm, int createdBy)
        {
            try
            {
                var contextModel = new EscalationMatrix
                {
                    CompanyName = escalationForm.CompanyName,
                    CompanyEmail = escalationForm.CompanyEmail,
                    CompanyPhone = escalationForm.CompanyPhone,
                    ContactEmail = escalationForm.ContactEmail,
                    ContactName = escalationForm.ContactName,
                    ContactPhone = escalationForm.ContactPhone,
                    UserId = createdBy,
                    IsDeleted = 0
                };

                // Checks email for duplicate
                //if (await _applictionUserRepo.IsExistsAsync<EscalationMatrix>(au => au.CompanyName.Equals(escalationForm.CompanyName) || au.ContactEmail.Equals(escalationForm.ContactEmail)))
                //    return new ServiceResult<bool>(true, "Email Already Exist!", true);

                await _applictionUserRepo.AddAsync(contextModel, createdBy);

                return new ServiceResult<bool>(true, "Form added!");
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {

                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.Date.ToString("dd,MM,yyyy", CultureInfo.CreateSpecificCulture("en-US"))
            };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }

        /// <summary>
        /// Check Whether Esalation form is submitted by vendor
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> CheckEscalationForm(int Id)
        {
            try
            {

                // Checks CheckEscalationForm for UserId
                if (await _applictionUserRepo.IsExistsAsync<EscalationMatrix>(x => x.UserId == Id))
                    return new ServiceResult<bool>(true, "Id Exists!");

                return new ServiceResult<bool>(true, "Id do not Exists!", true);
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.Date.ToString("dd,MM,yyyy", CultureInfo.CreateSpecificCulture("en-US"))
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }
        /// <summary>
        /// Adding Payment Request Form
        /// </summary>
        /// <param name="model"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> AddPaymentForm(PaymentRequestModel model, int createdBy)
        {
            try
            {
                var contextModel = new PaymentRequest
                {
                    ContractTitle = model.ContractTitle,
                    Department = model.Department,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Classification = model.Classification,
                    ApplicationName = model.ApplicationName,
                    ContractRefType = model.ContractRefType,
                    ProjectName = model.ProjectName,
                    InvoiceNo = model.InvoiceNo,
                    InvoiceDate = model.InvoiceDate,
                    InvoiceAmount = model.InvoiceAmount,
                    Details = model.Details,
                    OrginalInvoice = model.OrginalInvoice,
                    ServiceConfirmation = model.ServiceConfirmation,
                    CopyOfApproval = model.CopyOfApproval,
                    BankName = model.BankName,
                    AccountName = model.AccountName,
                    AccountNo = model.AccountNumber,
                    Iban = model.Iban,
                    SwiftCode = model.SwiftCode,
                    Branch = model.Branch,
                    PaymentMode = model.PaymentMode,
                    Contract=model.Contract,
                    IsDeleted = 0
                };

                var result = await _applictionUserRepo.AddAsync(contextModel, createdBy);
                if (result != null)
                {
                    var UserDetail = await _applictionUserRepo.GetAsync<User>(y => y.Id == createdBy);
                    var options = new RestClientOptions(ApiUrl.RequestAddUrl + $"?PORTALID={PORTALID}")
                    {
                        RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest { Method = Method.Post };
                    request.AddHeader("Authtoken", $"{authToken}");
                    request.AddParameter("input_data", "{\"request\":{\"template\": {\"name\": \"Default Request\"},\"subject\": \"Payment request\",\"group\": {\"name\": \"ITFM\"},\"request_type\": {\"name\": \"Service Request\"}, \"requester\": { \"email_id\":\"AlMatari@anb.com.sa\"},\"priority\": {\"name\": \"High\"}}}");
                    var response = client.ExecuteAsync(request).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        dynamic apiResponse = JsonConvert.DeserializeObject<object>(response.Content);
                        string id = apiResponse["request"]["id"];
                        id.Split('{', '}');
                        var context = await _applictionUserRepo.GetAsync<PaymentRequest>(x => x.Id == result.Id);
                        context.Ticketid = id;
                        await _applictionUserRepo.UpdateAsync(context, createdBy);
                        await EmailHandler.PaymentRequestMail(id, UserDetail.Email, From, SenderPassword, Host, Port);
                        return new ServiceResult<bool>(true, $"Payment Request Submited with Id: {id}!");
                    }
                    else
                    {

                        await EmailHandler.PaymentRequestFailMail(UserDetail.Email, From, SenderPassword, Host, Port);
                        return new ServiceResult<bool>(false, "There is Some Issue With Service Desk Plus", true);
                    }

                }
                return new ServiceResult<bool>(false, "Something went wrong", true);

            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.Date.ToString("dd,MM,yyyy", CultureInfo.CreateSpecificCulture("en-US"))
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }
        /// <summary>
        /// Get Payment Request by Vendor Id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<PaymentRequestModel>>> GetPaymentRequests(int UserId)
        {
            try
            {
               // var cultureInfo = new CultureInfo("en-US");
                //Calendar cl = currentCulture.Calendar;
                var list = (await _applictionUserRepo.GetAllAsync<PaymentRequest>(x => x.CreatedBy == UserId)).OrderByDescending(x=>x.Id);
                IEnumerable<PaymentRequestModel> data = new List<PaymentRequestModel>();
                if (list.Any())
                {
                    var result = list.Select(x => new PaymentRequestModel()
                    {
                        Id = x.Id,
                        ContractTitle = x.ContractTitle,
                        ProjectName = x.ProjectName,
                        Department = x.Department,
                        Classification = x.Classification,
                        Ticketid = x.Ticketid,
                        //Created = Convert.ToDateTime(x.CreatedOn.Split(',',1), CultureInfo.CreateSpecificCulture("en-US")),
                       // Created= DateTime.Parse(x.CreatedOn, cultureInfo)
                         Created = DateTime.ParseExact(x.CreatedOn, "dd,MM,yyyy", CultureInfo.InvariantCulture).Date
                        //Created = DateTime.ParseExact(x.CreatedOn, "dd,MM,yyyy", CultureInfo.CreateSpecificCulture("en-US")).Date
                        //Created = x.CreatedOn


                    });
                    return new ServiceResult<IEnumerable<PaymentRequestModel>>(result, "Payment Request  List!");

                }
                else
                {
                    return new ServiceResult<IEnumerable<PaymentRequestModel>>(data, "No Payment Request  List!");
                }

            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.Date.ToString("dd,MM,yyyy", CultureInfo.CreateSpecificCulture("en-US"))
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<IEnumerable<PaymentRequestModel>>(ex, ex.Message);
            }
        }
        public async Task<ServiceResult<IEnumerable<ContactResponseModel>>> GetContracts(string userdomain)
        {
            try
            {
                List<APIVendor> vendorsFromAPI = await GetVEndorsFromAPI(userdomain);
                List<ContactResponseModel> lst = new();
                var client = new RestClient(ApiUrl.GetAllContractsUrl);
                var contractlist = new List<Contract>();
                var request = new RestRequest("", Method.Get);
                request.AddHeader("authtoken", $"{authToken}");
                request.AddParameter("text/plain", "", ParameterType.RequestBody);
                request.AddParameter("input_data", input_data);
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic apiResponse = JsonConvert.DeserializeObject<object?>(response.Content);
                    ListInfo list_info = JsonConvert.DeserializeObject<ListInfo>(apiResponse["list_info"].ToString());
                    List<Contract> res = JsonConvert.DeserializeObject<List<Contract>>(apiResponse["contracts"].ToString());
                    contractlist.AddRange(res);
                    for (startIndex = 100; list_info.has_more_rows == true; startIndex = startIndex + 101)
                    {
                        var client2 = new RestClient(ApiUrl.GetAllContractsUrl);
                        var request2 = new RestRequest("", Method.Get);
                        request2.AddHeader("authtoken", $"{authToken}");
                        request2.AddParameter("text/plain", "", ParameterType.RequestBody);
                        input_data = $@"{{
                        ""list_info"": {{
                            ""row_count"": 100,
                            ""start_index"": {startIndex},
                            ""sort_field"": ""id"",
                            ""sort_order"": ""asc"",
                            ""get_total_count"": true
                            }}
                        }}";
                        request2.AddParameter("input_data", input_data);
                        var response2 = client2.Execute(request2);
                        dynamic apiResponse2 = JsonConvert.DeserializeObject<object?>(response2.Content);
                        list_info = JsonConvert.DeserializeObject<ListInfo>(apiResponse2["list_info"].ToString());
                        List<Contract> res2 = JsonConvert.DeserializeObject<List<Contract>>(apiResponse2["contracts"].ToString());
                        contractlist.AddRange(res2);
                    }
                    startIndex = 1;
                    input_data = $@"{{
                        ""list_info"": {{
                            ""row_count"": 100,
                            ""start_index"": {startIndex},
                            ""sort_field"": ""id"",
                            ""sort_order"": ""asc"",
                            ""get_total_count"": true
                            }}
                        }}";
                    lst = contractlist.Where(x => vendorsFromAPI.Any(y => y.id == x.vendor.id)).Select(x => new ContactResponseModel { Id = x.id, Name = x.name }).ToList();
                }
                return new ServiceResult<IEnumerable<ContactResponseModel>>(lst, "Contract List");
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.Date.ToString("dd,MM,yyyy", CultureInfo.CreateSpecificCulture("en-US"))
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<IEnumerable<ContactResponseModel>>(ex, ex.Message);
            }
        }

        public async Task<List<APIVendor>> GetVEndorsFromAPI(string userdomain)
        {
            var res3 = new List<APIVendor>();
            try
            {
                var vendorlist = new List<APIVendor>();
                var client = new RestClient(ApiUrl.GetAllVendorsUrl);
                var request = new RestRequest("", Method.Get);
                request.AddHeader("authtoken", $"{authToken}");
                request.AddParameter("text/plain", "", ParameterType.RequestBody);
                request.AddParameter("input_data", input_data);
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic apiResponse = JsonConvert.DeserializeObject<object?>(response.Content);
                    ListInfo list_info = JsonConvert.DeserializeObject<ListInfo>(apiResponse["list_info"].ToString());
                    List<APIVendor> res = JsonConvert.DeserializeObject<List<APIVendor>>(apiResponse["vendors"].ToString());
                    vendorlist.AddRange(res);
                    for (startIndex = 100; list_info.has_more_rows == true; startIndex = startIndex + 100)
                    {
                        var client2 = new RestClient(ApiUrl.GetAllVendorsUrl);
                        var request2 = new RestRequest("", Method.Get);
                        request2.AddHeader("authtoken", $"{authToken}");
                        request2.AddParameter("text/plain", "", ParameterType.RequestBody);
                        input_data = $@"{{
                        ""list_info"": {{
                            ""row_count"": 100,
                            ""start_index"": {startIndex},
                            ""sort_field"": ""id"",
                            ""sort_order"": ""asc"",
                            ""get_total_count"": true
                            }}
                        }}";
                        request2.AddParameter("input_data", input_data);
                        var response2 = client2.Execute(request2);
                        dynamic apiResponse2 = JsonConvert.DeserializeObject<object?>(response2.Content);
                        list_info = JsonConvert.DeserializeObject<ListInfo>(apiResponse2["list_info"].ToString());
                        List<APIVendor> res2 = JsonConvert.DeserializeObject<List<APIVendor>>(apiResponse2["vendors"].ToString());
                        vendorlist.AddRange(res2);
                    }
                    startIndex = 1;
                    input_data = $@"{{
                        ""list_info"": {{
                            ""row_count"": 100,
                            ""start_index"": {startIndex},
                            ""sort_field"": ""id"",
                            ""sort_order"": ""asc"",
                            ""get_total_count"": true
                            }}
                        }}";
                    res3 = vendorlist.Where(x => x.email_id != null && x.email_id.Contains('@')).ToList();
                }

                return res3.Where(x => x.email_id.Split('@')[1] == userdomain).ToList();
            }
            catch (Exception ex)
            {

                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.Date.ToString("dd,MM,yyyy", CultureInfo.CreateSpecificCulture("en-US"))
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return res3;
            }
        }

        public async Task<ServiceResult<Contract>> GetContractById(int id)
        {
            try
            {
                Contract res = new();
                var client = new RestClient(ApiUrl.GetAllContractsUrl + id.ToString());
                var request = new RestRequest("", Method.Get);
                request.AddHeader("authtoken", $"{authToken}");
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic apiResponse = JsonConvert.DeserializeObject<object?>(response.Content);
                    res = JsonConvert.DeserializeObject<Contract>(apiResponse["contract"].ToString());
                    if (res != null)
                    {
                        return new ServiceResult<Contract>(res, "Contract Details");
                    }
                }
                return new ServiceResult<Contract>(res, "No Data");
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.Date.ToString("dd,MM,yyyy", CultureInfo.CreateSpecificCulture("en-US"))
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<Contract>(ex, ex.Message);
            }
        }
    }
}
