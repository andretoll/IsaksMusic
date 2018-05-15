$(document).ready(function () {
    $(window).scroll(windowScroll);

    $('#adminSidemenuCollapse').click(rotateIcon);    

    $('[data-toggle="popover"]').popover();

    $('[data-toggle="tooltip"]').tooltip();

    /* Break paragraphs */
    paragraphBreaksFront();

    checkTextOverflow();  

    windowScroll();
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

/* Function to convert string to paragraph html with breaks */
function paragraphBreaksFront() {

    var containers = $('.news-entry-content-front');

    /* For each paragraph */
    $(containers).each(function () {
        var bodyP = $(this).children('.news-body-text');

        /* Insert line breaks */
        $(bodyP).html($(bodyP).text());
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

/* Function to load more news according to block size */
function loadNews() {

    if (!noMoreData) {

        $('#newsLoadingInner').css('visibility', 'visible');

        $.ajax({
            type: "Get",
            url: "/news/index?handler=NewsBlock",
            data: { skip: skipEntries },
            success: function (data) {
                noMoreData = data.noMoreData;
                appendNews(data);
                skipEntries += blockSize;
                $('#newsLoadingInner').css('visibility', 'hidden');

            },
            error: function () {

            }
        });
    }    
}

/* Function to append news entries */
function appendNews(data) {
    
    for (var i = 0; i < data.newsEntries.length; i++) {

        /* Append news entry */
        $('#newsBlockContainer').append(""
            + "<div class='news-entry-container news-entry-container-front'>"
            + "<div class='row'>"
            + "<div class='col-md-3 text-center'>"
            + "<img class='img-thumbnail' src=" + data.newsEntries[i].imageUrl + " alt='News Image' title='@Model.Headline' />"
            + "<span class='text-muted ml-auto'>" + data.newsEntries[i].publishDate + "</span>"
            + "</div>"
            + "<div class='col-md-9'>"
            + "<div class='news-entry-inner'>"
            + "<div class='news-entry-header'>"
            + "<h4 class='text-yellow'>" + data.newsEntries[i].headline + "</h4>"
            + "</div>"
            + "<hr />"
            + "<div class='news-entry-content-front collapse' id='newsEntryContent_" + data.newsEntries[i].id + "'>"
            + "<p class='text-muted font-italic'>" + data.newsEntries[i].lead + "</p>"
            + "<p class='news-body-text'>" + data.newsEntries[i].body + "</p>"
            + "</div>"
            + "<a class='collapse-news-btn collapsed' id='collapseNewsBtn_" + data.newsEntries[i].id + "' data-toggle='collapse' href='#newsEntryContent_" + data.newsEntries[i].id + "' aria-expanded='false' aria-controls='collapseDescription'></a>"
            + "</div>"
            + "</div>"
            + "</div>"
            + "</div>"
        );

        /* Append link, if any */
        if (data.newsEntries[i].linkTitle !== null) {
            $('#newsEntryContent_' + data.newsEntries[i].id).append("<a title='Opens new tab' class='d-inline-block text-yellow mt-3' href='" + data.newsEntries[i].linkUrl + "' target='_blank'>" + data.newsEntries[i].linkTitle + "</a>");
        }

        /* Check text height */
        checkTextOverflow();
    }
}