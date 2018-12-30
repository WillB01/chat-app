using System;
using System.Collections.Generic;

namespace ChatApp.Models.Entities
{
    public partial class Message
    {
        public Message()
        {
            Conversation = new HashSet<Conversation>();
        }

        public int Id { get; set; }
        public string IdentityId { get; set; }
        public string Message1 { get; set; }
        public DateTime Time { get; set; }

        public virtual AspNetUsers Identity { get; set; }
        public virtual ICollection<Conversation> Conversation { get; set; }
    }
}