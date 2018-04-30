$(document).ready(function () {

    var volumeSlider = $('#volumeSlider');    
})

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
    console.log("Ready");
    playPause();
});

/* Volume slider change */
volumeSlider.oninput = function () {

    var volume = $('#volumeSlider').val();
    console.log(volume);

    var value = volume / 100;
    console.log(value);

    wavesurfer.setVolume(value);
}

var slider = document.querySelector('#slider');

slider.oninput = function () {
    var zoomLevel = Number(slider.value);
    wavesurfer.zoom(zoomLevel);
};

function playSong(file, title) {
    wavesurfer.load('music/' + file);

    /* Set title */
    $('#songPlayingTitle').html(title);
}

function playPause() {

    if (wavesurfer.isPlaying()) {
        console.log("Pause");
        wavesurfer.pause();
        $('#playPauseBtn').children('i').removeClass('fa-pause-circle');
        $('#playPauseBtn').children('i').addClass('fa-play-circle');
    }
    else {
        console.log("Play");
        wavesurfer.play();
        $('#playPauseBtn').children('i').removeClass('fa-play-circle');
        $('#playPauseBtn').children('i').addClass('fa-pause-circle');
    }
}