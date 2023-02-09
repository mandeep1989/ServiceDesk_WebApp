//UI varaibles
let form_id = "#contract-form",
    create_url = '/Admin/AddVendor',
    get_url = '/Admin/GetVendorById',
    getContract_url = '/Admin/GetContractById',
    getAllContractsUrl = '/Admin/GetAllContracts',
    getAllVendorUrl = '/Admin/GetAllVendors',
    deleteContractUrl = '/Admin/RemoveContract',
    update_url = "/Admin/UpdateContract",
    AddContract = "/Admin/AddContract",
    GetDepartments_url = "/Admin/GetDepartments",
    aapplication_contract_list = '#application-contract-list';

//Forms


let departmentNametxt = "#departmentNametxt",
   



btn_create_new = "#btn-create-new",
    btn_submit = "#btn-submit";

$(document).ready(function () {

    $.get(GetDepartments_url, function (data) {
        renderDepartmentsDropdown(data);
    });
    $.get(getAllVendorUrl, function (data) {
        renderVendorDropdown(data);
    });

    loadApplicationDepartments();
});
var renderDepartmentsDropdown = function (data) {
    /* let email = userEmail.split("@")[1];*/
    var dropdown = $('#txt-department');
    for (var i = 0; i < data.data.length; i++) {
        var entry = $('<option>', { value: data.data[i].id, text: data.data[i].departmentName })
        dropdown.append(entry);
    }
};

var renderVendorDropdown = function (data) {
    /* let email = userEmail.split("@")[1];*/
    var dropdown = $('#txt-vendor');
    for (var i = 0; i < data.data.length; i++) {
        var entry = $('<option>', { value: data.data[i].id, text: data.data[i].email })
        dropdown.append(entry);
    }
};



