$(document).ready(function () {
    $.ajax({
        url: '/Admin/FetchRegions',
        method: 'GET',
        success: function (response) {
                var dropdown = $('#stateDropdown');
                response.forEach(function (res) {
                    dropdown.append($('<option></option>').attr('value', res.regionid).text(res.name));
                });
            }
        },
        error: function (response) {
            console.log('error');
        }
    });
});
