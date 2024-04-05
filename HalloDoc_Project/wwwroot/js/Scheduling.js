$(document).ready(function () {
    // Handle click event on the Add New Shift button
    console.log("clalled bhai");
    $('#openShiftModalBtn').click(function () {
        // Trigger the display of the modal
        $('#createShiftModal').modal('show');
        console.log("ander hu me");
    });
});