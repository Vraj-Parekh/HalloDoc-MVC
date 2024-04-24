document.getElementById("toggle_theme_btn").addEventListener("click", function () {
    var icon = this.querySelector('i');

    if (icon.classList.contains('bi-moon')) {
        icon.classList.remove('bi-moon');
        icon.classList.add('bi-sun');
    }
    else {
        icon.classList.remove('bi-sun');
        icon.classList.add('bi-moon');
    }
})