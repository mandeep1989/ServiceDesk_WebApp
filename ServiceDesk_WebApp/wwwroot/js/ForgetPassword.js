let form_id = "#forgetPassWordForm",
    password_url = '/Admin/changePasswordRequest',
    txt_email = "#Email";


$(document).ready(function () {
    $('#preloader').hide();
});
// validating login user form blank checks
function validateForgetPassword() {
    SportaForms.ClearValidataionErrors(form_id);
    var blankChecks = [txt_email];
    SportaForms.BlankInputChecks(blankChecks);
    if ($(txt_email).val()) {
        SportaForms.ValidateInput(txt_email,
            !/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                .test($(txt_email).val()),
            "Enter a valid email Id");
    }

    return SportaForms.FormValidationStatus(form_id);
}


// Login Submit
$(form_id).unbind().bind('submit', function (e) {
    SportaForms.InitializeFormStyle(form_id);
    e.preventDefault();
    SportaForms.EnableLiveValidation(form_id, validateForgetPassword)
    var data = $(this).serialize();
    if (validateForgetPassword(form_id)) {
        ForgetPassword(data);
    }
})

// Login Function
function ForgetPassword(data) {
    $.ajax({
        url: '/Admin/ChangePasswordRequest',
        data: data,
        type: 'POST',
        beforeSend: function () { $('#preloader').show(); },
        success: function (response) {
            if (response.isSuccess) {
                SportaUtil.MessageBoxSuccess(response.message);
            }
            else {
                SportaUtil.MessageBoxDanger(response.message);
            }
        },
        error: function (xhr, error, errorMessage) {
            SportaUtil.MessageBoxDanger(errorMessage);
        },
        complete: function () { $('#preloader').hide(); }
    });
}
