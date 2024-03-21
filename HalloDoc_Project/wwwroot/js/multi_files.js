

//for uploading multiple files
// document.addEventListener("DOMContentLoaded", function () {
//     const fileInput = document.getElementById('upload');
//     const fileList = document.getElementById('filename1');

//     fileInput.addEventListener('change', function (event) {
//         const files = event.target.files;
//         fileList.innerHTML = '';

//         for (let i = 0; i < files.length; i++) {
//             const file = files[i];
//             const fileItem = document.createElement('div');
//             fileItem.className = 'file-item';

//             const fileName = document.createTextNode(file.name);
//             fileItem.appendChild(fileName);

//             const removeIcon = document.createElement('i');
//             removeIcon.classList = 'bi bi-x-square';
//             removeIcon.alt = 'Remove';
//             fileItem.appendChild(removeIcon);

//             fileList.appendChild(fileItem);
//             removeIcon.addEventListener('click', function () {
//                 fileList.removeChild(fileItem);
//                 fileItem.removeChild(fileItem);
//             });
//         }
//     });
// });


//let allFiles = [];
//let input = document.getElementById("upload");
//var button = document.getElementsByClassName("toggle-button")[0];

//let div = document.getElementById("fileNames");

//input.addEventListener("change", function () {
//    let files = input.files;


//    for (let i = 0; i < files.length; i++) {
//        allFiles.push(files[i]);
//    }

//    console.log(allFiles)
//    button.style = "display:block"
//    showNames(files);
//});

//function showNames(files) {

//    for (let i = 0; i < files.length; i++) {

//        let icon = document.createElement('i');
//        icon.classList = 'bi bi-x-lg fileIcon';

//        let br = document.createElement('br');

//        let span = document.createElement('span');
//        span.className = "showFile"
//        span.style.color = 'black';
//        span.textContent = files[i].name;
//        span.appendChild(icon);
//        div.appendChild(span);


//        div.appendChild(br);


//        icon.onclick = function () {
//            let index = allFiles.indexOf(files[i]);
//            allFiles.splice(index, 1);
//            console.log(allFiles);
//            div.removeChild(span);
//            div.removeChild(br);

//            if (allFiles.length == 0) {
//                button.style = "display:none";
//            }
//        }
//    }
//}

//function removeAll() {
//    allFiles = [];
//    div.innerHTML = "";
//    button.style = "display:none";
//}

//function submitFile(id) {

//    console.log(allFiles);
//    console.log("ADFA");

//    const formData = new FormData();

//    for (let i = 0; i < allFiles.length; i++) {
//        formData.append('files', allFiles[i]);
//    }


//    $.ajax({
//        type: 'POST',
//        url: "/ViewDocument/Upload/" + id,
//        data: (formData),
//        processData: false,
//        contentType: false,

//        success: function (response) {
//            console.log(response)
//            removeAll();
//            window.location.reload();
//        },
//        error: function (xhr, status, error) {
//            console.error("Error:", status, error);
//        }
//    });
//}