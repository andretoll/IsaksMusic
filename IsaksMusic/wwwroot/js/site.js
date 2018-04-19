$(document).ready(function () {
    //loop();
    $(window).scroll(windowScroll);
})

/* Function to loop through animation */
function loop() {
    $('#scrollIndicator').animate({ 'top': '75%' }, {
        duration: 500,
        complete: function () {
            $('#scrollIndicator').animate({ 'top': '80%' }, {
                duration: 500,
                complete: loop
            });
        }
    });
}

/* Function to handle scroll events */
function windowScroll() {

    if ($("#mainNav").offset().top > 100) {

        if ($(window).width() > 768) {
            $("#mainNav").addClass("navbar-shrink");

        }
        $('#backToTopBtn').show();
        $('#scrollIndicator').hide(500);

    } else {

        if ($(window).width() > 768) {
            $("#mainNav").removeClass("navbar-shrink");

        }
        $('#backToTopBtn').hide();
        $('#scrollIndicator').show(500);
    }
};

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
            scrollTop: $('#contactForm').offset().top -100
        }, 2000)
    })
}

/* Function to scroll to top of page */
function scrollToTop() {

    /* For Safari */
    document.body.scrollTop = 0;

    $("html, body").animate({ scrollTop: 0 }, 1000);
}