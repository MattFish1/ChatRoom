//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChatRoom
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Conversations = new HashSet<Conversation>();
            this.Conversations1 = new HashSet<Conversation>();
            this.UserTokens = new HashSet<UserToken>();
        }
    
        public string UserName { get; set; }
        public string Password { get; set; }
    
        public virtual ICollection<Conversation> Conversations { get; set; }
        public virtual ICollection<Conversation> Conversations1 { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}