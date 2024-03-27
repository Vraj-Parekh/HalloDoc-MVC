//-----for fetching menu-----
$('#accountType').bind("load change", function () {

    var selectedType = $('#accountType').val();
    console.log(selectedType);

    $.ajax({
        url: '/Admin/FetchMenus',
        method: 'POST',
        data: { accountType: selectedType },
        success: function (response) {
            var checkboxContainer = $('#checkboxContainer');
            checkboxContainer.empty();
            console.log(response);
            response.forEach(function (menu) {
                var checkbox = $('<input class="form-check-input" type="checkbox">')
                    .attr('id', menu.menuid)
                    .attr('name', 'menuCheckbox')
                    .attr('value', menu.menuid);

                var label = $('<label class="form-check-label"></label >')
                    .attr('for', menu.menuid)
                    .text(menu.name);

                checkboxContainer.append(checkbox);
                checkboxContainer.append(label);
                checkboxContainer.append('<br>');
                console.log("success");
            })
        },
        error: function (xhr, status, error) {
            console.error("error");
        }
    });
})