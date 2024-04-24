
$('#accountType').change(function () {
    var selectedValue = $(this).val();

    if (selectedValue === "0") {
        // If "All" --> hide the button
        $('#create-acc-btn').hide();
    } else {
        // If "Admin" or "Provider" is selected, show the button and update its text
        $('#create-acc-btn').show();
        if (selectedValue === "1") {
            // If "Admin" is selected, set the button text to "Create Admin Account"
            $('#create-acc-btn').text('Create Admin Account');
            $('#create-acc-btn').off('click').on('click', function () {
                window.location.href = '/Admin/CreateAdminAccount';
            });
        } else if (selectedValue === "2") {
            // If "Provider" is selected, set the button text to "Create Provider Account"
            $('#create-acc-btn').text('Create Provider Account');
            $('#create-acc-btn').off('click').on('click', function () {
                window.location.href = '/Admin/CreateProviderAccount';
            });
        }
    }
});

$('#accountType').trigger('change');

var page = 1;
var itemsPerPage = 5;

function loadFilteredData(page) {
    var accountType = $('#accountType').val();

    console.log(accountType);

    $.ajax({
        url: '/Admin/UserAccessTable',
        type: 'GET',
        data: {
            accountType: accountType,
            page: page,
            itemsPerPage: itemsPerPage
        },
        success: function (data) {
            $('#userAccessTableDiv').html(data);
            console.log("success");
        },
        error: function (xhr, status, error) {
            console.log("error");
        }
    });
}

$("#accountType").on('change', function () {
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