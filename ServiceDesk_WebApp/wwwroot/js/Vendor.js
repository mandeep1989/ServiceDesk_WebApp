////$(document).ready(function () {


////    $.ajax({
////        type: "GET",
////        url: "/Admin/GetVendor",
////        data: '{}',
////        contentType: "application/json; charset=utf-8",
////        dataType: "json",
////        success: OnSuccess,
////        failure: function (response) {
////            alert(response.d);
////        },
////        error: function (response) {
////            alert(response.d);
////        }
////    });
////});

////function OnSuccess(response) {
   
////    $("#Vendor").DataTable(
////        {
////            bLengthChange: true,
////            lengthMenu: [[5, 10, -1], [5, 10, "All"]],
////            bFilter: true,
////            bSort: true,
////            bPaginate: true,
////            data: response,
////            columns: [
////            { 'data': 'vendorNo' },
////            { 'data': 'vendorName' },
////            { 'data': 'residencyStatus' },
////            { 'data': 'poRemarks' },
////            { 'data': 'currency' },
////            { 'data': 'email' },
////            { data: null, render: driveActionButtons },]
////        });
////};
////function driveActionButtons(data, type, column) {
    
////    var _buttons = '<div class="btn-group">'
////        + `<button class="btn btn-sm btn-outline-grey" data-toggle="tooltip" title="Edit" onclick="EditVendor('${data.id}')"><i class="fas fa-pencil-alt"></i></button>`
////        + `<button class="btn btn-sm btn-outline-grey" data-toggle="tooltip" title="Delete" onclick="deleteVendor('${data.id}')"><i class="fa fa-trash"></i></button>`
////        + `</div>`;

////    return _buttons;

////}   