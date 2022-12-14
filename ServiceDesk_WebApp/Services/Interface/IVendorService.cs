using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.ViewModel;

namespace ServiceDesk_WebApp.Services.Interface
{
    public interface IVendorService
    {
        /// <summary>
        /// AddEscalationForm
        /// </summary>
        /// <param name="escalationForm"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> AddEscalationForm(EscalationFormRequest escalationForm, int createdBy);
        /// <summary>
        /// CheckEscalationForm
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> CheckEscalationForm(int Id);
        /// <summary>
        /// AddPaymentForm
        /// </summary>
        /// <param name="model"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> AddPaymentForm(PaymentRequestModel model, int createdBy);
        /// <summary>
        /// GetPaymentRequests
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<PaymentRequestModel>>> GetPaymentRequests(int UserId);

        Task<ServiceResult<IEnumerable<ContactResponseModel>>> GetContracts(string userdomain);
        Task<ServiceResult<Contract>> GetContractById(int id);
    }
}
