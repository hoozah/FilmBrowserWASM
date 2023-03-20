function clearAllCheckbox() {
    var checkboxes = document.querySelectorAll('input[type=checkbox]');

    for (var i = 0; i < checkboxes.length; i++) {
        checkboxes[i].checked = false;
    }
}

function scrollToTop() {
    document.getElementById('main-container').scrollIntoView({ block: 'start', behavior: 'smooth' });
}