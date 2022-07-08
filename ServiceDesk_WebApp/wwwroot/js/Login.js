

let login_form_id = '#loginForm',
    txt_login_emailID = '#Email',
    txt_login_password = '#Password'

// validating login user form blank checks
function validateLoginUser() {
    SportaForms.ClearValidataionErrors(login_form_id);
    var blankChecks = [txt_login_emailID, txt_login_password];
    SportaForms.BlankInputChecks(blankChecks);
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
        success: function (response) {
            if (response.isSuccess) {
                return location.replace(response.url);
            }
            else {
                SportaUtil.MessageBox(response.message, { type: "Error" });
            }
        },
        error: function (xhr, error, errorMessage) {
            SportaUtil.MessageBoxDanger(errorMessage);
        },
    });
}


