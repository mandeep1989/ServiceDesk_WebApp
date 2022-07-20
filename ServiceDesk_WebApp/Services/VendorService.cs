using ServiceDesk_WebApp.Common;
using ServiceDesk_WebApp.ViewModel;

namespace ServiceDesk_WebApp.Services
{
    public class VendorService : IVendorService
    {
        private readonly IRepository _applictionUserRepo;
        public VendorService(IRepository applictionUserRepo)
        {
            _applictionUserRepo = applictionUserRepo;
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
                    UserId=createdBy,
                    IsDeleted = 0
                };

                // Checks email for duplicate
                if (await _applictionUserRepo.IsExistsAsync<EscalationMatrix>(au => au.CompanyName.Equals(escalationForm.CompanyName) || au.ContactEmail.Equals(escalationForm.ContactEmail)))
                    return new ServiceResult<bool>(true, "Email Already Exist!", true);

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

    }
}
