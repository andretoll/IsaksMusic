var volumeSlider = $('#volumeSlider');
var zoomSlider = $('#zoomSlider');
var wavesurfer = $('#waveform');
var repeatToggle = $('#toggleRepeat');
var shuffleToggle = $('#toggleShuffle');
var currentSong;
var repeat;

$(document).ready(function () {

    repeat = false;

    /* Toggle repeat */
    repeatToggle.on('click', function (e) {

        var ele = $('#toggleRepeat');

        /* If button state is pressed */
        if (ele.attr("aria-pressed") === "true") {
            repeat = false;
        } else {
            repeat = true;
        }

        console.log(repeat);
    });

    /* Toggle shuffle */
    shuffleToggle.on('click', function (e) {

    });
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

/* When song has finished */
wavesurfer.on('finish', function () {
    $('#playPauseBtn').children('i').removeClass('fa-pause-circle');
    $('#playPauseBtn').children('i').addClass('fa-play-circle');

    playNext();
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

/* Volume slider change */
volumeSlider.oninput = function () {

    var volume = $('#volumeSlider').val();

    var value = volume / 100;

    wavesurfer.setVolume(value);
};

function playNext() {

    /* If shuffling is toggled, randomize song */
    /* LOGIC TO RANDOMIZE SONG HERE */

    /* Try to get the next song */
    var nextSong = $(currentSong).next('tr');
    console.log(repeat);
    /* If no song is found */
    if (nextSong.length === 0) {
        /* If repeat is toggled */
        if (repeat === true) {            
            nextSong = $('#musicTable > tbody').children('tr:first');
        }
        /* Else stop playback */
        else {
            return;
        }
    }

    /* If next song is found, get path and title */
    var songId = nextSong.attr('id');
    var songTitle = nextSong.attr('title');
    loadSong(songId, songTitle);
}

function loadSong(file, title) {
    wavesurfer.load(file);

    /* Set title */
    $('#songPlayingTitle').html(title);

    /* Highlight song */
    currentSong = document.getElementById(file);

    $('#musicTable > tbody > tr').each(function () {
        $(this).removeClass('song-active');
    });

    $(currentSong).addClass('song-active');
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

function formatTime (time) {
    return [
        Math.floor(time % 3600 / 60), // minutes
        ('00' + Math.floor(time % 60)).slice(-2) // seconds
    ].join(':');
}

function mousetooltiptime(e) {
    var timeset = formatTime(Math.floor(event.layerX / wavesurfer.drawer.width * wavesurfer.getDuration()));

    if (wavesurfer.getCurrentTime() !== 0) {
        if (e === false) {
            $('.tooltip-track').text(timeset).css('display', 'none');
        } else {
            $('.tooltip-track').text(timeset).css('left', e.pageX + 25).css('top', e.pageY - 25).css('display', 'block');
        }
    }    
}
