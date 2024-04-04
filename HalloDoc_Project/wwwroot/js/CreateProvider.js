function photo_validation() {
    var val = $('#fileLoader').val().toLowerCase(),
        regex = new RegExp("(.*?)\.(jpg|png)$");
    if (val == null || val == "") {
        return true;
    }
    if (!(regex.test(val))) {
        $('#photo_validation').html('Please select only jpg or png type photo');
        $('#photo_img').attr("src", "");
        return false;
    }
    $('#photo_validation').html('');
    var file = document.getElementById('fileLoader').files[0];
    if (file) {
        $('#photo_img').attr("src", URL.createObjectURL(file))
    }
    return true;
};

function selectcheckbox(id, btnId) {
    var checkbox = document.getElementById(id);
    var viewBtn = document.getElementById(btnId);
    checkbox.checked = true;
    checkbox.disabled = true;
    viewBtn.classList.remove('d-none');
}

function viewDocument(id) {
    var fileInput = document.getElementById(id);
    var fileUrl = URL.createObjectURL(fileInput.files[0]);

    // Open the document in a new tab
    window.open(fileUrl, '_blank');
}