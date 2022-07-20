//UI varaibles
let form_id = "#escalationRequest_form",
    request_url = '/Vendor/AddEscalationForm',
    check_escalation_url = '/Vendor/CheckEscalationForm';
//Forms
let txt_CompanyName = "#CompanyName",
    txt_CompanyEmail = "#CompanyEmail",
    txt_CompanyPhone = "#CompanyPhone",
    txt_ContactName = "#ContactName",
    txt_ContactEmail = "#ContactEmail",
    txt_ContactPhone = "#ContactPhone";

$(document).ready(function () {

    checkEscalationStatus();
});

function validateEscalationForm() {
    SportaForms.ClearValidataionErrors(form_id);

    var blankChecks = [txt_CompanyName, txt_CompanyEmail, txt_CompanyPhone, txt_ContactName, txt_ContactEmail, txt_ContactPhone];
    SportaForms.BlankInputChecks(blankChecks);
    if ($(txt_ContactEmail).val()) {
        SportaForms.ValidateInput(txt_ContactEmail,
            !/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                .test($(txt_ContactEmail).val()),
            "Enter a valid email Id");
    }
    if ($(txt_CompanyEmail).val()) {
        SportaForms.ValidateInput(txt_CompanyEmail,
            !/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                .test($(txt_CompanyEmail).val()),
            "Enter a valid email Id");
    }

    return SportaForms.FormValidationStatus(form_id);
}

$(form_id).unbind().bind('submit', function (e) {
    e.preventDefault();
    debugger
    SportaForms.EnableLiveValidation(form_id, validateEscalationForm)
    if (validateEscalationForm()) {
        $.ajax({
            url: request_url,
            data: $(this).serialize(),
            type: 'POST',
            success: function (response) {
                if (response.isSuccess) {

                    SportaUtil.MessageBoxSuccess(response.message);
                    $("#escalation_form").modal("hide");
                    $(form_id)[0].reset();
                    $("#escalationBtn").prop('disabled', true);
                    $("#paymentBtn").prop('disabled', false);
                    
                }
                else {
                    SportaUtil.MessageBoxDanger(response.message);
                }
            }
        });
    }
});
function checkEscalationStatus() {
    $.ajax({
        url: check_escalation_url,
        type: 'GET',
        success: function (response) {
            debugger
            if (response.isSuccess) {

                $("#escalationBtn").prop('disabled', true);
                $("#paymentBtn").prop('disabled', false);

            }
            else {

                $("#escalationBtn").prop('disabled', false);
                $("#paymentBtn").prop('disabled', true);

            }
        }
    });

}

function closeEscalationForm() {
    SportaForms.ResetForm(form_id);
    $("#escalation_form").modal("hide");
}


function openEscalationForm() {
    SportaForms.ResetForm(form_id);
    SportaForms.InitializeFormStyle(form_id);   
    $("#escalation_form").modal("show");
}