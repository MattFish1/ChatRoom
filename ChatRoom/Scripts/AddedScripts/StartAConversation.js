
$(function () {

    $("#startForm").on("submit", function () {
        var formId = "#" + $(this).attr("value").toString();
        window.location = "http://chatroombeta.azurewebsites.net/home/myConversations";
    });
});