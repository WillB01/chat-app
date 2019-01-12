using System;
using System.Collections.Generic;

namespace ChatApp.Models.Entities
{
    public partial class GroupChat
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupAdminId { get; set; }
        public string GroupMemberId { get; set; }
        public string Message { get; set; }
        public DateTime? Time { get; set; }
        public bool? ExitGroup { get; set; }

        public virtual AspNetUsers GroupAdmin { get; set; }
        public virtual AspNetUsers GroupMember { get; set; }
    }
}
