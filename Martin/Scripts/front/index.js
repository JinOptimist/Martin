$(document).ready(function () {
    $.ajaxSetup({ cache: false });
    function updateBackgroundImage() {
        $(".slide").css("background-image", "url(" + $("#albumCover").val() + ")");
    }

    function openNewAlbum(obj) {
        var url = $(obj).data("url");

        $.get(url, function (data) {
            //after get response

            function myFadeOut() {
                var div = $(data).hide();
                $(this).replaceWith(div);
                $('.slide').fadeIn({
                    duration: 1000,
                    queue: false
                });
                updateBackgroundImage();
            }

            $(".slide").fadeOut({
                duration: 1000,
                complete: myFadeOut,
                queue: false
            });

            //$(".slide").fadeOut("slow", function() {
            //    var div = $(data).hide();
            //    $(this).replaceWith(div);
            //    $('.slide').fadeIn("slow");
            //    updateBackgroundImage();
            //});
        });

        return false;
    }

    updateBackgroundImage();

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

    $(document).on("click", ".playSong", function () {
        playSong(this);
    });

    $(document).on("click", ".getAlbumLink", function () {
        openNewAlbum(this);
    });
});