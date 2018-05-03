var wavesurfer = $('#waveform');
var repeatToggle = $('#toggleRepeat');
var shuffleToggle = $('#toggleShuffle');
var pulseToggle = $('#togglePulse');
var currentSong;
var repeat;
var shuffle;
var pulse;
var playlist;
var autoplay;
var ccInterval;

/* Color array */
var colors = new Array(
    [62, 35, 255],
    [60, 255, 60],
    [255, 35, 98],
    [45, 175, 230],
    [255, 0, 255],
    [255, 128, 0]);

var step = 0;
var colorIndices = [0, 1, 2, 3];

var gradientSpeed = 0.002;

$(document).ready(function () {       

    repeat = false;
    shuffle = false;
    pulse = false;
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

    /* Toggle pulse */
    pulseToggle.on('click', function (e) {
        var ele = $('#togglePulse');

        /* If button state is pressed */
        if (ele.attr("aria-pressed") === "true") {
            pulse = false;
            clearInterval(ccInterval);
            $('#waveformContainer').removeAttr('Style');
            $('#waveformControls').removeClass('pulse');
        } else {
            pulse = true;
            ccInterval = setInterval(updateGradient, 10);
            $('#waveformControls').addClass('pulse');
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
    $('#waveformEmptyMessage').hide();
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

function updateGradient() {

    if ($ === undefined) return;

    var c0_0 = colors[colorIndices[0]];
    var c0_1 = colors[colorIndices[1]];
    var c1_0 = colors[colorIndices[2]];
    var c1_1 = colors[colorIndices[3]];

    var istep = 1 - step;
    var r1 = Math.round(istep * c0_0[0] + step * c0_1[0]);
    var g1 = Math.round(istep * c0_0[1] + step * c0_1[1]);
    var b1 = Math.round(istep * c0_0[2] + step * c0_1[2]);
    var color1 = "rgb(" + r1 + "," + g1 + "," + b1 + ")";

    var r2 = Math.round(istep * c1_0[0] + step * c1_1[0]);
    var g2 = Math.round(istep * c1_0[1] + step * c1_1[1]);
    var b2 = Math.round(istep * c1_0[2] + step * c1_1[2]);
    var color2 = "rgb(" + r2 + "," + g2 + "," + b2 + ")";

    $('#waveformContainer').css({
        background: "-webkit-gradient(linear, left top, right top, from(" + color1 + "), to(" + color2 + "))"
    }).css({
        background: "-moz-linear-gradient(left, " + color1 + " 0%, " + color2 + " 100%)"
    });

    step += gradientSpeed;

    if (step >= 1) {
        step %= 1;
        colorIndices[0] = colorIndices[1];
        colorIndices[2] = colorIndices[3];

        colorIndices[1] = (colorIndices[1] + Math.floor(1 + Math.random() * (colors.length - 1))) % colors.length;
        colorIndices[3] = (colorIndices[3] + Math.floor(1 + Math.random() * (colors.length - 1))) % colors.length;

    }
}