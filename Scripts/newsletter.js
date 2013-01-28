$("document").ready(function () {

    var config = {};
    var html = '';
    //editor = CKEDITOR.appendTo('ChildItems', config, html);
    //editor = CKEDITOR.appendTo('ChildItems', config, html);
    if ($('#ChildItems .editor').length > 0) {
        $('#ChildItems .editor').ckeditor();
    }

    $("#AddNewsletterItem").click(function (event) {
        event.preventDefault();
        var count = $("#ChildItems table").size();
        $("#ChildItems").append(
            "<table><tbody><tr><td colspan=\"2\"><h3 style=\"float:left\">Nieuwsbrief Item " + (count+1) + "</h3><a href=\"#\" class=\"remove\" style=\"float:right;margin-top:1em;\"><img alt=\"Verwijderen\" class=\"delete-button\" src=\"/Content/img/delete.png\" title=\"Verwijderen\"></a></td></tr>" +
            "<tr><td><label for=\"Items_" + count + "__Title\">Titel</label></td><td><input class=\"text-box single-line\" id=\"Items_" + count + "__Title\" name=\"Items[" + count + "].Title\" type=\"text\" \"></td></tr>" +
            "<tr><td><label for=\"Items_" + count + "__SubTitle\">Sub titel</label></td><td><input class=\"text-box single-line\" id=\"Items_" + count + "__SubTitle\" name=\"Items[" + count + "].SubTitle\" type=\"text\" \></td></tr>" + 
            "<tr><td><label for=\"Items_" + count + "__IconImagePath\">Icon</label></td><td><input id=\"Items_" + count + "__IconImagePath\" class=\"text-box single-line\" type=\"text\" value=\"\" name=\"Items[" + count + "].IconImagePath\"></td></tr>" +
            "<tr><td><label for=\"Items_" + count + "__ImagePath\">Afbeelding</label></td><td><input id=\"Items_" + count + "__ImagePath\" class=\"text-box single-line\" type=\"text\" value=\"\" name=\"Items[" + count + "].ImagePath\"></td></tr>" +
            "<tr><td colspan=\"2\"><label for=\"Items_" + count + "__Text\">Text</label></td></tr><tr><td colspan=\"2\"><textarea class=\"editor\" cols=\"20\" id=\"Items_" + count + "__Text\" name=\"Items[" + count + "].Text\" rows=\"" + count + "\"></textarea></td></tr>" +
            "</tbody></table>");
        $("#Items_" + count + "__Text").ckeditor();
    });


    $(".remove").click(function (event) {
        event.preventDefault();

        editor = $(this).parent().parent().parent().parent().find("textarea.editor").ckeditorGet();
        editor.destroy
        var count = /\d+(?:\.\d+)?/.exec($(this).parent().parent().parent().parent().find(".text-box").attr("name"))[0];
        $(this).parent().parent().parent().parent().nextAll().each(function () {

            var textareaId = $(".editor", this).attr("id");
            var editor = $(".editor", this).ckeditorGet();
            $(".editor", this).html(editor.getData());
            editor.destroy(true);

            $(".text-box", this).each(function () {
                var name = $(this).attr("name");
                $(this).attr("name", name.substr(0, 6) + count + name.substr(7));
            });

            var name = $(".editor", this).attr("name");
            $(".editor", this).attr("name", name.substr(0, 6) + count + name.substr(7));
            $(".editor", this).attr("id", textareaId.substr(0, 6) + count + textareaId.substr(7));
            $(".editor", this).ckeditor();

            count++;
        });

        $(this).parent().parent().parent().parent().remove();
    });

    $(".preview").click(function (event) {
        $.ajax({
            type: "POST",
            url: "/newsletter/PreviewNewsletter",
            data: $("#ModelForm").parent().serialize(),
            success: function (data) {
                $("#dialog-modal").html(data).dialog({
                    width: 1100,
                    height: 600,
                    modal: true
                }).on("dialogclose", function (event, ui) {
                    this.html("");
                });
            }
        });
    });

    $(".adresses").click(function (event) {
        $.ajax({
            type: "POST",
            url: "/newsletter/NewsletterAdresses/",
            success: function (data) {
                $("#dialog-modal").html(data).dialog({
                    width: 1100,
                    height: 600,
                    modal: true
                });
            }
        });
    });

    $(".send").click(function (event) {
        event.preventDefault();
        $('#dialog_box').dialog({ 
            title: 'Weet je zeker dat je de nieuwsbrief wilt versturen?',
            width: 500, 
            height: 30, 
            modal: true, 
            resizable: false, 
            draggable: false, 
            buttons:
            [{ 
                text: 'Ja',
                click: function () {
                    location.href = "/Newsletter/Send/" + $("#NewsletterID").val();
                } 
            }, { 
                text: 'Nee',
                click: function () { $(this).dialog('close'); return false; }
            }]
        });
    });
});