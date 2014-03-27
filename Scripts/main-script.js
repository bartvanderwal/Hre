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
    $("a[href^='https://www.hetrondjeeilanden.nl/']").each(function() {
            var $this = $(this),
            aHref = $this.attr('href');  //get the value of an attribute 'href'.
            $this.attr('href', aHref.replace('https://www.hetrondjeeilanden.nl/', 'http://localhost:59162/')); //set the value of an attribute 'href'.
    });
});