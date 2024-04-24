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

//script for cross and list of selected files
let allFiles = [];
let input = document.getElementById("choose-file");
var button = document.getElementsByClassName("toggle-button")[0];

let div = document.getElementById("fileNames");

input.addEventListener("change", function () {
    let files = input.files;


    for (let i = 0; i < files.length; i++) {
        allFiles.push(files[i]);
    }

    console.log(allFiles)
    button.style = "display:block"
    showNames(files);
});

function showNames(files) {
    for (let i = 0; i < files.length; i++) {
        let icon = document.createElement('i');
        icon.classList = 'bi bi-x-lg fileIcon';


        let span = document.createElement('span');
        span.className = "showFile";
        span.style.color = 'black';
        span.textContent = files[i].name;

        let spanIcon = icon.cloneNode(true); // Clone the icon for each file
        span.appendChild(spanIcon);
        div.appendChild(span);

        spanIcon.onclick = function () {
            let index = allFiles.indexOf(files[i]);
            allFiles.splice(index, 1);
            div.removeChild(span);

            if (allFiles.length == 0) {
                button.style = "display:none";
            }
        };
    }
}

function removeAll() {
    allFiles = [];
    div.innerHTML = "";
    button.style = "display:none";
}

function submitFile(id) {

    console.log("submit files calls");
    console.log(id);
    const formData = new FormData();

    for (let i = 0; i < allFiles.length; i++) {
        formData.append('files', allFiles[i]);
    }

    console.log(allFiles);
    console.log(formData);
    $.ajax({
        type: 'POST',
        url: "/Admin/Upload/" + id,
        data: (formData),
        processData: false,
        contentType: false,

        success: function (response) {
            console.log(response)
            removeAll();
            window.location.reload();
            toastr.success("files uploaded successfully");
        },
        error: function (xhr, status, error) {
            console.error("Error:", status, error);
            toastr.error('error loading reasons');
        }
    });
}

//script for download selected files
function downloadSelectedFiles() {

    var selectedFiles = document.querySelectorAll('input[name="checkdoc"]:checked');
    var fileUrls = [];


    selectedFiles.forEach(function (checkbox) {
        var row = checkbox.closest('tr');
        var fileUrl = row.querySelector('a').getAttribute('href');
        fileUrls.push(fileUrl);
    });

    fileUrls.forEach(function (url) {
        var link = document.createElement('a');
        link.href = url;
        link.download = '';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    });
}

//script for checking all the files
document.addEventListener('DOMContentLoaded', function () {
    const selectAllCheckbox = document.querySelector('thead input[type="checkbox"]');
    const rowCheckboxes = document.querySelectorAll('tbody input[type="checkbox"]');

    // Add event listener to the "Select All" checkbox
    selectAllCheckbox.addEventListener('change', function () {
        const isChecked = this.checked;
        // Set the checked property of all row checkboxes to match the state of the "Select All" checkbox
        rowCheckboxes.forEach(function (checkbox) {
            checkbox.checked = isChecked;
        });
    });
});

function deleteSelectedFiles() {
    var selectedFiles = document.querySelectorAll('input[name="checkdoc"]:checked');
    var fileIds = [];

    console.log(selectedFiles);

    selectedFiles.forEach(function (checkbox) {
        let docId = checkbox.getAttribute('data-docId');
        console.log(docId);
        fileIds.push(docId);
    });

    console.log(selectedFiles);

    if (fileIds.length > 0) {

        $.ajax({
            url: '/Admin/DeleteSelectedFiles',
            type: 'POST',
            data: { fileIds: fileIds },
            success: function (response) {
                location.reload();
                toastr.success("file deleted successfully");
            },
            error: function (xhr, status, error) {
                console.error('Error deleting files:', error);
                toastr.error('Error loading reasons');
            }
        });
    } else {
        console.log('No files selected to delete');
    }
}

function sendemail(id, mail) {

    var selectedFiles = document.querySelectorAll('input[name="checkdoc"]:checked');
    var fileIds = [];

    console.log(selectedFiles);

    selectedFiles.forEach(function (checkbox) {
        let docId = checkbox.getAttribute('data-docId');
        console.log(docId);
        fileIds.push(docId);
    });
    console.log(fileIds);
    console.log(id);
    console.log(mail);
    if (fileIds.length > 0) {
        $.ajax({
            type: "POST",
            url: '/Admin/SendAttachment',
            data: { request_id: id, files_jx: fileIds, mail: mail },
            async: false,
            success: function () {
                console.log("Email sent successfully");
                toastr.success("Mail sent successfully");
            },
            error: function (xhr, status, error) {
                toastr.error("Error Loading Reasons");
            }
        });
    } else {
        toastr.error("Select the files");
    }
}