$(document).ready(function () {
    // Disable all input fields initially
    $('input, select,.btn-map').prop('disabled', true);
    $('.btn-save').hide();

    // Handle click event for the Edit button
    $('.btn-edit').click(function () {
        // Enable all input fields
        $('input, select,.btn-map').prop('disabled', false);
        $('.btn-save').show();
    });
});