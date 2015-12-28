function CkEditorReadonly(ckEditor, readonly) {
    ckEditor.document.$.body.disabled = readonly;
    ckEditor.document.$.body.contentEditable = !readonly;
    ckEditor.document.$.designMode = readonly ? "off" : "on";
}

function loadStaticConttenc() {
    var typeStaticContent = $("#TypeStaticContent").val();
    var ckEditor = CKEDITOR.instances["Body"];
    CkEditorReadonly(ckEditor, true);

    ckEditor.setData("Wait. Loading...");

    var url = urlGetStaticContent + "?type=" + typeStaticContent;

    $.get(url, function (data) {
        ckEditor.setData(data.body);
        $("#Title").val(data.title);
        CkEditorReadonly(ckEditor, false);
    });
}

$(document).ready(function () {
    CKEDITOR.replace('Body');

    CKEDITOR.on('instanceReady', function () {
        loadStaticConttenc();
    });

    $("#TypeStaticContent").change(function () {
        loadStaticConttenc();
    });
});