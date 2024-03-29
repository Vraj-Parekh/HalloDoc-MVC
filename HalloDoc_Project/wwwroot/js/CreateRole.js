//-----for fetching menu-----
function fetchMenus(selectedType) {
    $.ajax({
        url: '/Admin/FetchMenus',
        method: 'POST',
        data: { accountType: selectedType },
        success: function (response) {
            var checkboxContainer = $('#checkboxContainer');
            checkboxContainer.empty();
            //console.log(response);
            response.forEach(function (menu) {
                var checkbox = $('<input class="form-check-input" type="checkbox">')
                    .attr('id', menu.menuid)
                    .attr('value', menu.menuid)
                    .attr('class', 'me-1 menuCheckbox');

                var label = $('<label class="form-check-label"></label >')
                    .attr('for', menu.menuid)
                    .text(menu.name);

                var checkboxSpan = $('<span>');
                //checkboxSpan.addClass('col').css('white-space', 'nowrap');
                checkboxSpan.append(checkbox);
                checkboxSpan.append(label);

                checkboxContainer.append(checkboxSpan);
                console.log("success");
            })
        },
        error: function (xhr, status, error) {
            console.error("error");
        }
    });
}

$(document).ready(function () {
    var selectedType = $('#accountType').val();
    console.log(selectedType);

    fetchMenus(selectedType);

    $('#accountType').change(function () {
        fetchMenus($(this).val());
    });



    $('#submit-btn').click(function () {

        if (!$('#roleName').val()) {
            $('#roleNameError').text('Role Name is required');
        } else {
            $('#roleNameError').text('');
        }

        var checkedIds = [];
        $('.menuCheckbox:checked').each(function () {
            var checkboxId = $(this).attr('id');
            checkedIds.push(checkboxId);
        });
        console.log(checkedIds);

        let model = {};
        model.RoleName = $('#roleName').val();
        model.Menus = checkedIds;
        model.AccountType = $('#accountType').val();
        console.log(model);

        $.ajax({
            url: '/Admin/CreateRole',
            method: 'POST',
            data: model,
            success: function (response) {
                console.log("success");
                window.location.href = '/Admin/AccountAccess';
            },
            error: function (xhr, status, error) {
                console.error("error");
            }
        });
    })

})