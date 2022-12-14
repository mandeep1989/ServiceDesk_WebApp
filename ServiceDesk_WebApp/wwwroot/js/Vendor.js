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

//PaymentRequestForm
let req_form_id = "#payment_req_form",
    txt_ContractTitle = "#ContractTitle",
    txt_StartDate = "#StartDate",
    txt_Department = "#Department",
    txt_EndDate = "#EndDate",
    txt_Classification = "#Classification",
    txt_ApplicationName = "#ApplicationName",
    txt_ContractRefType = "#ContractRefType",
    txt_ProjectName = "#ProjectName",
    txt_InvoiceNumber = "#InvoiceNumber",
    txt_InvoiceDate = "#InvoiceDate",
    txt_InvoiceAmount = "#InvoiceAmount",
    txt_Details = "#Details",
    file_OrignalInvoice = "#OrignalInvoice",
    file_ServiceConfirmation = "#ServiceConfirmation",
    file_CopyOfApproval = "#CopyOfApproval",
    txt_PaymentMode = "#PaymentMode",
    txt_BankName = "#BankName",
    txt_IBAN = "#IBAN",
    txt_AccountName = "#AccountName",
    txt_SwiftCode = "#SwiftCode",
    txt_AccountNumber = "#AccountNumber",
    txt_Branch = "#Branch",
    payment_url = "/Vendor/AddRequestForm",
    txt_Contract = "#Contract";


$(document).ready(function () {

    checkEscalationStatus();
    var csv_path = "./wwwroot/CSVFile/Book1.csv";
    $.get("/Vendor/GetCsv", function (data) {
       // var csv = CSVToArray(data);
       renderCSVDropdown(data);
    });
    ContractOnChange();
});


