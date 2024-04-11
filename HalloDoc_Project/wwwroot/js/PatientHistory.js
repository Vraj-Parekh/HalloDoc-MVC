$(document).ready(function () {

    function loadFilteredData() {
        var firstName = $('#firstname').val();
        var lastName = $('#lastname').val();
        var email = $('#email').val();
        var phoneNumber = $('#phonenumber').val();

        console.log(firstName);
        console.log(lastName);
        console.log(email);
        console.log(phoneNumber);

        $.ajax({
            url: '/Provider/PatientHistoryTable',
            type: 'GET',
            data: {
                firstName: firstName,
                lastName: lastName,
                email: email,
                phoneNumber: phoneNumber
            },
            success: function (data) {
                $('#historyTableDiv').html(data);
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
        $('#lastname').val('');
        $('#email').val('');
        $('#phonenumber').val('');

        loadFilteredData();
    });

    // Initial loading of table data
    loadFilteredData();
});