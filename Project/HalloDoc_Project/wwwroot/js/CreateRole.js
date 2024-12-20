﻿//-----for fetching menu-----
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

        var checkedMenus = [];
        $('.menuCheckbox:checked').each(function () {
            var checkboxId = $(this).attr('id');
            var menuName = $(this).next('label').text();
            var menuObject = {
                MenuId: checkboxId,
                Name: menuName,
                IsPresent: true
            };

            checkedMenus.push(menuObject);
        });
        console.log(checkedMenus);

        let model = {};
        model.RoleName = $('#roleName').val();
        model.Menus = checkedMenus;
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
                console.log(xhr.status);
                if (xhr.status == 400) {
                    // HTTP status code 400 indicates a bad request
                    var errorResponse = JSON.parse(xhr.responseText);
                    // Display the error message in the UI
                    $('#roleNameError').text(errorResponse.message);
                    console.log(errorResponse.message);
                } else {
                    // Handle other types of errors
                    // Display a generic error message or take appropriate action
                    $('#roleNameError').text('An unexpected error occurred.');
                }
            }
        });
    })

})