var ContractOnChange = function () {
    $("#Contract").on("change", function () {
        $.get(`/Vendor/GetContractById/${this.value}`, function (data) {
            bindContractData(data.data);
        });
    })
}
function bindContractData(data) {

// Set the value of the <input> element with a type of "date" to the formatted 
    $(txt_StartDate).val(FormatDate(data.from_date.display_value)); 
    $(txt_EndDate).val(FormatDate(data.to_date.display_value)); 
    $(txt_ContractTitle).val(data.name);
    $(txt_Department).val(data.user.department.name)  ;
    $(txt_InvoiceAmount).val(data.total_price) ;
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

var renderCSVDropdown = function (csv) {
   /* let email = userEmail.split("@")[1];*/
    var dropdown = $('#Contract');
    for (var i = 0; i < csv.data.length; i++) {
        //if (csv[i][1] && email) {
        //    if (csv[i][1].toLowerCase() == email.toLowerCase()) {
                var entry = $('<option>', { value: csv.data[i].id, text: csv.data[i].name})
                dropdown.append(entry);
        //    }
        //}
    }
};

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

function closePaymentForm() {
    SportaForms.ResetForm(req_form_id);
    $("#payment_req_modal").modal("hide");
}
function openPaymentForm() {
    SportaForms.ResetForm(req_form_id);
    SportaForms.InitializeFormStyle(req_form_id);
    $("#payment_req_modal").modal("show");
}

function openEscalationForm() {
    SportaForms.ResetForm(form_id);
    SportaForms.InitializeFormStyle(form_id);   
    $("#escalation_form").modal("show");
}
//--------------//


$(req_form_id).unbind().bind('submit', function (e) {
    e.preventDefault();
    SportaForms.EnableLiveValidation(req_form_id, validatePaymentRequestForm)
    if (validatePaymentRequestForm()) {
        var orginalInvoiceUpload = $(file_OrignalInvoice).get(0);
        var orginalInvoicefiles = orginalInvoiceUpload.files;

        var CopyOfApprovalUpload = $(file_CopyOfApproval).get(0);
        var CopyOfApprovalfiles = CopyOfApprovalUpload.files;


        var ServiceConfirmationUpload = $(file_ServiceConfirmation).get(0);
        var ServiceConfirmationfiles = ServiceConfirmationUpload.files;

        // Create FormData object 
        var fileData = new FormData();
        // Looping over all files and add it to FormData object 
        for (var i = 0; i < orginalInvoicefiles.length; i++) {
            fileData.append('orginalInvoice', orginalInvoicefiles[i], orginalInvoicefiles[i].name);
        }
        for (var i = 0; i < CopyOfApprovalfiles.length; i++) {
            fileData.append('CopyOfApproval', CopyOfApprovalfiles[i], CopyOfApprovalfiles[i].name);
        }
        for (var i = 0; i < ServiceConfirmationfiles.length; i++) {
            fileData.append('ServiceConfirmation', ServiceConfirmationfiles[i], ServiceConfirmationfiles[i].name);
        }
        //Adding  more key to FormData object 
        fileData.append('ContractTitle', $(txt_ContractTitle).val());
        fileData.append('StartDate', $(txt_StartDate).val());
        fileData.append('EndDate', $(txt_EndDate).val());
        fileData.append('Department', $(txt_Department).val());
        fileData.append('Classification', $(txt_Classification).val());
        fileData.append('ApplicationName', $(txt_ApplicationName).val());
        fileData.append('ContractRefType', $(txt_ContractRefType).val());
        fileData.append('ProjectName', $(txt_ProjectName).val());
        fileData.append('InvoiceNumber', $(txt_InvoiceNumber).val());
        fileData.append('Invoicedate', $(txt_InvoiceDate).val());
        fileData.append('Details', $(txt_Details).val());
        fileData.append('InvoiceAmount', $(txt_InvoiceAmount).val());
        fileData.append('PaymentMode', $(txt_PaymentMode).val());
        fileData.append('BankName', $(txt_BankName).val());
        fileData.append('IBAN', $(txt_IBAN).val());
        fileData.append('AccountName', $(txt_AccountName).val());
        fileData.append('AccountNumber', $(txt_AccountNumber).val());
        fileData.append('SwiftCode', $(txt_SwiftCode).val());
        fileData.append('Branch', $(txt_Branch).val());
        fileData.append('Contract', $(txt_Contract).val());
        console.log(fileData);
        console.log($(txt_Contract).val());
        console.log($(txt_PaymentMode).val());

        $.ajax({
            url: payment_url,
            type: "POST",
            contentType: false, // Not to set any content header 
            processData: false, // Not to process data 
            data: fileData,
            success: function (response) {
                if (response.isSuccess) {
                    SportaUtil.MessageBoxSuccess(response.message);
                    SportaForms.ResetForm(req_form_id);
                    $("#payment_req_modal").modal("hide");

                }
                else {
                    SportaUtil.MessageBoxDanger(response.message);
                }
            },
            error: function (xhr, error, errorMessage) {
                SportaUtil.MessageBoxDanger(errorMessage);
            }
        });
    }
});


function validatePaymentRequestForm() {
    SportaForms.ClearValidataionErrors(req_form_id);
    var blankChecks = [txt_ContractTitle, txt_StartDate, txt_EndDate, txt_Department, txt_Classification, txt_ApplicationName, txt_ContractRefType, txt_ProjectName, txt_InvoiceNumber, txt_Details, txt_PaymentMode, txt_BankName, txt_IBAN, txt_AccountName, txt_SwiftCode, txt_Branch, txt_InvoiceAmount, txt_AccountNumber, txt_InvoiceDate, txt_Contract];
    SportaForms.BlankInputChecks(blankChecks);
    //if (!$(txt_Contract).val()) {
    //    debugger
    //    SportaForms.ValidateInput(txt_Contract,
    //        !$(txt_Contract).val(),
    //        "Please choose contract");
    //}
    return SportaForms.FormValidationStatus(req_form_id);
}

function CSVToArray(strData, strDelimiter) {

    strDelimiter = (strDelimiter || ",");

    var objPattern = new RegExp((
        "(\\" + strDelimiter + "|\\r?\\n|\\r|^)" +

        "(?:\"([^\"]*(?:\"\"[^\"]*)*)\"|" +

        "([^\"\\" + strDelimiter + "\\r\\n]*))"
    ), "gi");


    var arrData = [
        []
    ];

    var arrMatches = null;

    while (arrMatches = objPattern.exec(strData)) {

        var strMatchedDelimiter = arrMatches[1];

        if (strMatchedDelimiter.length && strMatchedDelimiter !== strDelimiter) {

            arrData.push([]);

        }

        var strMatchedValue;

        if (arrMatches[2]) {

            strMatchedValue = arrMatches[2].replace(new RegExp("\"\"", "g"), "\"");

        } else {
            strMatchedValue = arrMatches[3];

        }

        arrData[arrData.length - 1].push(strMatchedValue);
    }

    return (arrData);
}



