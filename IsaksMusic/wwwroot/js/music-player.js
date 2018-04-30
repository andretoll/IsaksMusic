$(document).ready(function () {

    var volumeSlider = $('#volumeSlider');
    var zoomSlider = $('#zoomSlider');
});

/* Wavesurfer options */
wavesurfer = WaveSurfer.create({
    container: '#waveform',
    waveColor: 'white',
    progressColor: '#ffc600',
    barHeight: 2,
    barWidth: 3,
    responsive: true,
    hideScrollbar: true
});

/* Set initial volume */
wavesurfer.setVolume(0.5);

/* When song has finished */
wavesurfer.on('finish', function () {
    $('#playPauseBtn').children('i').removeClass('fa-pause-circle');
    $('#playPauseBtn').children('i').addClass('fa-play-circle');
});

/* When file is ready */
wavesurfer.on('ready', function () {
    playPause();
});

/* Volume slider change */
volumeSlider.oninput = function () {

    var volume = $('#volumeSlider').val();

    var value = volume / 100;

    wavesurfer.setVolume(value);
};

/* Zoom slider change */
zoomSlider.oninput = function () {
    var zoomLevel = Number(slider.value);
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