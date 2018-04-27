$(document).ready(function () {
    $(window).scroll(windowScroll);

    $('#adminSidemenuCollapse').click(rotateIcon);

    $(function () {
        $('[data-toggle="popover"]').popover()
    })
});

windowScroll();

/* Function to handle scroll events */
function windowScroll() {

    if ($("#mainNav").offset().top > 100) {

        if ($(window).width() > 768) {
            $("#mainNav").addClass("navbar-shrink");

        }
        $('#backToTopBtn').show();
        $('#scrollIndicator').fadeOut(500);

    } else {

        if ($(window).width() > 768) {
            $("#mainNav").removeClass("navbar-shrink");

        }
        $('#backToTopBtn').hide();
        $('#scrollIndicator').fadeIn(500);
    }
}

/* Function to show snackbar */
function ShowSuccessSnackbar(message) {

    /* Get snackbar */
    var x = document.getElementById("snackbar-success");

    /* Set message */
    x.innerHTML = message;

    /* Show snackbar */
    x.className = "show";

    /* After 3 seconds, hide snackbar */
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
}

/* Function to scroll to contact form */
function scrollToContactForm() {

    $(document).ready(function () {

        $('html, body').animate({
            scrollTop: $('#contactForm').offset().top - 100
        }, 2000);
    });
}

/* Function to scroll to top of page */
function scrollToTop() {

    /* For Safari */
    document.body.scrollTop = 0;

    $("html, body").animate({ scrollTop: 0 }, 1000);
}

/* Function to rotate icon */
function rotateIcon() {

    var icon = $('#adminSidemenuCollapse').children('i');

    if (icon.hasClass("down")) {
        icon.removeClass("down");
    }
    else {
        icon.addClass("down");
    }
}