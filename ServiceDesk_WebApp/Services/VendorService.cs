using Newtonsoft.Json;
using RestSharp;
using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.ViewModel;

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
        public VendorService(IRepository applictionUserRepo, IConfiguration configuration)
        {
            _applictionUserRepo = applictionUserRepo;
            authToken = configuration.GetValue<string>("Authtoken");
            From = configuration.GetValue<string>("EmailSettings:From");
            SenderPassword = configuration.GetValue<string>("EmailSettings:Password");
            Host = configuration.GetValue<string>("EmailSettings:Host");
            Port = configuration.GetValue<int>("EmailSettings:Port");

        }

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
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }


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
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }

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
                    var client = new RestClient(ApiUrl.RequestAddUrl);
                    var request = new RestRequest { Method = Method.Post };
                    request.AddHeader("Authtoken", $"{authToken}");
                    request.AddParameter("input_data", "{\"request\":{\"template\": {\"name\": \"Default Request\"},\"subject\": \"Payment request\",\"group\": {\"name\": \"network\"},\"request_type\": {\"name\": \"Service Request\"}, \"requester\": {\"name\": \"" + UserDetail.Name + " \" , \"email_id\":\"" + UserDetail.Email + "\"},\"priority\": {\"name\": \"High\"}}}");
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
                        return new ServiceResult<bool>(false, "There is Some Issue With Service Desk Plus", true);
                    }

                }
                return new ServiceResult<bool>(false, "Something went wrong", true);

            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }

        public async Task<ServiceResult<IEnumerable<PaymentRequestModel>>> GetPaymentRequests(int UserId)
        {
            try
            {
                var list = (await _applictionUserRepo.GetAllAsync<PaymentRequest>(x => x.CreatedBy == UserId)).OrderByDescending(x=>x.Id);
                IEnumerable<PaymentRequestModel> data = new List<PaymentRequestModel>();
                if (list.Any())
                {
                   var  result = list.Select(x => new PaymentRequestModel()
                    {
                        Id = x.Id,
                        ContractTitle = x.ContractTitle,
                        ProjectName = x.ProjectName,
                        Department = x.Department,
                        Classification = x.Classification,
                       Created = DateTime.ParseExact(x.CreatedOn, "dd,MM,yyyy", null).Date

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
                return new ServiceResult<IEnumerable<PaymentRequestModel>>(ex, ex.Message);
            }



        }
    }
}
