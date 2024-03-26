$(document).ready(function () {

    //-------------for admin info-------------
    // Disable all input fields initially
    $('.dis-admin-field').prop('disabled', true);
    $('.btn-save').hide();

    // Handle click event for the Edit button
    $('.btn-edit-admin').click(function () {
        // Enable all input fields
        $('.dis-admin-field').prop('disabled', false);

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


    //-------------for updating admin info-------------
    $('.btn-save').on('click', function () {

        var firstname = $('#firstname').val();
        var lastname = $('#lastname').val();
        var  email= $('#email').val();
        var phonenumber= $('#phone').val();

        var data = {
            FirstName: firstname,
            LastName: lastname,
            Email: email,
            PhoneNumber: phonenumber,
        };

        $.ajax({
            url: '/Admin/UpdateAdminInfo',
            method: 'POST',
            data: data,
            success: function (response) {
                console.log('admin updated successfully');
                window.location.reload();
            },
            error: function (xhr, status, error) {
                console.log('Error');
            }
        });
    });


    //-------------for updating billing info-------------
    $('.btn-save-billing').on('click', function () {

        var address1 = $('#address1').val();
        var address2 = $('#address2').val();
        var city = $('#city').val();
        var state = $('#state').val();
        var zip = $('#zip').val();
        var altPhoneNumber = $('#phone1').val();

        var data = {
            Address1: address1,
            Address2: address2,
            City: city,
            State: state,
            Zip: zip,
            AltPhoneNumber: altPhoneNumber
        };

        $.ajax({
            url: '/Admin/UpdateBillingInfo',
            method: 'POST',
            data: data,
            success: function (response) {
                console.log('billing updated successfully');
                window.location.reload();
            },
            error: function (xhr, status, error) {
                console.log('Error');
            }
        });
    });

    //-------------ajax for state dropdown-------------
    $.ajax({
        url: './FetchRegions',
        method: 'GET',
        success: function (response) {
            response.forEach(function (res) {
                console.log("calls region");
                $('.regionDropDown').append("<option value='" + res.regionid + "'>" + res.name + "</option>");
            });
        }
    });
});