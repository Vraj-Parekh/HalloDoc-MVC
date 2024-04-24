var page = 1;
var itemsPerPage = 5;

function loadFilteredData(page) {
    var search = $('#search').val();

    $.ajax({
        url: '/Home/UserDataTable',
        type: 'GET',
        data: {
            search: search,
            page: page,
            itemsPerPage: itemsPerPage
        },
        success: function (data) {
            $('#tableDiv').html(data);
            console.log("success");
        },
        error: function (xhr, status, error) {
            console.log("error");
        }
    });
}


$('#search').on('input', function () {
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

function openAddUserModal() {
    const myModal = new bootstrap.Modal("#addUserModal", {});
    myModal.show();
}

function openEditUserModal(userId) {
    console.log(userId);

    $.ajax({
        url: '/Home/GetUserDetails',
        type: 'GET',
        data: { userId: userId },
        success: function (data) {
            // Populate the modal with the retrieved user data
            console.log(data);
            $('#firstname').val(data.firstname);
            $('#lastname').val(data.lastname);
            $('#email').val(data.email);
            $('#dob').val(data.dateOfBirth);
            $('input[name=gender][value=' + data.gender + ']').prop('checked', true);
            $('#phone').val(data.phoneNumber);
            $('#country').val(data.country);
            $('#city').val(data.city);

            const myModal = new bootstrap.Modal("#editUserModal", {});
            myModal.show();
        },
        error: function (xhr, status, error) {

            console.error(xhr.responseText);
        }
    });

}