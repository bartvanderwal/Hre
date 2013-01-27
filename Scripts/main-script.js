$(function() {
    // Generic | Validation Message after Submit
    $("#errorMessagePopup").css("display","inherit");
    $("#errorMessagePopup").dialog({
        modal: true,
        autoOpen: true,
        resizable: false,
        height: "auto",
        width: 450,
        buttons: {
            "Ok": function () {
                $(this).dialog("close");
            }
        }
    });

});