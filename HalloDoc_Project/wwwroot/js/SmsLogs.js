var page = 1;
var itemsPerPage = 10;

function loadFilteredData(page) {
    var role = $('#role').val();
    var receiverName = $('#receivername').val();
    var phoneNumber = $('#phoneNumber').val();
    var createdDate = $('#createdDate').val();
    var sentDate = $('#sentDate').val();

    $.ajax({
        url: '/Admin/SmsLogsTable',
        type: 'GET',
        data: {
            role: role,
            receiverName: receiverName,
            phoneNumber: phoneNumber,
            createdDate: createdDate,
            sentDate: sentDate,
            page: page,
            itemsPerPage: itemsPerPage
        },
        success: function (data) {
            $('#smsLogsTableDiv').html(data);
            console.log("success");
        },
        error: function (xhr, status, error) {
            $('#smsLogsTableDiv').html("<h3 class='text-center'>No data found</h3>");
            console.log("error");
        }
    });
}

$('#searchButton').click(function () {
    page = 1;
    loadFilteredData(page);
});

$('#clearButton').click(function () {
    $('#role').val(0);
    $('#receivername').val('');
    $('#phoneNumber').val('');
    $('#createdDate').val('');
    $('#sentDate').val('');
    page = 1;

    loadFilteredData(page);
});

// Initial loading of table data
loadFilteredData(1);

function previousPage() {
    if (page > 1) {
        page--;
        loadFilteredData(page);
    }
    else {
        console.log('else')
        $('#prevBtn').prop('disabled', true);
    }
}
function changePage(pageNumber) {
    page = pageNumber;
    loadFilteredData(page);
}
function nextPage(totalPages) {
    //console.log(totalPages)
    if (page < totalPages) {
        page++;
        loadFilteredData(page);
    }
}

