$(document).ready(function () {
    // Disable all input fields initially
    $('.dis-field').prop('disabled', true);
    $('.btn-save').hide();

    // Handle click event for the Edit button
    $('.btn-edit').click(function () {
        // Enable all input fields
        $('.dis-field').prop('disabled', false);

        $('.btn-save').show();
    });
});