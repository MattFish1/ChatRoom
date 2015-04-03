using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using ChatRoom.Models;

namespace ChatRoom.Controllers
{
    public class APIController : ApiController
    {
        ChatDBEntities db = new ChatDBEntities();

        public JArray GetConversation(string conversationID)
        {
            List<Message> messages = db.Conversations.Find(conversationID).Messages.ToList<Message>();
            //List<string> messages = db.Conversations.Find(conversationID).Messages.Select(x => x.MessageText).ToList<string>();
            JArray jMessages = new JArray();
             
            foreach (var message in messages)
            {
                //put parts of message that we need into a new dynamic object,
                dynamic dynamicMessage = new ExpandoObject();
                dynamicMessage.ID = message.MessageID;
                dynamicMessage.Sender = message.Sender;
                dynamicMessage.MessageText = message.MessageText;
                //add it to jMessages by turning it into a jToken
                jMessages.Add(JToken.FromObject(dynamicMessage));
            }

            return jMessages;
        }

      
    }
}
