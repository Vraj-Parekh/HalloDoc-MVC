$(document).ready(function () {

    //-------------for admin info-------------
    // Disable all input fields initially
    $('.checkedregion').prop('disabled', true);
    $('.dis-admin-field').prop('disabled', true);
    $('.btn-save').hide();

    // Handle click event for the Edit button
    $('.btn-edit-admin').click(function () {
        // Enable all input fields
        $('.dis-admin-field').prop('disabled', false);
        $('.checkedregion').prop('disabled', false);

        $('.btn-save').show();
    });


    //-------------for billing info-------------
    // Disable all input fields initially
    $('.dis-billing-field').prop('disabled', true);
    $('.btn-save-billing').hide();

    // Handle click event for the Edit button
    $('.btn-edit-billing').click(function () {
        // Enable all input fields
        $('.dis-billing-field').prop('disabled', false);

        $('.btn-save-billing').show();
    });

    //-------------for profile info-------------
    // Disable all input fields initially
    $('.dis-profile-field').prop('disabled', true);
    $('.btn-save-profile').hide();

    // Handle click event for the Edit button
    $('.btn-profile').click(function () {
        // Enable all input fields
        $('.dis-profile-field').prop('disabled', false);

        $('.btn-save-profile').show();
    });
});