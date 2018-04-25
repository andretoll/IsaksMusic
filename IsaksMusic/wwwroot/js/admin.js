$(document).ready(function () {

    /* Change active link in admin layout */
    $('a.active').removeClass('active');
    $('a[href="' + location.pathname + '"]').closest('.list-group-item').addClass('active');

    /* Enable validation of hidden fields */
    $('#songForm').validate({
        ignore: [],
        submitHandler: function (form) {
            form.submit();
        }
    });

    /* On file upload change */
    $('#musicFile').change(function () {
        
    });
});

/* Ajax function to upload music file */
function uploadFiles(inputId) {
    var input = document.getElementById(inputId);
    var files = input.files;
    var formData = new FormData();

    for (var i = 0; i !== files.length; i++) {
        formData.append("musicFile", files[i]);
    }

    $.ajax(
        {
            url: "../api/files/upload",
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                xhr.upload.addEventListener("progress", function (evt) {
                    if (evt.lengthComputable) {
                        var progress = Math.round(evt.loaded / evt.total * 100);
                        $(".progress-bar").css("width", progress + "%").attr("aria-valuenow", progress);
                        $(".progress-bar").html(progress + "%");
                    }
                }, false);
                return xhr;
            },
            success: function (data) {
                $("#progress").hide();
                $("#upload-status").show();

                $('.progress-bar').addClass('bg-success');
            }
        }
    );
}