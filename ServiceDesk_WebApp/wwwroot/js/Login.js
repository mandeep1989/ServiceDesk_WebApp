

let login_form_id = '#loginForm',
    txt_login_emailID = '#Email',
    txt_login_password = '#Password'

$(document).ready(function () {
    $('#preloader').hide();
});
// validating login user form blank checks
function validateLoginUser() {
    SportaForms.ClearValidataionErrors(login_form_id);
    var blankChecks = [txt_login_emailID, txt_login_password];
    SportaForms.BlankInputChecks(blankChecks);
    debugger
    if ($(txt_login_emailID).val()) {
        SportaForms.ValidateInput(txt_login_emailID,
            !/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                .test($(txt_login_emailID).val()),
            "Enter a valid email Id");
    }

    return SportaForms.FormValidationStatus(login_form_id);
}


// Login Submit
$(login_form_id).unbind().bind('submit', function (e) {
    SportaForms.InitializeFormStyle(login_form_id);
    e.preventDefault();
    SportaForms.EnableLiveValidation(login_form_id, validateLoginUser)
    var data = $(this).serialize();
    if (validateLoginUser(login_form_id)) {
        Login(data);
    }
})

// Login Function
function Login(data) {
    $.ajax({
        url: '/Account/Login',
        data: data,
        type: 'POST',
        beforeSend: function () { $('#preloader').show(); },
        success: function (response) {
            debugger
            if (response.isSuccess) {
                return location.replace(response.url);
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


