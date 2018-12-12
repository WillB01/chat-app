using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.ViewModels
{
    public class ChatsViewModel
    {
        public int Id { get; set; }
        public string IdentityId { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }

        public virtual AspNetUsers Identity { get; set; }
    }
}
