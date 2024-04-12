$(document).ready(function () {

    function loadFilteredData() {
        var role = $('#role').val();
        var receiverName = $('#receivername').val();
        var emailId = $('#emailid').val();
        var createdDate = $('#createdDate').val();
        var sentDate = $('#sentDate').val();

        console.log(role);
        console.log(receiverName);
        console.log(emailId);
        console.log(createdDate);
        console.log(sentDate);

        $.ajax({
            url: '/Provider/SmsLogsTable',
            type: 'GET',
            data: {
                role: role,
                receiverName: receiverName,
                emailId: emailId,
                createdDate: createdDate,
                sentDate: sentDate
            },
            success: function (data) {
                $('#smsLogsTableDiv').html(data);
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
        $('#role').val(0);
        $('#receivername').val('');
        $('#emailid').val('');
        $('#createdDate').val('');
        $('#sentDate').val('');

        loadFilteredData();
    });

    // Initial loading of table data
    loadFilteredData();
});
