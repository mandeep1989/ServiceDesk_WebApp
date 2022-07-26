﻿using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.ViewModel;

namespace ServiceDesk_WebApp.Services.Interface
{
    public interface IVendorService
    {
        Task<ServiceResult<bool>> AddEscalationForm(EscalationFormRequest escalationForm, int createdBy);
        Task<ServiceResult<bool>> CheckEscalationForm(int Id);
        Task<ServiceResult<bool>> AddPaymentForm(PaymentRequestModel model, int createdBy);
        Task<ServiceResult<IEnumerable<PaymentRequestModel>>> GetPaymentRequests(int UserId);
    }
}
