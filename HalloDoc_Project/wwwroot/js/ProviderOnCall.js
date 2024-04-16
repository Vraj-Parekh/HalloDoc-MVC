
function loadFilteredData() {
    var regionId = $('#region').val();
    console.log(regionId);
    $.ajax({
        url: '/Admin/MdOnCallDiv',
        type: 'GET',
        data: {
            regionId: regionId
        },
        success: function (data) {
            $('#MDsDiv').html(data);
            console.log("success");
        },
        error: function (xhr, status, error) {
            $('#MDsDiv').html("<h3 class='text-center'>No data found</h3>");
            console.log("error");
        }
    });
}

$('#region').on('change',function () {
    loadFilteredData();
});


// Initial loading of table data
loadFilteredData();