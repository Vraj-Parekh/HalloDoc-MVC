var page = 1;
var itemsPerPage = 10;
var isDateFilter = false;
function loadFilteredData(page) {
    var regionId = $('#region').val();

    $.ajax({
        url: '/Admin/RequestedShiftTable',
        type: 'GET',
        data: {
            regionId: regionId,
            isDateFilter: isDateFilter,
            page: page,
            itemsPerPage: itemsPerPage
        },
        success: function (data) {
            $('#requestedShiftTableDiv').html(data);
            console.log("success");
        },
        error: function (xhr, status, error) {
            $('#requestedShiftTableDiv').html("<h3 class='text-center'>No data found</h3>");
            console.log("error");
        }
    });
}

$('#region').on('change', function () {
    page = 1;
    loadFilteredData(page);
});

$("#monthshifts").click(function () {
    isDateFilter = true;

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