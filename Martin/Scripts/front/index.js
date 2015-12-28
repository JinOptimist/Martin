$(document).ready(function () {
    $.ajaxSetup({ cache: false });

    function updateBackgroundImage() {
        $(".slide").css("background-image", "url(" + $("#albumBgImg").val() + ")");
    }

    function playSong(obj) {
        var songPath = $(obj).data("songpath");
        var albumName = $(obj).data("albumname");
        var songName = $(obj).data("songname");

        $("#mainMp3Source").attr("src", songPath);
        $("#nowPlaying").text(albumName + " - " + songName);

        var music = $("#mainMp3Player")[0];
        music.load();
        music.play();
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
            var bgColor = $(current).data("bgcolor");
            ctx.fillStyle = bgColor;
            ctx.moveTo(0, 0);
            ctx.lineTo(w, h / 2);
            ctx.lineTo(0, h);
            ctx.lineTo(0, 0);
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

    $(document).on("click", ".downloadSong", function () {
        //var songpath = $(this).data("songpath");
        //window.location.href = songpath;
    });
});