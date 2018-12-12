using System;
using System.Collections.Generic;

namespace ChatApp.Models.Entities
{
    public partial class PrivateMessage
    {
        public PrivateMessage()
        {

        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string ToUserId { get; set; }
        public int ChatId { get; set; }

        public virtual Chat Chat { get; set; }
        public virtual AspNetUsers ToUser { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
