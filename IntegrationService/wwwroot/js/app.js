(function () {

    /* ========= sidebar toggle ======== */
    const sidebarNavWrapper = document.querySelector(".sidebar-nav-wrapper");
    const mainWrapper = document.querySelector(".main-wrapper");
    const menuToggleButton = document.querySelector("#menu-toggle");
    const menuToggleButtonIcon = document.querySelector("#menu-toggle i");
    const overlay = document.querySelector(".overlay");

    menuToggleButton.addEventListener("click", () => {
        sidebarNavWrapper.classList.toggle("active");
        overlay.classList.add("active");
        mainWrapper.classList.toggle("active");

        if (document.body.clientWidth > 1200) {
            if (menuToggleButtonIcon.classList.contains("icon-angle-left")) {
                menuToggleButtonIcon.classList.remove("icon-angle-left");
                menuToggleButtonIcon.classList.add("icon-menu");
            } else {
                menuToggleButtonIcon.classList.remove("icon-menu");
                menuToggleButtonIcon.classList.add("icon-angle-left");
            }
        } else {
            if (menuToggleButtonIcon.classList.contains("icon-angle-left")) {
                menuToggleButtonIcon.classList.remove("icon-angle-left");
                menuToggleButtonIcon.classList.add("icon-menu");
            }
        }
    });
    overlay.addEventListener("click", () => {
        sidebarNavWrapper.classList.remove("active");
        overlay.classList.remove("active");
        mainWrapper.classList.remove("active");
    });




})();

$(document).ready(function () {
    $.datepicker.setDefaults($.datepicker.regional["el"]);
})