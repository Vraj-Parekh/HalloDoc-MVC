$(document).ready(function () {
    $.ajax({
        url: '/Admin/FetchRegions',
        method: 'GET',
        success: function (response) {
            var dropdown = $('#stateDropdown'); 
            dropdown.empty(); 
            dropdown.append($('<option></option>').attr('value', '').text('Select State')); 

            response.forEach(function (res) {
                dropdown.append($('<option></option>').attr('value', res.regionid).text(res.name)); 
            });
        },
        error: function (response) {
            console.log('error');
        }
    });
});
