$(document).ready(function () {

    $.ajax({
        url: '/Admin/FetchProfession',
        method: 'GET',
        success: function (response) {
            var dropdown = $('#proffessionType');
            response.forEach(function (res) {
                dropdown.append($('<option></option>').attr('value', res.healthprofessionalid).text(res.professionname));
            });
        },
        error: function (response) {
            console.log('error');
        }
    });

    function loadFilteredData() {
        var searchVendor = $("#searchVendor").val().trim();
        console.log(searchVendor);

        var professionType = $("#proffessionType").val();
        console.log(professionType);

        $.ajax({
            url: '/Provider/VendorsTable',
            type: 'GET',
            data: { searchVendor: searchVendor, professionType: professionType },
            success: function (data) {
                $('#vendorTableDiv').html(data);
                console.log("success");
            },
            error: function (xhr, status, error) {
                console.log("error");
            }
        });
    }

    $("#proffessionType").on('change',function () {
        loadFilteredData();
    });
    $("#searchVendor").on('input',function () {
        loadFilteredData();
    });

    // Initial loading of table data
    loadFilteredData();
});