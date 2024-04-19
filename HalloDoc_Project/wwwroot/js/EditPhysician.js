$(document).ready(function () {

    //-------------for provider info-------------
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

        var firstname = $('#firstname').val();
        var lastname = $('#lastname').val();
        var email = $('#email').val();
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
            url: '/Provider/UpdatePhysicianInfo',
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

    //-------------for updating billing info-------------
    $('.btn-save-billing').on('click', function () {

        var address1 = $('#address1').val();
        var address2 = $('#address2').val();
        var city = $('#city').val();
        var state = $('#state').val();
        var zip = $('#zip').val();
        var altPhoneNumber = $('#phone1').val();
        var phyId = $('#phyId').val();

        var data = {
            Address1: address1,
            Address2: address2,
            City: city,
            State: state,
            Zip: zip,
            AltPhoneNumber: altPhoneNumber,
            PhysicianId: phyId
        };

        $.ajax({
            url: '/Provider/UpdatePhysicianBillingInfo',
            method: 'POST',
            data: data,
            async: false,
            success: function (response) {
                console.log('billing updated successfully');
                window.location.reload();
            },
            error: function (xhr, status, error) {
                console.log('Error');
            }
        });
    });

    $('.btn-save-provider-profile').on('click', function () {

        var businessName = $('#businessName').val();
        var businessWebsite = $('#businessWebsite').val();
        var Adminnotes = $('#Adminnotes').val();
        var photo = document.getElementById('fileLoader').files[0];
        var sign = document.getElementById('sign').files[0];
        var phyId = $('#phyId').val();

        var data = {
            BusinessName: businessName,
            BusinessWebsite: businessWebsite,
            AdminNotes: Adminnotes,
            Photo: photo,
            Signature: sign,
            PhysicianId: phyId
        };
        console.log(data);
        $.ajax({
            url: '/Provider/UpdatePhysicianProfileInfo',
            method: 'POST',
            data: data,
            async: false,
            success: function (response) {
                console.log('successfully');
                window.location.reload();
            },
            error: function (xhr, status, error) {
                console.log('Error');
            }
        });
    });
});
function photo_validation() {
    var val = $('#fileLoader').val().toLowerCase(),
        regex = new RegExp("(.*?)\.(jpg|png)$");
    if (val == null || val == "") {
        return true;
    }
    if (!(regex.test(val))) {
        $('#photo_validation').html('Please select only jpg or png type photo');
        $('#photo_img').attr("src", "");
        return false;
    }
    $('#photo_validation').html('');
    var file = document.getElementById('fileLoader').files[0];
    if (file) {
        $('#photo_img').attr("src", URL.createObjectURL(file))
    }
    return true;
};
function signature_validation() {
    var val = $('#sign').val().toLowerCase(),
        regex = new RegExp("(.*?)\.(jpg|png)$");
    if (val == null || val == "") {
        return true;
    }
    if (!(regex.test(val))) {
        $('#filename').html('Please select only jpg or png type signature');
        $('#sign_img').attr("src", "");
        return false;
    }
    $('#filename').html('');
    var file = document.getElementById('sign').files[0];
    if (file) {
        $('#sign_img').attr("src", URL.createObjectURL(file))
    }
    return true;
};
