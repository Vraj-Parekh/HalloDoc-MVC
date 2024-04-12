$(document).ready(function () {

    function loadFilteredData() {
        var requestStatus = $('#requestStatus').val();
        var patientName = $('#patientname').val();
        var requestType = $('#requestType').val();
        var fromDateOfService = $('#fromDateofService').val();
        var toDateOfService = $('#toDateofService').val();
        var providerName = $('#providername').val();
        var email = $('#email').val();
        var phoneNumber = $('#phonenumber').val();

        console.log(requestStatus);
        console.log(patientName);
        console.log(requestType);
        console.log(fromDateOfService);
        console.log(toDateOfService);
        console.log(providerName);
        console.log(email);
        console.log(phoneNumber);
        $.ajax({
            url: '/Provider/SearchRecordsTable',
            type: 'GET',
            data: {
                patientName: patientName,
                email: email,
                phoneNumber: phoneNumber,
                requestStatus: requestStatus,
                requestType: requestType,
                fromDateOfService: fromDateOfService,
                toDateOfService: toDateOfService,
                providerName: providerName
            },
            success: function (data) {
                $('#searchRecordsTableDiv').html(data);
                console.log("success");
            },
            error: function (xhr, status, error) {
                console.log("error");
            }
        });
    }

    $('#searchButton').click(function () {
        loadFilteredData();
    });

    $('#clearButton').click(function () {
        $('#firstname').val('');
        $('#email').val('');
        $('#phonenumber').val('');
        $('#requestStatus').val(0);
        $('#requestType').val(0);
        $('#fromDateofService').val('');
        $('#toDateofService').val('');
        $('#providername').val('');

        loadFilteredData();
    });

    // Initial loading of table data
    loadFilteredData();
});
