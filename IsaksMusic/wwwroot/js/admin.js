﻿$(document).ready(function () {

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
});

/* Function to start loading animation */
function startLoadingAnimation() {
    $('#songUploadAnim').removeClass('d-none');
}