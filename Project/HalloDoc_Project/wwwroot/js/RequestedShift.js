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

$(document).ready(function () {
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
    };

    // Check or uncheck all checkboxes when the headCheck checkbox is clicked
    $('thead input[name="headCheck"]').change(function () {
        const isChecked = $(this).prop('checked');
        $('tbody input[type="checkbox"]').prop('checked', isChecked);
    });

    $('#approveSelected').click(function () {
        var selectedIds = [];
        $('input[name="checkedshift"]:checked').each(function () {
            selectedIds.push($(this).val());
        });

        console.log(selectedIds);

        $.ajax({
            url: '/Admin/ApproveSelected',
            type: 'post',
            async: false,
            data: {
                Ids: selectedIds,
            },
            success: function (data) {
                console.log("success");
                window.location.reload();
                toastr.success('Shifts approve successfully');
            },
            error: function (xhr, status, error) {
                console.log("error");
                toastr.error('error loading reasons');
            }
        });
    });

    $('#deleteSelected').click(function () {
        var selectedIds = [];
        $('input[name="checkedshift"]:checked').each(function () {
            selectedIds.push($(this).val());
        });

        console.log(selectedIds);

        $.ajax({
            url: '/Admin/DeleteSelectedShift',
            type: 'post',
            async: false,
            data: {
                Ids: selectedIds,
            },
            success: function (data) {
                console.log("success");
                window.location.reload();
                toastr.success('Shifts deleted successfully');
            },
            error: function (xhr, status, error) {
                console.log("error");
                toastr.error('error loading reasons');
            }
        });
    });

});

