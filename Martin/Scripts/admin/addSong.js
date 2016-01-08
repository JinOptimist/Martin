$(document).ready(function () {
   
    function updateFullSongName() {
        $("#fullSongName").text($("#visibleOrder").text() + ". Martin S. - " + $("#Name").val());
    }

    $("#Name").on('input', function () {
        updateFullSongName();
    });

    updateFullSongName();
});