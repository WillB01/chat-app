using System;
using System.Collections.Generic;

namespace ChatApp.Models
{
    public partial class Chat
    {
        public int Id { get; set; }
        public string IdentityId { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }

        public virtual AspNetUsers Identity { get; set; }
    }
}
