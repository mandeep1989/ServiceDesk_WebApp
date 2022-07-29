﻿using Newtonsoft.Json;
using RestSharp;
using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.Models;
using ServiceDesk_WebApp.RepositoryLayer;
using ServiceDesk_WebApp.Services.Interface;
using ServiceDesk_WebApp.ViewModel;
using System.Globalization;
using System.Net;

namespace ServiceDesk_WebApp.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IRepository _applictionUserRepo;
        private readonly string From;
        private readonly string UserName;
        private readonly string SenderPassword;
        private readonly string Host;
        private readonly int Port;
        private readonly int PORTALID;
        private readonly bool SSlEnable;
        private readonly string authToken;
        private readonly ServiceDesk_WebAppContext _context;
        public ApplicationUserService(IRepository applictionUserRepo, IConfiguration configuration, ServiceDesk_WebAppContext context)
        {
            _applictionUserRepo = applictionUserRepo;
            From = configuration.GetValue<string>("EmailSettings:From");
            UserName = configuration.GetValue<string>("EmailSettings:UserName");
            SenderPassword = configuration.GetValue<string>("EmailSettings:Password");
            Host = configuration.GetValue<string>("EmailSettings:Host");
            Port = configuration.GetValue<int>("EmailSettings:Port");
            SSlEnable = configuration.GetValue<bool>("EmailSettings:SSlEnable");
            authToken = configuration.GetValue<string>("Authtoken");
            PORTALID = configuration.GetValue<int>("PORTALID");
            _context = context;
        }
        /// <summary>
        /// LogInAsync
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public async Task<ServiceResult<LoginResponse>> LogInAsync(LoginRequest loginRequest)
        {
            try
            {
                var applicationUsersList = await _applictionUserRepo.GetAllAsync<User>(x => x.Email.ToLower().Equals(loginRequest.Email.ToLower()));


                if (applicationUsersList.Any())
                {
                    var applicationUser = applicationUsersList.FirstOrDefault(x => x.Password.Equals(loginRequest.Password));
                    if (applicationUser != null)
                    {
                        if (applicationUser.IsDeleted == 1)
                            return new ServiceResult<LoginResponse>(null, "Your Account is blocked, please contact admin.", true);

                        LoginResponse loginResponse = new();

                        loginResponse.UserRole = applicationUser.UserRole;
                        loginResponse.Id = applicationUser.Id;
                        loginResponse.Name = applicationUser.Name;
                        loginResponse.Email = applicationUser.Email;

                        return new ServiceResult<LoginResponse>(loginResponse, "User Details");

                    }
                    else
                        return new ServiceResult<LoginResponse>(null, "Invalid Credentials!");
                }
                else
                    return new ServiceResult<LoginResponse>(null, "User not found, please contact admin.");
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.ToString()
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<LoginResponse>(ex, ex.Message);
            }
        }
        /// <summary>
        /// AddVendor
        /// </summary>
        /// <param name="vendorViewModel"></param>
        /// <param name="createdBy"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task<ServiceResult<User>> AddVendor(VendorViewModel vendorViewModel, int createdBy, string link)
        {
            try
            {
                var contextModel = new User
                {
                    Name = vendorViewModel.VendorName,
                    Email = vendorViewModel.Email,
                    Password = vendorViewModel.Password,
                    UserRole = (int)UserRole.Vendor,
                    IsDeleted = 0
                };

                // Checks email for duplicate
                if (await _applictionUserRepo.IsExistsAsync<User>(au => au.Email.Equals(vendorViewModel.Email)))
                    return new ServiceResult<User>(contextModel, "Email Already Exist!", true);

                await _applictionUserRepo.AddAsync(contextModel, createdBy);
                var vendorModel = new Vendor
                {
                    UserId = contextModel.Id,
                    VendorName = vendorViewModel.VendorName,
                    VendorNo = vendorViewModel.VendorNo,
                    ResidencyStatus = vendorViewModel.ResidencyStatus,
                    Poremarks = vendorViewModel.PORemarks,
                    Currency = vendorViewModel.Currency,
                };
                await _applictionUserRepo.AddAsync(vendorModel, createdBy);
                await EmailHandler.SendUserDetails(vendorViewModel.Password, vendorViewModel.Email, link, From, SenderPassword, Host, Port, SSlEnable);

                return new ServiceResult<User>(contextModel, "Vendor added!");
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.Date.ToString("dd,MM,yyyy")
                };
                LogService log = new LogService();
                log.AddLogError(ex.Message + " " + ex.Message);

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<User>(ex, ex.Message);
            }
        }
        /// <summary>
        /// GetAllVendors
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<VendorViewModel>>> GetAllVendors()
        {
            try
            {
                var list = await _applictionUserRepo.GetAllAsync<User>(x => x.UserRole == (int)UserRole.Vendor);
                var applicationUsers = new List<VendorViewModel>();
                foreach (var user in list)
                {
                    var UserDetail = await _applictionUserRepo.GetAsync<Vendor>(y => y.UserId == user.Id);
                    applicationUsers.Add(new VendorViewModel
                    {
                        Id = user.Id,
                        VendorName = user.Name,
                        Email = user.Email,
                        VendorNo = UserDetail.VendorNo,
                        ResidencyStatus = UserDetail.ResidencyStatus
                    });
                }

                return new ServiceResult<IEnumerable<VendorViewModel>>(applicationUsers, "Vendor List!");
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.ToString()
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<IEnumerable<VendorViewModel>>(ex, ex.Message);
            }
        }
        /// <summary>
        /// GetVendorById
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<VendorViewModel>> GetVendorById(int Id)
        {
            try
            {
                var User = await _applictionUserRepo.GetAsync<User>(x => x.Id == Id);
                var applicationVendors = new VendorViewModel();

                if (User != null)
                {
                    applicationVendors.Id = User.Id;
                    applicationVendors.Email = User.Email;
                    applicationVendors.VendorName = User.Name;
                    applicationVendors.Password = User.Password;
                    var UserDetail = await _applictionUserRepo.GetAsync<Vendor>(x => x.UserId == Id);
                    applicationVendors.VendorNo = UserDetail.VendorNo;
                    applicationVendors.ResidencyStatus = UserDetail.ResidencyStatus;
                    applicationVendors.PORemarks = UserDetail.Poremarks;
                    applicationVendors.Currency = UserDetail.Currency;
                    return new ServiceResult<VendorViewModel>(applicationVendors, "Vendor List!");
                }
                else
                {
                    return new ServiceResult<VendorViewModel>(null, "No Vendor Found!");
                }
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.ToString()
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<VendorViewModel>(ex, ex.Message);
            }
        }
        /// <summary>
        /// UpdateVendor
        /// </summary>
        /// <param name="vendorViewModel"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> UpdateVendor(VendorViewModel vendorViewModel, int modifiedBy)
        {
            try
            {
                var User = await _applictionUserRepo.GetAsync<User>(x => x.Id == vendorViewModel.Id);
                if (User != null)
                {

                    User.Name = vendorViewModel.VendorName;
                    await _applictionUserRepo.UpdateAsync(User, modifiedBy);
                    var UserDetail = await _applictionUserRepo.GetAsync<Vendor>(x => x.UserId == vendorViewModel.Id);
                    UserDetail.VendorNo = vendorViewModel.VendorNo;
                    UserDetail.ResidencyStatus = vendorViewModel.ResidencyStatus;
                    UserDetail.Poremarks = vendorViewModel.PORemarks;
                    UserDetail.Currency = vendorViewModel.Currency;

                    return new ServiceResult<bool>(await _applictionUserRepo.UpdateAsync(UserDetail, modifiedBy) != null, "Vendor updated!");
                }

                return new ServiceResult<bool>(false, "No record found!", true);
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.ToString()
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<bool>(ex, ex.Message);
            }

        }
        /// <summary>
        /// RemoveVendor
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> RemoveVendor(int Id, int modifiedBy)
        {
            try
            {
                var User = await _applictionUserRepo.GetAsync<User>(x => x.Id == Id);
                await _applictionUserRepo.RemoveAsync(User, modifiedBy);
                var UserDetail = await _applictionUserRepo.GetAsync<Vendor>(x => x.UserId == Id);
                await _applictionUserRepo.RemoveAsync(UserDetail, modifiedBy);

                return new ServiceResult<bool>(true, "Vendor deleted Successfully!");
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.ToString()
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<bool>(ex, ex.Message);
            }

        }
        /// <summary>
        /// GetVendorsCountByDate
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<GetVendorCount>> GetVendorsCountByDate()
        {
            try
            {
                var list = await _applictionUserRepo.GetAllAsync<User>(x => x.UserRole == (int)UserRole.Vendor);
                GetVendorCount getVendorCount = new GetVendorCount();
                getVendorCount.TodayCount = list.Where(x => DateTime.ParseExact(x.CreatedOn, "dd,MM,yyyy", null).Date == DateTime.Now.Date).Count();
                getVendorCount.YesterDayCount = list.Where(x => (DateTime.Now.Date - DateTime.ParseExact(x.CreatedOn, "dd,MM,yyyy", null).Date).TotalDays == 1).Count();
                getVendorCount.Last7DaysCount = list.Where(x => (DateTime.Now.Date - DateTime.ParseExact(x.CreatedOn, "dd,MM,yyyy", null).Date).TotalDays <= 7).Count();
                getVendorCount.Last30DaysCount = list.Where(x => (DateTime.Now.Date - DateTime.ParseExact(x.CreatedOn, "dd,MM,yyyy", null).Date).TotalDays <= 30).Count();
                getVendorCount.Last90DaysCount = list.Where(x => (DateTime.Now.Date - DateTime.ParseExact(x.CreatedOn, "dd,MM,yyyy", null).Date).TotalDays <= 90).Count();

                return new ServiceResult<GetVendorCount>(getVendorCount, "Vendor count!");
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.ToString()
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<GetVendorCount>(ex, ex.Message);
            }
        }
        /// <summary>
        /// ChangePasswordRequest
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> ChangePasswordRequest(string Email)
        {
            try
            {
                var checkEmail = await _applictionUserRepo.GetAsync<ChangePasswordRequest>(x => x.Status == 0 && x.Email == Email);
                if (checkEmail == null)
                {
                    var User = await _applictionUserRepo.GetAsync<User>(x => x.Email == Email && x.UserRole == 2);

                    if (User != null)
                    {
                        //  ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                        var options = new RestClientOptions(ApiUrl.RequestAddUrl + $"?PORTALID={PORTALID}")
                        {
                            RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
                        };
                        var client = new RestClient(options);
                        //  var client = new RestClient(ApiUrl.RequestAddUrl+$"?PORTALID=4202");
                        var request = new RestRequest { Method = Method.Post };
                        request.AddHeader("Authtoken", $"{authToken}");
                        request.AlwaysMultipartFormData = true;
                        request.AddParameter("input_data", "{\"request\":{\"template\": {\"name\": \"Default Request\"},\"subject\": \"Change Password request\",\"group\": {\"name\": \"ITFM\"},\"request_type\": {\"name\": \"Service Request\"}, \"requester\": {\"email_id\":\"AAlMatari@anb.com.sa\"},\"priority\": {\"name\": \"High\"}}}");
                        var response = client.ExecuteAsync(request).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            dynamic apiResponse = JsonConvert.DeserializeObject<object>(response.Content);
                            string id = apiResponse["request"]["id"];
                            id.Split('{', '}');
                            var count = await _applictionUserRepo.CountAsync<ChangePasswordRequest>(true);
                            var contextModel = new ChangePasswordRequest
                            {
                                Id = GenerateId("PW-", count),
                                UserId = User.Id,
                                Email = User.Email,
                                Status = 0,
                                IsDeleted = 0,
                                ApiTicketId = id
                            };
                            await _applictionUserRepo.AddAsync(contextModel, Convert.ToInt32(User.Id));
                            await EmailHandler.PasswordRequestMail(contextModel.Id, contextModel.ApiTicketId, Email, From, SenderPassword, Host, Port);
                            return new ServiceResult<bool>(true, "Password Rest Request Sent!", false);
                        }
                        else
                        {
                            return new ServiceResult<bool>(false, "There is Some Issue With Service Desk Plus", true);
                        }

                    }
                    else
                    {
                        return new ServiceResult<bool>(false, "No Vendor With this Email Found!", true);
                    }
                }
                else
                {
                    return new ServiceResult<bool>(false, "Reset Request Already sent!", true);
                }

            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.ToString()
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<bool>(ex, ex.Message);
            }

        }
        /// <summary>
        /// GetPasswordRequests
        /// </summary>
        /// <returns></returns>

        public async Task<ServiceResult<IEnumerable<PasswordResetRequest>>> GetPasswordRequests()
        {
            try
            {
                var list = (await _applictionUserRepo.GetAllAsync<ChangePasswordRequest>()).OrderBy(x => x.Status);
                var passwordRequest = new List<PasswordResetRequest>();
                foreach (var request in list)
                {
                    var UserDetail = await _applictionUserRepo.GetAsync<User>(y => y.Id == request.UserId);

                    if (UserDetail != null)
                    {
                        passwordRequest.Add(new PasswordResetRequest
                        {
                            TicketId = request.Id,
                            Name = UserDetail.Name,
                            Email = UserDetail.Email,
                            UserId = UserDetail.Id,
                            Status = request.Status,
                            ApiTicketId = request.ApiTicketId

                        });
                    }
                }

                return new ServiceResult<IEnumerable<PasswordResetRequest>>(passwordRequest, "Request List!");
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.ToString()
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<IEnumerable<PasswordResetRequest>>(ex, ex.Message);
            }
        }
        /// <summary>
        /// UpdatePassword
        /// </summary>
        /// <param name="changePasswordRequestModel"></param>
        /// <param name="modifiedBy"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> UpdatePassword(ChangePasswordRequestModel changePasswordRequestModel, int modifiedBy, string link)
        {
            try
            {
                var User = await _applictionUserRepo.GetAsync<User>(x => x.Id == changePasswordRequestModel.UserId);
                if (User != null)
                {

                    User.Password = changePasswordRequestModel.Password;
                    await _applictionUserRepo.UpdateAsync(User, modifiedBy);
                    var details = await _applictionUserRepo.GetAsync<ChangePasswordRequest>(x => x.Id == changePasswordRequestModel.TicketId);
                    details.Status = 1;
                    await _applictionUserRepo.UpdateAsync(details, modifiedBy);
                    await EmailHandler.PasswordResolveMail(changePasswordRequestModel.Password, changePasswordRequestModel.TicketId, changePasswordRequestModel.ApiTicketId, details.Email, From, SenderPassword, Host, Port, link);
                    return new ServiceResult<bool>(null, "Password updated!");
                }

                return new ServiceResult<bool>(false, "Email Disabled!", true);
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.ToString()
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<bool>(ex, ex.Message);
            }

        }
        /// <summary>
        /// RemoveRequest
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> RemoveRequest(string Id, int modifiedBy)
        {
            try
            {
                var Request = await _applictionUserRepo.GetAsync<ChangePasswordRequest>(x => x.Id == Id);
                Request.Status = 1;
                await _applictionUserRepo.UpdateAsync(Request, modifiedBy);
                await _applictionUserRepo.RemoveAsync(Request, modifiedBy);
                return new ServiceResult<bool>(true, "Ticket deleted Successfully!");
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.ToString()
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<bool>(ex, ex.Message);
            }

        }
        /// <summary>
        /// CreateErrorLog
        /// </summary>
        /// <param name="errorRequest"></param>
        /// <returns></returns>
        public async Task<ServiceResult<string>> CreateErrorLog(ErrorRequest errorRequest)
        {
            try
            {
                LogError errorLog = new()
                {
                    Information = errorRequest.Information,
                    UserId = errorRequest.UserId,
                    Time = DateTime.Now.ToString()
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();

                return new ServiceResult<string>(errorLog.Information, "Exception Logged");
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(ex, ex.Message);
            }
        }
        /// <summary>
        /// GetPaymentRequests
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<PaymentRequestModel>>> GetPaymentRequests()
        {
            try
            {
                var list = await _applictionUserRepo.GetAllAsync<PaymentRequest>(x => true);
                var paymentRequestModel = new List<PaymentRequestModel>();
                foreach (var user in list)
                {
                    var UserDetail = await _applictionUserRepo.GetAsync<User>(y => y.Id == user.CreatedBy);
                    paymentRequestModel.Add(new PaymentRequestModel
                    {
                        Id = user.Id,
                        Name = UserDetail.Name,
                        Email = UserDetail.Email,
                        ContractTitle = user.ContractTitle,
                        ProjectName = user.ProjectName,
                        Department = user.Department,
                        Classification = user.Classification,
                        Created = DateTime.ParseExact(user.CreatedOn, "dd,MM,yyyy", null).Date
                    });
                }

                return new ServiceResult<IEnumerable<PaymentRequestModel>>(paymentRequestModel, "Payment Request  List!");
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.Message + " " + ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.ToString()
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<IEnumerable<PaymentRequestModel>>(ex, ex.Message);
            }
        }

        public async Task<ServiceResult<GetVendorCount>> GetRequestCountByDate()
        {
            try
            {
                var list = await _applictionUserRepo.GetAllAsync<PaymentRequest>();
                GetVendorCount getRequestCount = new GetVendorCount();
                getRequestCount.TodayCount = list.Where(x => DateTime.ParseExact(x.CreatedOn, "dd,MM,yyyy", null).Date == DateTime.Now.Date).Count();
                getRequestCount.YesterDayCount = list.Where(x => (DateTime.Now.Date - DateTime.ParseExact(x.CreatedOn, "dd,MM,yyyy", null).Date).TotalDays == 1).Count();
                getRequestCount.Last7DaysCount = list.Where(x => (DateTime.Now.Date - DateTime.ParseExact(x.CreatedOn, "dd,MM,yyyy", null).Date).TotalDays <= 7).Count();
                getRequestCount.Last30DaysCount = list.Where(x => (DateTime.Now.Date - DateTime.ParseExact(x.CreatedOn, "dd,MM,yyyy", null).Date).TotalDays <= 30).Count();
                getRequestCount.Last90DaysCount = list.Where(x => (DateTime.Now.Date - DateTime.ParseExact(x.CreatedOn, "dd,MM,yyyy", null).Date).TotalDays <= 90).Count();

                return new ServiceResult<GetVendorCount>(getRequestCount, "Request count!");
            }
            catch (Exception ex)
            {
                LogError errorLog = new()
                {
                    Information = ex.StackTrace,
                    UserId = 1,
                    Time = DateTime.Now.ToString()
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();
                return new ServiceResult<GetVendorCount>(ex, ex.Message);
            }
        }

        /// <summary>
        /// GenerateId
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="count"></param>
        /// <returns></returns>

        private string GenerateId(string prefix, long count)
        {
            return $"{prefix}{count + 1:D3}"; //For count 0 it will return the code as SP00001
        }

    }
}
