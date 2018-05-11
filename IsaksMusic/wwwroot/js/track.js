var wavesurfer = $('#waveform');
var repeat;
var filePath;

$(document).ready(function () {

    $('#songPlayingTitle').hide();
    $('#songPlayingCategories').hide();

    /* Toggle play/pause */
    $('#playPauseBtn').on('click', function () {
        playPause();
    });

    /* Initiate volume slider */
    $("#volumeSlider").slider({
        min: 0,
        max: 100,
        value: 50,
        range: "min",
        slide: function (event, ui) {
            setVolume(ui.value / 100);
        }
    });

    /* Check description overflow */
    var element = document.querySelector('#collapseDescription');

    if (element.offsetHeight < element.scrollHeight || element.offsetWidth < element.scrollWidth) {
        $('#collapseDescriptionBtn').show();
    }
    
});

/* Wavesurfer options */
wavesurfer = WaveSurfer.create({
    container: '#waveform',
    waveColor: 'white',
    progressColor: '#ffc600',
    barHeight: 1,
    barWidth: 3,
    cursorWidth: 0,
    responsive: true,
    hideScrollbar: true
});

/* When song has finished */
wavesurfer.on('finish', function () {
    $('#playPauseBtn').children('i').removeClass('fa-pause-circle');
    $('#playPauseBtn').children('i').addClass('fa-play-circle');

    if (repeat === true) {
        playPause();
    }
});

/* When file is ready */
wavesurfer.on('ready', function () {
    $('#waveformCounter').text(formatTime(wavesurfer.getCurrentTime()));
    $('#waveformDuration').text(formatTime(wavesurfer.getDuration()));
    $('#waveformMessage').hide();

    $('#songPlayingTitle').show();
    $('#songPlayingCategories').show();
});

/* When song is being played */
wavesurfer.on('audioprocess', function () {
    $('#waveformCounter').text(formatTime(wavesurfer.getCurrentTime()));
});

/* When song is seeked */
wavesurfer.on('seek', function () {
    $('#waveformCounter').text(formatTime(wavesurfer.getCurrentTime()));
});

/* When cursor moves over waveform */
$('#waveform').on('mousemove', function (e) {
    mousetooltiptime(e);
});

/* When cursor leaves waveform */
$('#waveform').on('mouseleave', function (e) {
    mousetooltiptime(false);
});

/* Toggle repeat */
$('#toggleRepeat').on('click', function () {
    /* If button state is pressed */
    if ($(this).attr("aria-pressed") === "true") {
        repeat = false;
    } else {
        repeat = true;
    }
});

function playPause() {

    if (wavesurfer.isPlaying()) {
        wavesurfer.pause();
        $('#playPauseBtn').children('i').removeClass('fa-pause-circle');
        $('#playPauseBtn').children('i').addClass('fa-play-circle');
    }
    else {
        wavesurfer.play();
        $('#playPauseBtn').children('i').removeClass('fa-play-circle');
        $('#playPauseBtn').children('i').addClass('fa-pause-circle');
    }
}

function formatTime(time) {
    return [
        Math.floor(time % 3600 / 60), // minutes
        ('00' + Math.floor(time % 60)).slice(-2) // seconds
    ].join(':');
}

function mousetooltiptime(e) {

    var perc = event.layerX / $('#waveform').width();

    var timeset = formatTime(Math.floor(wavesurfer.getDuration() * perc));

    if (e === false) {
        $('.tooltip-track').text(timeset).css('display', 'none');
    } else {
        $('.tooltip-track').text(timeset).css('left', e.pageX + 25).css('top', e.pageY - 25).css('display', 'block');
    }
}

function setVolume(myVolume) {

    if (myVolume === 0) {
        $('#volumeMenuButton').children('i').removeClass('fa-volume-down');
        $('#volumeMenuButton').children('i').removeClass('fa-volume-up');
        $('#volumeMenuButton').children('i').addClass('fa-volume-off');
    } else if (myVolume < .75) {
        $('#volumeMenuButton').children('i').removeClass('fa-volume-off');
        $('#volumeMenuButton').children('i').removeClass('fa-volume-up');
        $('#volumeMenuButton').children('i').addClass('fa-volume-down');
    } else if (myVolume >= .75) {
        $('#volumeMenuButton').children('i').removeClass('fa-volume-off');
        $('#volumeMenuButton').children('i').removeClass('fa-volume-down');
        $('#volumeMenuButton').children('i').addClass('fa-volume-up');
    }

    wavesurfer.setVolume(myVolume);
}
