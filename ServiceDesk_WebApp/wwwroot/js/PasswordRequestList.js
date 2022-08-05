let getAllRequestUrl = '/Admin/PasswordRequestList',
    deleteRequestUrl='/Admin/RemoveRequest' ,
    forget_pw_list = "#forget-pw-list",
    txt_email = "#email",
    txt_ticketId = "#ticketId",
    userId = "#userId",
    txt_apiTicketId = "#apiTicketId",
    password = "#password",
    Password_change_form = "#Password_change_form",
    Password_change_model = "#Password_change_model";
    


$(document).ready(function () {
    loadPasswordRequests();

});



function loadPasswordRequests() {
    $.get(getAllRequestUrl, function (response) {
        var tableColumns = [
            { data: "apiTicketId" },
            { data: "name" },
            { data: "email" },
           
            {
                data: null, render: function (data) {
                    var bsClass;
                    var value;
                    if (data.status == 0) {
                        bsClass = "text-warning"
                        value = "Initaited"
                    }
                    else {
                        bsClass = "text-success"
                        value = "Resolved"
                    }
                    return `<strong><span class='${bsClass}'>${value}</span></strong>`
                }
            },
            { data: null, render: usersActionButtons }

        ]
        SportaDataTable.CreateDataTable(forget_pw_list, response.data, tableColumns)
    });
}
// Buttons Added to Grid
function usersActionButtons(data, type, column) {
    var _buttons = '<div class="btn-group">'
        + `<button class="btn btn-sm btn-outline-grey"  ${data.status == '1' ? 'disabled' : ''} title="Edit" onclick="openPasswordForm('${data.userId}','${data.status}','${data.email}','${data.ticketId}','${data.apiTicketId}')"><i class="fa-solid fa-key"></i></button>`
        + `<button class="btn btn-sm btn-outline-grey"  title="Delete" onclick="deleteRequest('${data.ticketId}','${data.apiTicketId}')"><i class="far fa-trash-alt"></i></button>`
        + `</div>`;
    return _buttons;
}

function openPasswordForm(Id, status, email, ticketId, apiTicketId) {
    if (status == 0) {
        SportaForms.ResetForm(Password_change_form);
        $(userId).val(Id);
        $(txt_email).val(email);
        $(txt_ticketId).val(ticketId);
        $(txt_apiTicketId).val(apiTicketId);
        SportaForms.InitializeFormStyle(Password_change_form);
        $(Password_change_model).modal("show");

    } else {

        SportaUtil.MessageBoxDanger("Already Resolved");
    }
}
$(Password_change_form).unbind().bind('submit', function (e) {
    var actionUrl = '/Admin/UpdatePassword';
    e.preventDefault();
    SportaForms.EnableLiveValidation(Password_change_form, validatePassword)
    if (validatePassword()) {
        $.ajax({
            url: actionUrl,
            data: $(this).serialize(),
            type: 'POST',
            success: function (response) {
                if (response.isSuccess) {
                    $(Password_change_model).modal("hide");
                    SportaUtil.MessageBoxSuccess(response.message);
                    loadPasswordRequests();
                }
                else {
                    SportaUtil.MessageBoxDanger(response.message);
                }
            }
        });
    }
});

function validatePassword() {
    SportaForms.ClearValidataionErrors(Password_change_form);

    var blankChecks = [password];
    SportaForms.BlankInputChecks(blankChecks);
    return SportaForms.FormValidationStatus(Password_change_form);
}

function deleteRequest(ticketId, apiTicketId) {
    SportaUtil.ConfirmDialogue(`Are you sure  to delete Ticket : ${apiTicketId}?`, null, function () {

        $.get(deleteRequestUrl, { 'Id': ticketId }, function (response) {
            if (response.isSuccess)
                SportaUtil.MessageBoxSuccess(response.message);
            else
                SportaUtil.MessageBoxDanger(response.message);

            loadPasswordRequests();
        })
    })
}

