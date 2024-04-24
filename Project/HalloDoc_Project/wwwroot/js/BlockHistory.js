var page = 1;
var itemsPerPage = 10;

function loadFilteredData(page) {
    var name = $('#name').val();
    var createdDate = $('#createdDate').val();
    var email = $('#email').val();
    var phonenumber = $('#phonenumber').val();

    $.ajax({
        url: '/Admin/BlockHistoryTable',
        type: 'GET',
        data: {
            name: name,
            createdDate: createdDate,
            email: email,
            phonenumber: phonenumber,
            page: page,
            itemsPerPage: itemsPerPage
        },
        success: function (data) {
            $('#blockHistroyTableDiv').html(data);
            console.log("success");
        },
        error: function (xhr, status, error) {
            $('#blockHistroyTableDiv').html("<h3 class='text-center'>No data found</h3>");
            console.log("error");
        }
    });
}

$('#searchButton').click(function () {
    page = 1;
    loadFilteredData(page);
});

$('#clearButton').click(function () {
    $('#name').val('');
    $('#createdDate').val('');
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