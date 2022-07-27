let getAllRequestUrl = '/Admin/GetPaymentRequest',
     request_list=  "#request-payment-list";


$(document).ready(function () {
    loadPaymentRequests();

});


function loadPaymentRequests() {
    $.get(getAllRequestUrl, function (response) {
        var tableColumns = [
            { data: "name" },
            { data: "email" },
            { data: "contractTitle" },
            { data: "projectName" },
            { data: "classification" },
            { data: "department" },
            { data: 'created', render: function (date) { return moment(date).format('DD-MM-YYYY') } },
           
           // { data: null, render: usersActionButtons }

        ]
        SportaDataTable.CreateDataTable(request_list, response.data, tableColumns)
    });
}