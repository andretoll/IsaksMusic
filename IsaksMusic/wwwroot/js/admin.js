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

    $('#songUpload').change(function () {
        var filePath = $('#songUpload').val();
        console.log(filePath);

        var fileExtension = filePath.split('.').pop();
        console.log(fileExtension);
    });
});

function startLoadingAnimation() {
    $('#songUploadAnim').removeClass('d-none');
}