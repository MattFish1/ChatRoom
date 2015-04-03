//variables
var conversationID;
var friend;
var currentUser;

function make_ajax_call(conversationID)
{
    //get messages from database with ajax
    $.ajax({
        type: "get",
        url: "http://chatroombeta.azurewebsites.net/api/api/getconversation/?conversationID=" + conversationID,
        success: function (data)
        {
            $("#Conversation").empty();
            var messages = new Array();
            messages = data;
            for (var i = 0; i < messages.length; i++)
            {
                    
                //alert(messages[i].Sender + messages[i].MessageText);
                var messageElement = document.createElement("p");
                var sender = "";
                if(messages[i].Sender === currentUser)
                {
                    sender = "Me";
                }
                else {
                    sender = messages[i].Sender;
                }
                var messageText = document.createTextNode(sender + ": " + messages[i].MessageText)
                messageElement.appendChild(messageText);
                document.getElementById("Conversation").appendChild(messageElement);
            }
            setTimeout(make_ajax_call(conversationID), 1000);
            
        },
        
});
}

$(function () {

    $.ajax({
        type: "GET",
        ulr: "http://chatroombeta.azurewebsites.net/home/getcurrentuser",
        success: function (data) {
            currentUser = data;
        }
    });

    $("#sendMessage").hide();

    var currentUser = $("#currentUser").attr("value");
  
    
    $(".conversation").on("click", function () {
        
        
        //var thisElement = document.getElementById($(this).id);
        
        conversationID = $(this).attr("data-conversationid").toString();
        friend = $(this).attr("value");

        //hide conversationList div and show conversation
        $("#conversationList").hide();
        $("#Conversation").show();
        $("#ConversationTitle").show();
        $("#sendMessage").show();
        //start ajax loop
        make_ajax_call(conversationID);

        //create conversation in dom
        var title = document.createElement("h3");
        var titleText = document.createTextNode($(this).attr("value"));
        title.appendChild(titleText);
        document.getElementById("ConversationTitle").appendChild(title);

        //fill out send message values
        $("#friendInput").attr("value") = friend;
        $("#conversationIDInput").attr("value") = friend;
    });

    $("#sendMessageForm").on("submit", function (e) {
        e.preventDefault();
        var message = $("#message").val();
        //send message with ajax
        $.ajax({
            type: this.method,
            url: this.action,
            data: {friend: friend, conversationID: conversationID, message:message},
            success: function(data)
            {

            }
        });
        $("#message").val() = "";
    });
})