using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.ViewModels
{
    public class GroupChatVM
    {
        public string GroupName { get; set; }
        public string GroupAdminId { get; set; }
        public string GroupMemberId { get; set; }
        public string Message { get; set; }
        public DateTime? Time { get; set; }
        public bool IsLoggedIn { get; set; }
        public string Name { get; set; }


        //public virtual AspNetUsers GroupAdmin { get; set; }
        //public virtual AspNetUsers GroupMember { get; set; }
    }
}
