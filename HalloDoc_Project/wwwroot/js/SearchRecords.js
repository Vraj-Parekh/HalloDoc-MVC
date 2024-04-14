var page = 1;
var itemsPerPage = 10;

function loadFilteredData(page) {
    var requestStatus = $('#requestStatus').val();
    var patientName = $('#patientname').val();
    var requestType = $('#requestType').val();
    var fromDateOfService = $('#fromDateofService').val();
    var toDateOfService = $('#toDateofService').val();
    var providerName = $('#providername').val();
    var email = $('#email').val();
    var phoneNumber = $('#phonenumber').val();

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
            providerName: providerName,
            page: page,
            itemsPerPage: itemsPerPage
        },
        success: function (data) {
            $('#searchRecordsTableDiv').html(data);
            console.log("success");
        },
        error: function (xhr, status, error) {
            $('#searchRecordsTableDiv').html("<h3 class='text-center'>No data found</h3>");
            console.log("error");
        }
    });
}

$('#searchButton').click(function () {
    page = 1;
    loadFilteredData(page);
});

$('#clearButton').click(function () {
    $('#patientname').val('');
    $('#email').val('');
    $('#phonenumber').val('');
    $('#requestStatus').val(0);
    $('#requestType').val(0);
    $('#fromDateofService').val('');
    $('#toDateofService').val('');
    $('#providername').val('');
    page = 1;

    loadFilteredData(page);
});

$('#exportToExcel').click(function () {
    $.ajax({
        type: 'POST',
        url: '/Admin/ExportSearchRecords',
        data: {
            patientName: patientName,
            email: email,
            phoneNumber: phoneNumber,
            requestStatus: requestStatus,
            requestType: requestType,
            fromDateOfService: fromDateOfService,
            toDateOfService: toDateOfService,
            providerName: providerName,
            page: page,
            itemsPerPage: itemsPerPage
        },
        xhrFields: {
            responseType: 'blob' // Set the response type to blob
        },
        success: function (response) {
            var blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = 'patient_records.xlsx';
            link.click();
        },
        error: function (xhr, status, error) {
            console.error("Error:", status, error);
        }
    });
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