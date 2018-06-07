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
                //FB.XFBML.parse();
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