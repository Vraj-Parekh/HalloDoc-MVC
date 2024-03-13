
var today = new Date().toISOString().split('T')[0];

document.getElementById("dob").setAttribute("max", today);
document.getElementById("dateofservice").setAttribute("max", today);
