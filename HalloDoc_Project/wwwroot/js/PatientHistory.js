var page = 1;
var itemsPerPage = 10;

function loadFilteredData(page) {
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
            phoneNumber: phoneNumber,
            page: page,
            itemsPerPage: itemsPerPage
        },
        success: function (data) {
            $('#historyTableDiv').html(data);
            console.log("success");
        },
        error: function (xhr, status, error) {
            $('#historyTableDiv').html("<h3 class='text-center'>No data found</h3>");
            console.log("error");
        }
    });
}

$('#searchButton').click(function () {
    page = 1;
    loadFilteredData(page);
});

$('#clearButton').click(function () {

    $('#firstname').val('');
    $('#lastname').val('');
    $('#email').val('');
    $('#phonenumber').val('');
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
