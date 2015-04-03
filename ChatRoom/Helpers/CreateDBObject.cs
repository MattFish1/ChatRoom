using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatRoom.Models;

namespace ChatRoom.Helpers
{
    public class CreateDBObject
    {
        /// <summary>
        /// Attempts to creates a new user in Chat Room DataBase. Returns Tuple, item1: creation success/failure bool, item2: userName of (atempted) user created.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static DBObjectCreationResult CreateUser(string userName, string password)
        {
            //return value is a tuple object, item1 is a bool for the success or failure of user creation, item2 is the user name of attempted user 
            //creation
            DBObjectCreationResult returnValue = new DBObjectCreationResult(false, userName);

            //load database
            ChatDBEntities db = new ChatDBEntities();
            //create model of new user
            User userToCreate = new User();
            userToCreate.UserName = userName;
            userToCreate.Password = password;
            //try to save user to db
            try{
                db.Users.Add(userToCreate);
                db.SaveChanges();
                //find the user you tried to create, to see if it was successfully saved to the db
                User findUser = db.Users.Find(userName);
                //if user is found and has the same user name as userToCreate, return true and userName of successful creation
                if(findUser != null && findUser.UserName == userToCreate.UserName)
                {
                    returnValue = new DBObjectCreationResult(true, userName);

                    return returnValue;
                }
                    //user creation failed , return false and userName of failed creation
                else 
                    return returnValue;
            }
                //user creation failed
            catch{
                    return returnValue;
            }
            
        }

        /// <summary>
        /// Attempts to creates a new user token in Chat Room DataBase. Returns Tuple, item1: creation success/failure bool, item2: token (atempted to be) created.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static DBObjectCreationResult CreateUserToken(string userName)
        {
            //load db
            ChatDBEntities db = new ChatDBEntities();

            DBObjectCreationResult returnValues;
            //create model of new token
            UserToken tokenToCreate = new UserToken();
            tokenToCreate.DateCreated = DateTime.Now;
            tokenToCreate.UserName = userName;
            tokenToCreate.User = db.Users.Find(userName);
            tokenToCreate.Token = Guid.NewGuid().ToString();
            tokenToCreate.LastUsed = DateTime.Now;
            
            //try to save token to db
            try
            {
                db.UserTokens.Add(tokenToCreate);
                db.SaveChanges();
                UserToken findTK = db.UserTokens.Find(tokenToCreate.Token);
                if( findTK != null && findTK.Token == tokenToCreate.Token)
                {
                    returnValues = new DBObjectCreationResult(true, findTK.Token);
                    return returnValues;
                }
                    //creation failed
                    else
                    returnValues = new DBObjectCreationResult(false, tokenToCreate.Token);
                    return returnValues;     
            }
                //creation failed
            catch
            {
                returnValues = new DBObjectCreationResult(false, tokenToCreate.Token);
                return returnValues;  
            }
        }

        /// <summary>
        /// Attempts to creates a new conversation in Chat Room DataBase. Returns Tuple, item1: creation success/failure bool, item2: ID of conversation created.
        /// Item2 returns null if conversation is not created.  
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static DBObjectCreationResult CreateConversation(string user1, string user2)        {
            DBObjectCreationResult returnValues;

            //load db
            ChatDBEntities db = new ChatDBEntities();

            //create model of new conversation
            Conversation conversationToCreate = new Conversation();
            conversationToCreate.ConversationID = (db.Conversations.Count() + 1).ToString();
            conversationToCreate.User1 = user1;
            conversationToCreate.User2 = user2;

            //add foriegn key realationship of user to conversation, or virtual User to model
            User userOne = db.Users.Find(user1);
            User userTwo = db.Users.Find(user2);
            conversationToCreate.UserOne = userOne;
            conversationToCreate.UserTwo = userTwo;
            List<Conversation> currentUserConversations = db.Conversations.Where(x => x.User1 == user1 || x.User2 == user1).ToList<Conversation>();

            //if conversation ID or a conversation bewteen the same user and friend doesnt already exists return conversationCreated and save to db
            //else return null
            if ( currentUserConversations.Where(x => x.User2 == user2).Count() == 0) 
                
            {
                if (currentUserConversations.Where(x => x.User1 == user2).Count() == 0)
                {
                    //try to save conversation to db
                    try
                    {
                        db.Conversations.Add(conversationToCreate);
                        db.SaveChanges();

                        Conversation findConv = db.Conversations.Find(conversationToCreate.ConversationID);

                        if (findConv != null)
                        {
                            returnValues = new DBObjectCreationResult(true, findConv.ConversationID.ToString());
                            return returnValues;
                        }
                        else
                            returnValues = new DBObjectCreationResult(false, null);
                        return returnValues;
                    }
                    catch
                    {
                        returnValues = new DBObjectCreationResult(false, null);
                        return returnValues;
                    }
                }
                else
                    returnValues = new DBObjectCreationResult(false, null);
                return returnValues;
           }   

            else
                returnValues = new DBObjectCreationResult(false, null);
                    return returnValues;;
        }

        /// <summary>
        /// Attempts to creates a new message in Chat Room DataBase. Returns Tuple, item1: creation success/failure bool, item2: ID of message created.
        /// Item 2 returns null if message is not created.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static DBObjectCreationResult CreateMessage(string user, string friend, string messageText, string conversationID)
        {
            //test values
            //user = "matt_fish"; friend = "a_User"; messageText = "hello"; conversationID = "1";
            
            //return value
            DBObjectCreationResult returnValues = new DBObjectCreationResult(false, null);
            //load db
            ChatDBEntities db = new ChatDBEntities();
            //sender
            List<Conversation> currentUserConversations = db.Conversations.Where(x => x.User1 == user || x.User2 == user).ToList<Conversation>();
            User sender = db.Users.Find(user);
            //create message id
            string messageID = (db.Messages.Count() + 1).ToString();
            //try creating a conversation
            Conversation conversation;
            var conversationAttemp = CreateConversation(user, friend);
            if(conversationAttemp.Result)
            {
                conversation = db.Conversations.Find(conversationAttemp.Identifier);
            }
            else{
                conversation = currentUserConversations.Where(x => x.User1 == friend || x.User2 == friend).First();
            }
            
            //create model of new message
            Message messageCreated = new Message();
            messageCreated.MessageID = messageID.ToString();
            messageCreated.Sender = user;
            messageCreated.Reciever = friend;
            messageCreated.MessageText = messageText;
            //add parrent conversation field and realationship
            messageCreated.ParentConversation = conversationID.ToString();
            messageCreated.Conversation = conversation;
            messageCreated.DateSent = DateTime.Now;
            
            
            try
            {
                db.Messages.Add(messageCreated);
                db.SaveChanges();
                returnValues = new DBObjectCreationResult(true, messageCreated.MessageID);
            }
            catch
            {
                
            }
            
            return returnValues;
        }


    }
}