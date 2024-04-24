
var requestTypeId = 5;//for all
var status = 1;//new state
var pageIndex = 1;
var pageSize = 5;
var previousTab = null;
var previousFilter = null;

function activateTab(tabId) {
    // Deactivate previous
    if (previousTab !== null) {
        previousTab.classList.remove('active');
    }

    // Activate clicked tab
    let clickedTab = document.getElementById(tabId);
    clickedTab.classList.add('active');
    var spanEle = document.getElementById('spanText');
    spanEle.textContent = "(" + tabId.substring(0, tabId.indexOf("State")) + ")";

    $('#search').val('');

    switch (tabId) {
        case "NewState":
            status = 1;
            pageIndex = 1;
            getData();
            break;
        case "PendingState":
            status = 2;
            pageIndex = 1;
            getData();
            break;
        case "ActiveState":
            status = 3;
            pageIndex = 1;
            getData();
            break;
        case "ConcludeState":
            status = 4;
            pageIndex = 1;
            getData();
            break;
    }


    // Update the previousTab variable
    previousTab = clickedTab;
}

function activateFilter(tabId) {
    // Deactivate previous filter
    if (previousFilter !== null) {
        previousFilter.classList.remove('box');
    }

    // Activate clicked filter
    let clickedfilter = document.getElementById(tabId);
    clickedfilter.classList.add('box');

    switch (tabId) {
        case "AllFilter":
            requestTypeId = 5;
            getData();
            break;
        case "PatientFilter":
            requestTypeId = 2;
            getData();
            break;
        case "FamilyFilter":
            requestTypeId = 3;
            getData();
            break;
        case "ConciergeFilter":
            requestTypeId = 4;
            getData();
            break;
        case "BusinessFilter":
            requestTypeId = 1;
            getData();
            break;
        case "VipFilter":
            requestTypeId = 5;
            getData();
            break;
    }
    // Update previous Filter
    previousFilter = clickedfilter;
}

activateTab('NewState');
activateFilter('AllFilter');

$(document).ready(function () {
    $('#NewState').trigger('click');
    $('#All').trigger('click');
    $('#search').on('input', function () {
        getData();
    });

});


function getData() {

    query = $('#search').val().trim();
    console.log(query);
    console.log(query);


    $.ajax({
        url: '/Provider/Table',
        type: 'get',
        data: { status: status, requestTypeId: requestTypeId, pageIndex: pageIndex, pageSize: pageSize, searchQuery: query },
        success: function (result) {
            if (result.includes('table')) {
                $(".tableData").html(result);
            }
            else {
                window.location.reload();
            }
        }
    });
}


function previousPage() {
    if (pageIndex > 1) {
        pageIndex--;
        getData();
    }
    else {
        console.log('else')
        $('#prevBtn').prop('disabled', true);
    }
}
function changePage(page) {
    pageIndex = page;
    getData();
}
function nextPage(totalPages) {
    console.log(totalPages)
    if (pageIndex < totalPages) {
        pageIndex++;
        getData();
    }
}

function checkFinalize(requestId) {
    console.log(requestId);
    $.ajax({
        url: '/Provider/CheckIsFinalize',
        type: 'POST',
        data: {
            requestId: requestId
        },
        success: function (response) {

            if (response === true) {
                //popup
                const myModal = new bootstrap.Modal("#finalize", {});
                myModal.show();

                $('#downloadId').attr('onclick', 'DownloadEncounter(' + requestId + ')');

            } else {
                console.log("Request is not finalized");
                window.location.href = '/Provider/Encounter/' + requestId;
            }
        },
        error: function (xhr, status, error) {
            console.error(xhr.status + ': ' + xhr.statusText);
            // Handle other errors here
        }
    });
}


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

function consult(requestId) {
    console.log(requestId);
    $.ajax({
        url: '/Provider/Consult/' + requestId,
        type: 'get',
        success: function (response) {
            toastr.success("Request Moved to conclude state");
            window.location.reload();
        },
        error: function (xhr, status, error) {
            toastr.error("Error Loading Reasons");
        }
    });
}