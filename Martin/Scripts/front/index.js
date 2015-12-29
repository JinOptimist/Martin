$(document).ready(function () {
    var songs = [];
    var songPlayedIndex = 0;

    $.ajaxSetup({ cache: false });

    function updateBackgroundImage() {
        $(".slide").css("background-image", "url(" + $("#albumBgImg").val() + ")");
    }

    function playCurrentSong() {
        $(".playSong").removeClass("song-played");
        $(".playSong[data-index='" + songPlayedIndex + "']").addClass("song-played");

        var song = songs[songPlayedIndex];
        $("#mainMp3Source").attr("src", song.path);
        $("#nowPlaying").text(song.albumName + " - " + song.songName);
        var music = $("#mainMp3Player")[0];
        music.load();
        music.play();
    }

    function playSong(obj) {
        songPlayedIndex = $(obj).data("index");
        playCurrentSong();
    }

    function drawPlayByClassBlack(className) {//Old Black+
        var example = $("." + className);
        for (var i = 0; i < example.length; i++) {
            var current = example[i];
            if (!current.getContext) {
                return;
            }
            ctx = current.getContext('2d');
            var w = current.width;
            var h = current.height;
            example.width = w;
            example.height = h;
            var bgColor = $(current).data("bgcolor");
            ctx.fillStyle = bgColor;
            ctx.moveTo(0, 0);
            ctx.lineTo(w, h / 2);
            ctx.lineTo(0, h);
            ctx.lineTo(0, 0);
            ctx.fill();
        }
    }

    function drawPlayByClass(className) {
        var example = $("." + className);
        for (var i = 0; i < example.length; i++) {
            var current = example[i];
            if (!current.getContext) {
                return;
            }
            ctx = current.getContext('2d');
            var w = current.width;
            var h = current.height;
            example.width = w;
            example.height = h;

            ctx.beginPath();
            ctx.fillStyle = $(current).data("bgcolor");
            ctx.arc(w / 2, h / 2, h / 2, 0, 2 * Math.PI);
            ctx.fill();

            ctx.beginPath();
            ctx.fillStyle = "red";
            ctx.lineWidth = 2;
            ctx.moveTo(w / 4, h / 4);
            ctx.lineTo(w * 4 / 5, h / 2);
            ctx.lineTo(w / 4, h * 3 / 4);
            ctx.lineTo(w / 4, h / 4);
            ctx.fill();
        }
    }

    function drawDownloadByClass(className) {
        var example = $("." + className);
        for (var i = 0; i < example.length; i++) {
            var current = example[i];
            if (!current.getContext) {
                return;
            }
            ctx = current.getContext('2d');
            var w = current.width;
            var h = current.height;
            example.width = w;
            example.height = h;
            var bgColor = $(current).data("bgcolor");
            ctx.fillStyle = bgColor;

            ctx.fillRect(w * 2 / 5, 0, w / 5, h * 2 / 5);

            ctx.moveTo(w / 5, h * 2 / 5);
            ctx.lineTo(w * 4 / 5, h * 2 / 5);
            ctx.lineTo(w / 2, h * 4 / 5);
            ctx.lineTo(w / 5, h * 2 / 5);

            ctx.fillRect(0, h * 4 / 5, w, h / 5);

            ctx.fill();
        }
    }

    function updateAll() {
        updateBackgroundImage();
        drawPlayByClass("playSong");
        drawDownloadByClass("downloadImg");

        songs = [];
        $(".playSong").each(function() {
            var song = {
                "path": $(this).data("songpath"),
                "albumName": $(this).data("albumname"),
                "songName": $(this).data("songname")
            }
            songs.push(song);
        });

        var cuteUrl = $("#albumCuteUrl").val();
        window.history.pushState("Smile ^_^", "Title", cuteUrl);
    }

    function openNewAlbum(obj) {
        var url = $(obj).data("url");
        $('.slide').hide(1000);

        $.get(url, function (data) {
            //done
            $(".slide").fadeOut(1000, function() {
                var div = $(data).hide();
                $(this).replaceWith(div);
                $('.slide').fadeIn(1000);
                updateAll();
            });
        });

        return false;
    }

    updateAll();

    $(document).on("click", ".playSong", function () {
        playSong(this);
    });

    $(document).on("click", ".getAlbumLink", function () {
        openNewAlbum(this);
    });

    $("#mainMp3Player")[0].addEventListener('ended', function (e) {
        songPlayedIndex++;
        if (songPlayedIndex >= songs.length)
            songPlayedIndex = 0;
        playCurrentSong();
    });
});