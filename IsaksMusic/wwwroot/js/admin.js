var categoryValue;
var categoryId;

$(document).ready(function () {

    /* Change active link in admin layout */
    $('a.active').removeClass('active');
    $('a[href="' + location.pathname + '"]').closest('.list-group-item').addClass('active');

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

    /* Initiate checkboxes */
    $('input[type=checkbox]').iCheck({
        checkboxClass: 'icheckbox_square-red',
        radioClass: 'iradio_square-red',
        increaseArea: '20%'
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
});

/* Function to start loading animation */
function startLoadingAnimation() {
    $('#songUploadAnim').removeClass('d-none');

    $('#songForm').find(':submit').hide();
}

/* Function to delete song */
function deleteSong(songId) {

    var result = confirm("Are you sure you want to delete this song?");
    if (result) {
        $.ajax({

            type: "Get",
            url: "/admin/songs?handler=Delete",
            data: { id: songId },
            success: function () {
                var row = $('#row_' + songId);
                ShowSuccessSnackbar("Song removed");
                removeTableRow(row);
            }
        });
    }
}

/* Function to remove specific row from table */
function removeTableRow(row) {
    $(row).fadeOut(500, function () {
        row.remove();
    });
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
                $(element).remove();
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