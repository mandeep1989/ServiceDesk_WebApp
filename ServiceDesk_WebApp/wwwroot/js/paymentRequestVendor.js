﻿let getAllRequestVendorUrl = '/Vendor/GetPaymentRequest',
    request_list = "#request-paymentVendor-list";


$(document).ready(function () {
    loadPaymentVendorRequests();

});


function loadPaymentVendorRequests() {
    $.get(getAllRequestVendorUrl, function (response) {
        var tableColumns = [
            //{ data: "name" },
            //{ data: "email" },
            { data: "ticketid" },
            { data: "contractTitle" },
            { data: "projectName" },
            { data: "classification" },
            { data: "department" },
           // { data: "created" },
            { data: 'created', render: function (date) { return moment(date).format('DD-MM-YYYY') } },
        

            // { data: null, render: usersActionButtons }

        ]
        SportaDataTable.CreateDataTable(request_list, response.data, tableColumns)
    });
}