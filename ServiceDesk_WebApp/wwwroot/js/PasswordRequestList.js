let getAllRequestUrl = '/Admin/PasswordRequestList',
    forget_pw_list = "#forget-pw-list";
$(document).ready(function () {
    loadPasswordRequests();

});



function loadPasswordRequests() {
    $.get(getAllRequestUrl, function (response) {
        var tableColumns = [
            { data: "name" },
            { data: "email" },
            {
                data: null, render: function (data) {
                    var bsClass;
                    var value;
                    if (data.status == 0) {
                        bsClass = "text-warning"
                        value="Initaited"
                    }
                    else 
                    {
                        bsClass = "text-warning"
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
        + `<button class="btn btn-sm btn-outline-grey"   title="Edit" onclick="openUserForm('${data.id}')"><i class="fa-solid fa-key"></i></button>`
        + `</div>`;
    return _buttons;
}