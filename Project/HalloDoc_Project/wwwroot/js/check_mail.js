function checkEmail() {
    var element = document.getElementById("email");
    var email = element.value;
    console.log(email);
    $.ajax({
        type: 'GET',
        url: "/Request/IsPatientPresent/" + email,
        contentType: false,
        processData: false,

        success: function (response) {
            console.log(response);
            if (!response) {
                document.getElementById("passwordDiv").style.display = "flex";
            }
            else {
                document.getElementById("passwordDiv").style.display = "none";
            }
        },
        error: function () {
            console.log("error");
        }
    });
}