
let vendor_count_by_date = '/Admin/GetVendorCountByDate',
    get_total_vendor_count = '/Admin/GetAllVendors',
    request_count_by_date = '/Admin/GetRequestCountByDate',
    get_total_request_count = '/Admin/GetPaymentRequest';


$(document).ready(function () {
    VendorCounts();
    totalVendorCount();
    RequestCountsByDate();
    totalRequestCount();
});

//window.addEventListener('DOMContentLoaded', event => {
//    $('.navbar-toggler').dropdown();
//    // Toggle the side navigation
//    const sidebarToggle = document.body.querySelector('#sidebarToggle');
//    if (sidebarToggle) {
//        // Uncomment Below to persist sidebar toggle between refreshes
//        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
//        //     document.body.classList.toggle('sb-sidenav-toggled');
//        // }
//        sidebarToggle.addEventListener('click', event => {
//            event.preventDefault();
//            document.body.classList.toggle('sb-sidenav-toggled');
//            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
//        });
//    }
  
//});
//Total Vendor Count By Date
function VendorCounts() {
    $.get(vendor_count_by_date, function (response) {

        if (response) {
            var result = "";
            $("#tableBody").empty();
            result = "";
            result = result + "<td>" + response.data.todayCount + "</td>";
            result = result + "<td>" + response.data.yesterDayCount + "</td>";
            result = result + "<td>" + response.data.last7DaysCount + "</td>";
            result = result + "<td>" + response.data.last30DaysCount + "</td>";
            result = result + "<td>" + response.data.last90DaysCount + "</td>";
            $("#tableBody").append(result);
        }
    });
}

//Total Vendor Count By Date
function RequestCountsByDate() {
    $.get(request_count_by_date, function (response) {

        if (response) {
            var result = "";
            $("#tableBody1").empty();
            result = "";
            result = result + "<td>" + response.data.todayCount + "</td>";
            result = result + "<td>" + response.data.yesterDayCount + "</td>";
            result = result + "<td>" + response.data.last7DaysCount + "</td>";
            result = result + "<td>" + response.data.last30DaysCount + "</td>";
            result = result + "<td>" + response.data.last90DaysCount + "</td>";
            $("#tableBody1").append(result);
        }
    });
}


//Total Vendor Count
function totalVendorCount() {
    
    $("#VendorCount").empty();
    $.get(get_total_vendor_count, function (response) {
        $("#VendorCount").append("<h5>" + response.data.length + "</h5>");
    });
}

//Total Request Count
function totalRequestCount() {

    $("#requestCount").empty();
    $.get(get_total_request_count, function (response) {
        $("#requestCount").append("<h5>" + response.data.length + "</h5>");
    });
}
