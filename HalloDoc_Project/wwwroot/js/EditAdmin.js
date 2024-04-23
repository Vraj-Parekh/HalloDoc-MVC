$(document).ready(function () {

    //-------------for pass info-------------
    // Disable all input fields initially
    $('.dis-pass').prop('disabled', true);
    $('.btn-save-pass').hide();

    // Handle click event for the Edit button
    $('.btn-edit-pass').click(function () {
        // Enable all input fields
        $('.dis-pass').prop('disabled', false);

        $('.btn-save-pass').show();
    });


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


    //-------------for updating admin info-------------
    $('.btn-save').on('click', function () {
        let isValid = true;

        if ($('#firstname').val() == '') {
            $('#f-name').text('Firstname is required');
            isValid = false;
        } else {
            $('#f-name').text('');
        }
        if ($('#lastname').val() == '') {
            $('#l-name').text('Lastname is required');
            isValid = false;
        } else {
            $('#l-name').text('');
        }

        if ($('#email1').val() == '') {
            $('#email-text').text('Email is required');
            isValid = false;
        } else {
            $('#email-text').text('');
        }

        if ($('#confirmEmail').val() == '') {
            $('#conf-email').text('Confirm email is required');
            isValid = false;
        } else {
            $('#conf-email').text('');
        }

        if ($('#email1').val() !== $('#confirmEmail').val()) {
            $('#conf-email').text('Email not matching');
            isValid = false;
        }
        else {
            $('#conf-email').text('');
        }

        if ($('.phn').val() == '') {
            $('#phonenum').text('Phone number is required');
            isValid = false;
        } else {
            $('#phonenum').text('');
        }


        if (!isValid) {
            return;
        }

        var firstname = $('#firstname').val();
        var lastname = $('#lastname').val();
        var email = $('#email1').val();
        var phonenumber = $('#phone').val();
        var adminId = $('#adminId').val();

        var checkedMenus = [];

        var checkedRegion = $(".checkedregion:checked")
        for (var i = 0; i < checkedRegion.length; i++) {
            console.log(checkedRegion[i].id);
            var Menu = { RegionId: checkedRegion[i].id, IsPresent: true };
            checkedMenus.push(Menu);

        }
        var data = {
            FirstName: firstname,
            LastName: lastname,
            Email: email,
            PhoneNumber: phonenumber,
            Regions: checkedMenus,
            AdminId: adminId
        };
        console.log(data);
        $.ajax({
            url: '/admin/UpdateAdminInfoEdit',
            method: 'post',
            data: data,
            async: false,
            success: function (response) {
                console.log('admin updated successfully');
                window.location.reload();
            },
            error: function (xhr, status, error) {
                console.log('error');
            }
        });
    });
});