$(form_id).unbind().bind('submit', function (e) {
    var actionUrl = $(txt_id).val() == 0 ? AddContract : update_url
    e.preventDefault();
    SportaForms.EnableLiveValidation(form_id, validateVendor)
    if (validateVendor()) {
        var k = $(this).serialize();
        debugger;

        var txtBudgetattachmentUpload = $(txtBudgetattachment).get(0);
        var txtBudgetattachmentfiles = txtBudgetattachmentUpload.files;

        var txtContractattachmentUpload = $(txtContractattachment).get(0);
        var txtContractattachmentfiles = txtContractattachmentUpload.files;


        var txtOtherAttachmentsUpload = $(txtOtherAttachments).get(0);
        var txtOtherAttachmentsfiles = txtOtherAttachmentsUpload.files;


        var fileData = new FormData();
        for (var i = 0; i < txtBudgetattachmentfiles.length; i++) {
            fileData.append('Budgetattachment', txtBudgetattachmentfiles[i], txtBudgetattachmentfiles[i].name);
        }
        for (var i = 0; i < txtContractattachmentfiles.length; i++) {
            fileData.append('Contractattachment', txtContractattachmentfiles[i], txtContractattachmentfiles[i].name);
        }
        for (var i = 0; i < txtOtherAttachmentsfiles.length; i++) {
            fileData.append('OtherAttachments', txtOtherAttachmentsfiles[i], txtOtherAttachmentsfiles[i].name);
        }
        fileData.append('ParentContractId', $(txtParentContractId).val());
        fileData.append('ContractId', $("#txt-id").val());
        fileData.append('Name', $(txtName).val());
        fileData.append('ContractType', $(txtContractType).val());
        fileData.append('ContractClassification', $(txtContractClassification).val());
        fileData.append('CostCenter4', $(txtCostCenter4).val());
        fileData.append('Pid4', $(txtPid4).val());
        fileData.append('Description', $(txtDescription).val());
        fileData.append('StartDate', $(txtStartDate).val());
        fileData.append('EndDate', $(txtEndDate).val());
        fileData.append('Vendor', $(txtVendor).val());
        fileData.append('MemoReference', $(txtMemoReference).val());
        fileData.append('BudgetAmount', $(txtBudgetAmount).val());
        fileData.append('Pid', $(txtPid).val());
        fileData.append('ProjectName', $(txtProjectName).val());
        fileData.append('CostCenter', $(txtCostCenter).val());
        fileData.append('CostCenter2', $(txtCostCenter2).val());
        fileData.append('Pid2', $(txtPid2).val());
        fileData.append('CostCenter3', $(txtCostCenter3).val());
        fileData.append('Pid4', $(txtPid4).val());
        fileData.append('CostCenter4', $(txtCostCenter4).val());
        fileData.append('Pid3', $(txtPid3).val());
        fileData.append('Department', $(txtDepartment).val());
        fileData.append('Currency', $(txtCurrency).val());
        fileData.append('YearlyContractCostWithoutVat', $(txtYearlyContractCostWithoutVat).val());
        fileData.append('CostBreakdown', $(txtCostBreakdown).val());
        console.log(fileData);
        $.ajax({
            url: actionUrl,
            contentType: false, // Not to set any content header 
            processData: false, // Not to process data 
            data: fileData,
            type: 'POST',
            success: function (response) {
                if (response.isSuccess) {

                    SportaUtil.MessageBoxSuccess(response.message, "Vendor Created");
                    closeSideBar(form_id);
                    loadApplicationContracts();

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
    //let txtPid, txtProjectName, txtPid4, txtPid2, txtPid3, txtCostCenter4, txtCostCenter2, txtCostCenter3,



    var blankChecks = [];
    if ($(txtContractType).val() == "Project") {
        blankChecks.push(txtPid, txtProjectName);
    }
    if ($(txtCostCenter2).val() != "") {
        blankChecks.push(txtPid2);
    }
    if ($(txtCostCenter3).val() != "") {
        blankChecks.push(txtPid3);
    }
    if ($(txtCostCenter4).val() != "") {
        blankChecks.push(txtPid4);
    }
    SportaForms.BlankInputChecks(blankChecks);
    if ($(txt_email).val()) {
        SportaForms.ValidateInput(txt_email,
            !/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                .test($(txt_email).val()),
            "Enter a valid email Id");
    }

    return SportaForms.FormValidationStatus(form_id);
}

function loadApplicationDepartments() {
    $.get(GetDepartments_url, function (response) {
        var tableColumns = [
            { data: "id" },
            { data: "name" },
            
            { data: null, render: usersActionButtons }
        ]
        SportaDataTable.CreateDataTable(aapplication_contract_list, response.data, tableColumns)
    });
}
function FormatDate(date) {
    var currentDate = new Date(date);
    var year = currentDate.getFullYear();
    var month = currentDate.getMonth() + 1; // month is zero-based, so we add 1
    var day = currentDate.getDate();
    month = month.toString().padStart(2, '0');
    day = day.toString().padStart(2, '0');
    return year + '-' + month + '-' + day;
}
// Buttons Added to Grid
function usersActionButtons(data, type, column) {
    var _buttons = '<div class="btn-group">'
        + `<button class="btn btn-sm btn-outline-grey"   title="Edit" onclick="openUserForm('${data.contractId}')"><i class="fas fa-pencil-alt"></i></button>`
        + `<button class="btn btn-sm btn-outline-grey"  title="Delete" onclick="deleteContract('${data.contractId}','${data.name}')"><i class="far fa-trash-alt"></i></button>`
        + `</div>`;
    return _buttons;
}
function deleteContract(id, name) {
    SportaUtil.ConfirmDialogue(`Are you sure  to delete ${name}?`, null, function () {

        $.get(deleteContractUrl, { 'Id': id }, function (response) {
            if (response.isSuccess)
                SportaUtil.MessageBoxSuccess(response.message);
            else
                SportaUtil.MessageBoxDanger(response.message);

            loadApplicationContracts();
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
