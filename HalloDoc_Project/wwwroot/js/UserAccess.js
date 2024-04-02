$(document).ready(function () {
    $('#accountType').change(function () {
        var selectedValue = $(this).val();

        if (selectedValue === "1") {
            // If "All" --> hide the button
            $('#create-acc-btn').hide();
        } else {
            // If "Admin" or "Provider" is selected, show the button and update its text
            $('#create-acc-btn').show();
            if (selectedValue === "2") {
                // If "Admin" is selected, set the button text to "Create Admin Account"
                $('#create-acc-btn').text('Create Admin Account');
                $('#create-acc-btn').off('click').on('click', function () {
                    window.location.href = '/Admin/CreateAdminAccount';
                });
            } else if (selectedValue === "3") {
                // If "Provider" is selected, set the button text to "Create Provider Account"
                $('#create-acc-btn').text('Create Provider Account');
                $('#create-acc-btn').off('click').on('click', function () {
                    window.location.href = '/Provider/CreateProviderAccount';
                });
            }
        }
    });

     $('#accountType').trigger('change');
});
