
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
    $('.regionSearch').val(0);

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
        case "ToCloseState":
            status = 5;
            pageIndex = 1;
            getData();
            break;
        case "UnpaidState":
            status = 6;
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

    $.ajax({
        url: './FetchRegions',
        method: 'GET',
        success: function (response) {
            response.forEach(function (res) {
                $('.regionSearch').append("<option value='" + res.regionid + "'>" + res.name + "</option>");
            });
        },
        error: function (response) {
            console.log('error');
        }
    });

    $('.regionSearch').on('change', function () {
        getData();
    });
});


function getData() {

    query = $('#search').val().trim();
    console.log(query);

    regionId = $('.regionSearch').val();
    console.log(regionId);

    $.ajax({
        url: '/Admin/Table',
        type: 'get',
        data: { status: status, requestTypeId: requestTypeId, pageIndex: pageIndex, pageSize: pageSize, searchQuery: query, regionId: regionId },
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

//export filtered
$(document).ready(function () {
    $('#exportFiltered').click(function () {
        console.log('after this ajax will call')
        $.ajax({
            type: 'POST',
            url: '/Admin/ExportFiltered',
            data: { status: status, requestTypeId: requestTypeId, pageIndex: pageIndex, pageSize: pageSize, searchQuery: query, regionId: regionId },
            xhrFields: {
                responseType: 'blob' // Set the response type to blob
            },
            success: function (response) {
                var blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = 'patient_list.xlsx';
                link.click();
            },
            error: function (xhr, status, error) {
                console.error("Error:", status, error);
            }
        });
    });
});

//export all
$(document).ready(function () {
    $('#exportAll').click(function () {
        console.log('after this export all ajax will call')
        $.ajax({
            type: 'POST',
            url: '/Admin/ExportAll',
            data: { status: status },
            xhrFields: {
                responseType: 'blob' // Set the response type to blob
            },
            success: function (response) {
                var blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = 'all_patient_list.xlsx';
                link.click();
            },
            error: function (xhr, status, error) {
                console.error("Error:", status, error);
            }
        });
    });
});


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