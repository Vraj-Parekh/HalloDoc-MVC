$(document).ready(function () {

    $('.dis-pass').prop('disabled', true);
    $('.btn-save-pass').hide();

    // Handle click event for the Edit button
    $('.btn-edit-pass').click(function () {
        // Enable all input fields
        $('.dis-pass').prop('disabled', false);

        $('.btn-save-pass').show();
    });


    //-------------for provider info-------------
    // Disable all input fields initially
    $('.checkedregion').prop('disabled', true);
    $('.dis-admin-field').prop('disabled', true);
    $('.btn-phy-save').hide();

    // Handle click event for the Edit button
    $('.btn-edit-admin').click(function () {
        // Enable all input fields
        $('.dis-admin-field').prop('disabled', false);
        $('.checkedregion').prop('disabled', false);

        $('.btn-phy-save').show();
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

    //for photo
    $('.dis-provider-field').prop('disabled', true);
    $('.btn-save-provider-profile').hide();

    // Handle click event for the Edit button
    $('.btn-profile').click(function () {
        // Enable all input fields
        $('.dis-provider-field').prop('disabled', false);

        $('.btn-save-provider-profile').show();
    });

    $('.btn-phy-save').on('click', function () {
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
        var medicalLicense = $('#medicalLicense').val();
        var npiNumber = $('#npiNumber').val();
        var phyId = $('#phyId').val();

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
            MedicalLicense: medicalLicense,
            NPINumber: npiNumber,
            PhysicianId: phyId
        };
        console.log(data);
        $.ajax({
            url: '/Admin/UpdatePhysicianInfo',
            method: 'post',
            data: data,
            async: false,
            success: function (response) {
                console.log('updated successfully');
                window.location.reload();
            },
            error: function (xhr, status, error) {
                console.log('error');
            }
        });
    });

});
//function photo_validation() {
//    var val = $('#fileLoader').val().toLowerCase(),
//        regex = new RegExp("(.*?)\.(jpg|png)$");
//    if (val == null || val == "") {
//        return true;
//    }
//    if (!(regex.test(val))) {
//        $('#photo_validation').html('Please select only jpg or png type photo');
//        $('#photo_img').attr("src", "");
//        return false;
//    }
//    $('#photo_validation').html('');
//    var file = document.getElementById('fileLoader').files[0];
//    if (file) {
//        $('#photo_img').attr("src", URL.createObjectURL(file))
//    }
//    return true;
//};
//function signature_validation() {
//    var val = $('#sign').val().toLowerCase(),
//        regex = new RegExp("(.*?)\.(jpg|png)$");
//    if (val == null || val == "") {
//        return true;
//    }
//    if (!(regex.test(val))) {
//        $('#filename').html('Please select only jpg or png type signature');
//        $('#sign_img').attr("src", "");
//        return false;
//    }
//    $('#filename').html('');
//    var file = document.getElementById('sign').files[0];
//    if (file) {
//        $('#sign_img').attr("src", URL.createObjectURL(file))
//    }
//    return true;
//};
