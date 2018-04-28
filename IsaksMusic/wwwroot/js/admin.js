var categoryValue;
var categoryId;
var clicked;

$(document).ready(function () {

    $(document).click(function (e) {
        clicked = e.target;
        console.log(clicked);
    });

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
        categoryId = $(this).parent().parent('li').attr('id');

        /* Get content */
        categoryValue = $(this).html();

        /* Find and show save button */
        var listItem = this.closest('li');
        saveBtn = $(listItem).children('.list-item-save');
        $(saveBtn).show();
    });

    /* Upon leaving edit area */
    $('.list-item-editable').on('keypress blur', function (e) {

        if (e.keyCode && e.keyCode === 13 || e.type === 'blur') {

            /* Disable editable */
            this.contentEditable = false;

            //$(saveBtn).hide();

            return false;
        }

    });

    /* Prevent cut, copy and paste */
    $('.list-item-editable').on("cut copy paste", function (e) {
        e.preventDefault();
    });
});

/* Function to start loading animation */
function startLoadingAnimation() {
    $('#songUploadAnim').removeClass('d-none');
}

/* Function to delete song */
function deleteSong(songId) {

    var result = confirm("Are you sure you want to remove this song?");
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

/* Function to edit category name */
function editCategory(id, dom) {

    var newName = $(dom).html();

    console.log("Saving: " + categoryValue + " => " + newName);

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

    $(saveBtn).hide();
}