let getAllRequestVendorUrl = '/Vendor/GetPaymentRequest',
    request_list = "#request-paymentVendor-list";


$(document).ready(function () {
    loadPaymentVendorRequests();

});


function loadPaymentVendorRequests() {
    $.get(getAllRequestVendorUrl, function (response) {
        var tableColumns = [
            //{ data: "name" },
            //{ data: "email" },
            { data: "contractTitle" },
            { data: "projectName" },
            { data: "classification" },
            { data: "department" },
            /*{ data: "created" },*/

            // { data: null, render: usersActionButtons }

        ]
        SportaDataTable.CreateDataTable(request_list, response.data, tableColumns)
    });
}