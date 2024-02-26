// Get today's date
var today = new Date().toISOString().split('T')[0];

// Set the max attribute of the input element to today's date
document.getElementById("dob").setAttribute("max", today);
