var categoryValue;
var categoryId;
var songOrder;

$(document).ready(function () {

    if ($('a[href="' + location.pathname + '"]').length === 0) {
        var loc = location.pathname.substr(0, location.pathname.lastIndexOf("/"));
        $('a[href="' + loc + '"]').closest('.list-group-item').addClass('active');
    } else {
        $('a.active').removeClass('active');
        $('a[href="' + location.pathname + '"]').closest('.list-group-item').addClass('active');
    }

    /* When collapsing side menu */
    $('#adminSidemenuCollapse').on('click', function () {
        var icon = $('#adminSidemenuCollapse').children('i');

        if (icon.hasClass("down")) {
            icon.removeClass("down");
        }
        else {
            icon.addClass("down");
        }
    });        

    /* When submitting form */
    $('#songForm').submit(function (e) {
        if ($('#songForm').valid()) {
            startLoadingAnimation();
        }
    });

    /* When choosing file to upload */
    $('#songUpload').change(function () {

        if (this.files.length > 0) {

            var file = $("#songUpload")[0].files[0];
            var ext = file.name.split('.').pop();

            if (ext === 'wav' || ext === 'mp3') {
                $(this).addClass('file-valid');
            }
            else {
                $(this).removeClass('file-valid');
            }
        }
        else {
            $(this).removeClass('file-valid');
        }
    });

    /* When double clicking editable list item */
    $('.list-item-editable').click(function () {
        this.contentEditable = true;
        this.focus();

        /* Get category Id */
        categoryId = $(this).parent('li').attr('id').split("_").pop();

        /* Get content */
        categoryValue = $(this).html();
    });

    /* Upon leaving edit area */
    $('.list-item-editable').on('keypress blur', function (e) {

        if (e.keyCode && e.keyCode === 13 || e.type === 'blur') {

            /* Disable editable */
            this.contentEditable = false;

            if ($(this).html().length === 0) {
                $(this).html(categoryValue);
            } else {
                editCategory(categoryId, this);
            }


            return false;
        }
    });

    /* Prevent cut, copy and paste */
    $('.list-item-editable').on("cut copy paste", function (e) {
        e.preventDefault();
    });    

    /* When a checkbox is clicked */
    $('.feature-check').on('ifClicked', function () {

        /* Get Id */
        var id = $(this).closest('tr').attr('id').split('_').pop();

        /* Get row */
        var row = $(this).closest('tr');

        /* If checked */
        if (!$(this).is(':checked')) {

            /* Save */
            featureSong(id, true);

            $(row).addClass('featured-row');
        } else {

            /* Save */
            featureSong(id, false);
        }
    });

    /* When a checkbox is checked */
    $('.feature-check').on('ifChecked', function () {

        /* Uncheck others */
        $(this).removeClass('feature-check');
        $('.feature-check').iCheck('uncheck');
        $(this).addClass('feature-check');
    });

    /* When a checkbox is unchecked */
    $('.feature-check').on('ifUnchecked', function () {

        var row = $(this).closest('tr');
        $(row).removeClass('featured-row');
    });

    /* Initiate row reordering plugin */
    $('#songTable').tableDnD({
        onDragClass: "row-drag",
        onDrop: function (table, row) {
            var orderStr = $.tableDnD.serialize();

            var tmpArr = orderStr.split('&');
            var orderArr = [];
            for (var i = 0; i < tmpArr.length; i++) {
                var paramArr = tmpArr[i].split('=');
                if (paramArr[1] !== null && paramArr[1] !== '') {
                    orderArr.push(paramArr[1].split('_').pop());
                }
            }

            $('#saveOrderBtn').fadeIn(500);

            songOrder = orderArr.join(',');
        }
    });

    /* When user saves song order */
    $('#saveOrderBtn').on('click', function () {
        $.ajax({
            type: "Get",
            url: "/admin/songs?handler=Reorder",
            data: { orderString: songOrder },
            success: function () {
                $('#saveOrderBtn').fadeOut(250);
                ShowSuccessSnackbar("Order saved");
            }
        });
    });

    /* Break paragraphs */
    paragraphBreaks();

    /* Check news text overflow */
    checkTextOverflow();

    /* Check news entry count */
    checkNewsEntryCount($('#adminNewsEntries').children().length);

    /* jQuery UI Datepicker */
    $('#datepicker').datepicker({
        "showAnim": "slideDown",
        showOtherMonths: true,
        selectOtherMonths: true,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'yy-mm-dd',
        maxDate: '+0m',
        onSelect: function (e) {
            getEntriesByDate(e);
        }
    });
});

