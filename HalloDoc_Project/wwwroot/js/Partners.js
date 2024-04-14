var page = 1;
var itemsPerPage = 10;

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

function loadFilteredData(page) {
    var searchVendor = $("#searchVendor").val().trim();
    console.log(searchVendor);

    var professionType = $("#proffessionType").val();
    console.log(professionType);

    $.ajax({
        url: '/Provider/VendorsTable',
        type: 'GET',
        data: {
            searchVendor: searchVendor,
            professionType: professionType,
            page: page,
            itemsPerPage: itemsPerPage
        },
        success: function (data) {
            $('#vendorTableDiv').html(data);
            console.log("success");
        },
        error: function (xhr, status, error) {
            console.log("error");
        }
    });
}

$("#proffessionType").on('change', function () {
    page = 1;
    loadFilteredData(page);
});
$("#searchVendor").on('input', function () {
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
