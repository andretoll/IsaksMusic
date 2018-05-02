var wavesurfer = $('#waveform');
var repeatToggle = $('#toggleRepeat');
var shuffleToggle = $('#toggleShuffle');
var currentSong;
var repeat;
var shuffle;
var playlist;

$(document).ready(function () {       

    repeat = false;
    shuffle = false;
    currentSong = 1;

    /* Toggle repeat */
    repeatToggle.on('click', function (e) {
        var ele = $('#toggleRepeat');

        /* If button state is pressed */
        if (ele.attr("aria-pressed") === "true") {
            repeat = false;
        } else {
            repeat = true;
        }
    });

    /* Toggle shuffle */
    shuffleToggle.on('click', function (e) {
        var ele = $('#toggleShuffle');

        /* If button state is pressed */
        if (ele.attr("aria-pressed") === "true") {
            shuffle = false;
        } else {
            shuffle = true;
        }
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

$("#volumeSlider").slider({
    min: 0,
    max: 100,
    value: 50,
    range: "min",
    slide: function (event, ui) {
        setVolume(ui.value / 100);
    }
});

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

function playNext() {

    /* If shuffling is toggled, randomize song */
    if (shuffle === true && playlist.length > 1) {

        var random = generateRandom(1, playlist.length, currentSong);
        nextSong = playlist[random - 1];
        currentSong = random;
    } else {
        /* Try to get the next song */
        currentSong++;
        var nextSong = playlist[currentSong - 1];
    }      

    /* If no song is found */
    if (nextSong === undefined) {
        /* If repeat is toggled */
        if (repeat === true) {      
            nextSong = playlist[0];
        }
        /* Else stop playback */
        else {
            return;
        }
    }

    /* If next song is found */
    loadSong(nextSong.playlistId);
}

function loadSong(id) {

    var song = playlist.filter(function (e) {
        return e.playlistId === id;
    })[0];

    wavesurfer.load(song.filePath);

    currentSong = song.playlistId;

    $('#songPlayingTitle').html(song.title);
    $('#songPlayingCategories').html(song.categories);

    /* Highlight song */
    currentSongElement = document.getElementById(id);

    $('#musicTable > tbody > tr').each(function () {
        $(this).removeClass('song-active');
    });

    $(currentSongElement).addClass('song-active');
}

function playPause() {

    /* If a song is not loaded */
    if (wavesurfer.getDuration() === 0) {
        loadSong(playlist[0].playlistId);
    }

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

    var perc = event.layerX / $('#waveform').width();

    var timeset = formatTime(Math.floor(wavesurfer.getDuration() * perc));

    if (wavesurfer.getCurrentTime() !== 0) {
        if (e === false) {
            $('.tooltip-track').text(timeset).css('display', 'none');
        } else {
            $('.tooltip-track').text(timeset).css('left', e.pageX + 25).css('top', e.pageY - 25).css('display', 'block');
        }
    }    
}

function generateRandom(min, max, exclude) {
    var num = Math.floor(Math.random() * (max - min + 1)) + min;
    return num === exclude ? generateRandom(min, max, exclude) : num;
}