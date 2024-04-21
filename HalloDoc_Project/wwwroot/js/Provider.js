toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "positionClass": "toast-bottom-right",
    "preventDuplicates": true,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}
//-------------for open modal-------------
function SendMessageModal(id)
{
    const myModal = new bootstrap.Modal("#message", {});
    myModal.show();
    $("#SendNotification").attr("onclick", "SendMessage(" + id + ")");
}

function SendMessage(id)
{
    var mode = $('input[name="notification"]:checked').val();
    var msg = $('#msg').val();
    console.log(mode);
    console.log(msg);

    var data = {
        mode: mode,
        message: msg,
        physicianId: id
    };

    $.ajax({
        url: '/Admin/SendMessage',
        type: 'POST',
        data: data,
        success: function (response) {
            toastr.success("Notification sent successfully");
        },
        error: function (xhr, status, error) {
            toastr.error("Error Loading Reasons");
        }
    });
}

var page = 1;
var itemsPerPage = 5;

function loadFilteredData(page) {
    var region = $('.regionDropDown').val();

    console.log(region);

    $.ajax({
        url: '/Admin/ProvidersTable',
        type: 'GET',
        data: {
            regionId: region,
            page: page,
            itemsPerPage: itemsPerPage
        },
        success: function (data) {
            $('#providersTableDiv').html(data);
            console.log("success");
        },
        error: function (xhr, status, error) {
            console.log("error");
        }
    });
}

$(".regionDropDown").on('change', function () {
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

