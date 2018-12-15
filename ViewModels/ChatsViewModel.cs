using ChatApp.Models.Entities;
using System;

namespace ChatApp.ViewModels
{
    public class ChatsViewModel
    {
        public int Id { get; set; }
        public string IdentityId { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }

        public virtual AspNetUsers Identity { get; set; }

        public bool IsLoggedin { get; set; } = false;
    }
}