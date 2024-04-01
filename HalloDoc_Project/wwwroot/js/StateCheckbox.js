$(document).ready(function () {
    $.ajax({
        url: '/Admin/FetchRegions',
        method: 'GET',
        success: function (response) {
            var checkboxContainer = $('#checkboxContainer');

            response.forEach(function (region) {
                var checkboxId = 'region_' + region.regionId;
                checkboxContainer.append(
                    '<div class="form-check">' +
                    '<input class="form-check-input" type="checkbox" value="' + region.regionId + '" id="' + checkboxId + '">' +
                    '<label class="form-check-label" for="' + checkboxId + '">' + region.name + '</label>' +
                    '</div>'
                );
            });
        },
        error: function (response) {
            console.log('Error fetching regions');
        }
    });
});