/* Function to start loading animation */
function startLoadingAnimation() {
    $('#songUploadAnim').removeClass('d-none');

    $('#songForm').find(':submit').hide();
}

/* Function to delete song */
function deleteSong(songId) {

    var element = $('#row_' + songId);

    var result = confirm("Are you sure you want to delete this song?");
    if (result) {
        $.ajax({

            type: "Get",
            url: "/admin/songs?handler=Delete",
            data: { id: songId },
            success: function () {
                ShowSuccessSnackbar("Song removed");
                $(element).fadeOut(500, function () {
                    element.remove();
                });
            }
        });
    }
}

/* Function to delete a category */
function deleteCategory(categoryId) {

    var element = $('#category_' + categoryId);

    var result = confirm("Are you sure you want to delete this category?");
    if (result) {
        $.ajax({

            type: "Get",
            url: "/admin/categories?handler=Delete",
            data: { id: categoryId },
            success: function () {
                ShowSuccessSnackbar("Category removed");
                $(element).fadeOut(500, function () {
                    element.remove();
                });
            },
            error: function () {
                location.reload();
            }
        });
    }
}

/* Function to edit category name */
function editCategory(id, dom) {

    var newName = $(dom).html();

    if (categoryValue !== newName) {
        $.ajax({

            type: "Get",
            url: "/admin/categories?handler=Edit",
            data: { id: id, name: newName },
            success: function () {
                ShowSuccessSnackbar("Category updated");
            }
        });
    }
}

/* Function to feature song */
function featureSong(songId, feature) {

    disableCheckboxes(true);

    $.ajax({

        type: "Get",
        url: "/admin/songs?handler=Feature",
        data: { id: songId, featured: feature },
        success: function () {
            disableCheckboxes(false);
            ShowSuccessSnackbar("Changes saved");
        }
    });
}

/* Function to disable checkboxes while ajax request is open */
function disableCheckboxes(disable) {

    $('.feature-check').each(function () {
        if (disable) {
            $(this).attr('disabled', true);

        } else {
            $(this).attr('disabled', false);
        }
    });
}

/* Function to delete news entry */
function deleteNewsEntry(newsEntryId) {

    var element = $('#entryContainer_' + newsEntryId);

    var result = confirm("Are you sure you want to delete this entry?");
    if (result) {
        $.ajax({

            type: "Get",
            url: "/admin/news?handler=Delete",
            data: { id: newsEntryId },
            success: function () {
                ShowSuccessSnackbar("Entry removed");
                $(element).fadeOut(500, function () {
                    element.remove();
                });
            },
            error: function () {
                location.reload();
            }
        });
    }
}

/* Function to convert string to paragraph html with breaks */
function paragraphBreaks() {

    var containers = $('.news-entry-content');

    /* For each paragraph */
    $(containers).each(function () {
        var bodyP = $(this).children('.news-body-text');

        /* Insert line breaks */
        if ($(bodyP).text().indexOf("<br>") >= 0) {
            $(bodyP).html($(bodyP).text());
        }
    });
}

/* Function to determine text overflow */
function checkTextOverflow() {
    /* Check news text overflow */
    var element = $('.news-entry-content');
    $(element).each(function () {

        if (this.offsetHeight < this.scrollHeight || this.offsetWidth < this.scrollWidth) {
            var btn = $(this).parent().children('.collapse-news-btn');
            $(btn).show();
        }
    });
}

/* Function to get news entries by date */
function getEntriesByDate(date) {

    $.ajax({
        type: 'GET',
        url: "/admin/news/index?handler=Filter",
        contentType: 'application/json; charset=utf-8"',
        data: { dateFilter: date },
        success: function (result) {
            $("#adminNewsEntries").html(result);  
            paragraphBreaks();
            checkTextOverflow();
            checkNewsEntryCount($('#adminNewsEntries').children().length);
        },
        error: function (error) {
            console.log(error);
        }
    });
}

/* Function to check news entry count and show/hide alert */
function checkNewsEntryCount(count) {

    if (count === 0) {
        $('#noNewsAlert').show();
    } else {
        $('#noNewsAlert').hide();
    }
}

function openImgModal(url) {
    var modal = document.getElementById('newsImgModal');
    var modalHeader = document.getElementById('newsImgHeader');
    var modalImg = document.getElementById("newsImg");

    $(modal).modal("toggle");
    modalHeader.innerHTML = url;
    modalImg.src = url;
}