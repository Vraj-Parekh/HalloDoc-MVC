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
//-------------for open modal-------------
function SendMessageModal(id)
{
    const myModal = new bootstrap.Modal("#message", {});
    myModal.show();
    $("#SendNotification").attr("onclick", "SendMessage(" + id + ")");
}

function SendMessage(id)
{
    var mode = $('input[name="notification"]:checked').val();
    var msg = $('#msg').val();
    console.log(mode);
    console.log(msg);

    var data = {
        mode: mode,
        message: msg,
        physicianId: id
    };

    $.ajax({
        url: '/Admin/SendMessage',
        type: 'POST',
        data: data,
        success: function (response) {
            toastr.success("Notification sent successfully");
        },
        error: function (xhr, status, error) {
            toastr.error("Error Loading Reasons");
        }
    });
}

