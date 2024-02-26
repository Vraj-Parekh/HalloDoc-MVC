//for mobile field

const phoneInputField = document.querySelector("#phone");
const phoneInputField1 = document.querySelector("#phone1");

const phoneInput = window.intlTelInput(phoneInputField, {

    autoInsertDialCode: true,
    nationalMode: false,
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});
const phoneInput1 = window.intlTelInput(phoneInputField1, {

    autoInsertDialCode: true,
    nationalMode: false,
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});

