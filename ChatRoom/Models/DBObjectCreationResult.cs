using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Models
{
    public class DBObjectCreationResult
    {
        public bool Result { get; set; }
        public string Identifier { get; set; }

        public DBObjectCreationResult(bool result, string identifier)
        {
            this.Result = result;
            this.Identifier = identifier;
        }
    }
}