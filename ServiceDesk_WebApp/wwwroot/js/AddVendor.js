//UI varaibles
let form_id = "#vendor-form";
    //application_user_list = "#application-user-list";

//Forms
let txt_id = "#txt-id",
    txt_VendorName ="#txt-VendorName"
    txt_name = "#txt-name",
    txt_email = "#txt-email-id",
    btn_create_new = "#btn-create-new",
    btn_submit = "#btn-submit";

// opening Create User Form Or Edit User Form
function openUserForm(id, roleId) {
    SportaForms.ResetForm(form_id);
    if (id) {
        $.get(getUserUrl, { 'userId': id, 'roleId': roleId }, function (response) {
            var data = response.data;
            $(txt_id).val(data.id);
            $(txt_name).val(data.name);
            $(txt_email).val(data.username).prop('readonly', true);
            $(btn_submit).text('Update')
            $(form_id).fadeIn();
            SportaForms.InitializeFormStyle(form_id)
        })
    }
    else {
        $(txt_id).val(0);
        $(txt_email).prop('readonly', false);
        $(btn_submit).text('Save')

        $(form_id).fadeIn();
        SportaForms.InitializeFormStyle(form_id)
    }

}
// Closing User Form
function closeUserForm() {
    SportaForms.ResetForm(form_id);
    $(txt_id).val(0);
    $(txt_email).prop('readonly', false);
    $(btn_submit).text('Save')
    $(form_id).fadeOut();
    $(btn_create_new).show();
}