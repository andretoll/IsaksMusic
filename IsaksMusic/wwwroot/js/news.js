var noMoreData = false;
var blockSize = 3;
var skipEntries = blockSize;
var inProgress = false;

$(document).ready(function () {

    $('#newsScroller').on('scroll', function () {
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight && !inProgress) {
            loadNews();
        }
    });

    paragraphBreaks();
    checkTextOverflow();
});

/* Function to load more news according to block size */
function loadNews() {

    if (!noMoreData) {

        $('#newsLoadingInner').css('visibility', 'visible');
        $('#newsLoading').show();

        $.ajax({
            type: "Get",
            url: "/news/index?handler=NewsBlock",
            data: { skip: skipEntries },
            beforeSend: function () {
                inProgress = true;
            },
            success: function (result) {
                $('#newsBlockContainer').append(result);
                /* Refresh Like/Share buttons */
                FB.XFBML.parse();
                paragraphBreaks();
                checkTextOverflow();
                skipEntries += blockSize;
                $('#newsLoadingInner').css('visibility', 'hidden');
                $('#newsLoading').hide();
                inProgress = false;
            },
            error: function () {
                noMoreData = true;
            }
        });
    }
}

/* Function to convert string to paragraph html with breaks */
function paragraphBreaks() {

    var containers = $('.news-entry-content-front');

    /* For each paragraph */
    $(containers).each(function () {
        var bodyP = $(this).children('.news-body-text');

        /* Insert line breaks if needed */
        if ($(bodyP).text().indexOf("<br>") >= 0 ) {
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