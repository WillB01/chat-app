using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;

namespace ChatApp.Models.Entities
{
    public partial class Chat
    {
        //private ILazyLoader LazyLoader { get; set; }
        //public ICollection<PrivateMessage> _privateMessage;

        public Chat()
        {
            PrivateMessage = new HashSet<PrivateMessage>();
        }

        public int Id { get; set; }
        public string IdentityId { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }

        public virtual AspNetUsers Identity{ get; set;}
        public virtual ICollection<PrivateMessage> PrivateMessage{get; set;}
    }
}
