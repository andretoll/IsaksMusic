$(document).ready(function () {
    $(window).scroll(windowScroll);    

    $('[data-toggle="popover"]').popover();

    $('[data-toggle="tooltip"]').tooltip();

    $('input[type=checkbox]').iCheck({
        checkboxClass: 'icheckbox_square-red',
        radioClass: 'iradio_square-red',
        increaseArea: '20%'
    });

    windowScroll();

    paragraphBreaks();
    checkTextOverflow();
});

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

/* Function to convert string to paragraph html with breaks */
function paragraphBreaks() {

    var containers = $('.news-entry-content-front');

    /* For each paragraph */
    $(containers).each(function () {
        var bodyP = $(this).children('.news-body-text');

        /* Insert line breaks if needed */
        if ($(bodyP).text().indexOf("<br>") >= 0) {
            $(bodyP).html($(bodyP).text());
        }
    });
}

/* Function to determine text overflow */
function checkTextOverflow() {
    /* Check news text overflow */
    var element = $('.news-entry-content-front');
    $(element).each(function () {

        if (this.offsetHeight < this.scrollHeight || this.offsetWidth < this.scrollWidth) {
            var btn = $(this).parent().children('.collapse-news-btn');
            $(btn).show();
        }
    });
}