$(document).ready(function () {

    var volumeSlider = $('#volumeSlider');
    var zoomSlider = $('#zoomSlider');
    var wavesurfer = $('#waveform');
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

/* Set initial volume */
wavesurfer.setVolume(0.5);

$('#waveform').children().tooltip({
    track: false,
    tooltipClass: "tooltip-position"
});

/* When song has finished */
wavesurfer.on('finish', function () {
    $('#playPauseBtn').children('i').removeClass('fa-pause-circle');
    $('#playPauseBtn').children('i').addClass('fa-play-circle');
});

/* When file is ready */
wavesurfer.on('ready', function () {
    $('#waveformDuration').text(formatTime(wavesurfer.getDuration()));
    playPause();
});

/* When song is being played */
wavesurfer.on('audioprocess', function () {
    $('#waveformCounter').text(formatTime(wavesurfer.getCurrentTime()));
});

/* Volume slider change */
volumeSlider.oninput = function () {

    var volume = $('#volumeSlider').val();

    var value = volume / 100;

    wavesurfer.setVolume(value);
};

/* Zoom slider change */
zoomSlider.oninput = function () {
    var zoomLevel = Number(zoomSlider.value);
    wavesurfer.zoom(zoomLevel);
};

function loadSong(file, title) {
    wavesurfer.load('music/' + file);

    /* Set title */
    $('#songPlayingTitle').html(title);
}

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

var formatTime = function (time) {
    return [
        Math.floor((time % 3600) / 60), // minutes
        ('00' + Math.floor(time % 60)).slice(-2) // seconds
    ].join(':');
};