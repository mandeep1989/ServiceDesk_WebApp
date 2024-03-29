﻿//UI varaibles
let form_id = "#vendor-form",
    create_url = '/Admin/AddVendor',
    get_url = '/Admin/GetVendorById',
    getAllVendorUrl = '/Admin/GetAllVendors',
    deleteVendorUrl = '/Admin/RemoveVendor',
    update_url = "/Admin/UpdateVendor",
    application_vendor_list = '#application-vendor-list';

//Forms
let txt_id = "#txt-id",
    txt_VendorName = "#txt-VendorName",
    txt_name = "#txt-name",
    txt_email = "#txt-email-id",
    txt_VendorNo = "#txt-VendorNo",
    txt_ResidencyStatus = "#txt-ResidencyStatus",
    txt_Poremarks = "#txt-Poremarks",
    txt_Currency = "#txt-Currency",
    txt_Password = "#txt-Password",


    btn_create_new = "#btn-create-new",
    btn_submit = "#btn-submit";

$(document).ready(function () {

    loadApplicationVendors();
});




$(form_id).unbind().bind('submit', function (e) {
    var actionUrl = $(txt_id).val() == 0 ? create_url : update_url
    e.preventDefault();
    SportaForms.EnableLiveValidation(form_id, validateVendor)
    if (validateVendor()) {
        $.ajax({
            url: actionUrl,
            data: $(this).serialize(),
            type: 'POST',
            success: function (response) {
                if (response.isSuccess) {

                    SportaUtil.MessageBoxSuccess(response.message, "Vendor Created");
                    closeSideBar(form_id);
                    loadApplicationVendors();

                }
                else {
                    SportaUtil.MessageBoxDanger(response.message);
                }
            }
        });
    }
});

function validateVendor() {
    SportaForms.ClearValidataionErrors(form_id);

    var blankChecks = [txt_VendorName, txt_name, txt_email, txt_VendorNo, txt_ResidencyStatus, txt_Poremarks, txt_Currency, txt_Password];
    SportaForms.BlankInputChecks(blankChecks);
    if ($(txt_email).val()) {
        SportaForms.ValidateInput(txt_email,
            !/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                .test($(txt_email).val()),
            "Enter a valid email Id");
    }

    return SportaForms.FormValidationStatus(form_id);
}

function loadApplicationVendors() {
    $.get(getAllVendorUrl, function (response) {
        var tableColumns = [
            { data: "vendorName" },
            { data: "email" },
            { data: "vendorNo" },
            { data: "residencyStatus" },
            { data: null, render: usersActionButtons }
        ]
        SportaDataTable.CreateDataTable(application_vendor_list, response.data, tableColumns)
    });
}
// Buttons Added to Grid
function usersActionButtons(data, type, column) {
    var _buttons = '<div class="btn-group">'
        + `<button class="btn btn-sm btn-outline-grey"   title="Edit" onclick="openUserForm('${data.id}')"><i class="fas fa-pencil-alt"></i></button>`
        + `<button class="btn btn-sm btn-outline-grey"  title="Delete" onclick="deleteVendor('${data.id}','${data.vendorName}')"><i class="far fa-trash-alt"></i></button>`
        + `</div>`;
    return _buttons;
}
function deleteVendor(id, vendorName) {
    SportaUtil.ConfirmDialogue(`Are you sure  to delete ${vendorName}?`, null, function () {

        $.get(deleteVendorUrl, { 'Id': id }, function (response) {
            if (response.isSuccess)
                SportaUtil.MessageBoxSuccess(response.message);
            else
                SportaUtil.MessageBoxDanger(response.message);

            loadApplicationVendors();
        }).fail(function (xhr, error, errorMessage) {
            debugger
            SportaUtil.MessageBoxDanger(xhr.responseText.split('Path:')[0]);
        });
    })
}

function closeVendorForm() {
    if (form_id)
        SportaForms.ResetForm(form_id)

    $('.sidebar').slideUp();
}
