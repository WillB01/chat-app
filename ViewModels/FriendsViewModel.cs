using ChatApp.Models.Entities;

namespace ChatApp.ViewModels
{
    public class FriendsViewModel
    {
        public string IdentityId { get; set; }
        public string FriendId { get; set; }

        public virtual AspNetUsers Friend { get; set; }
        public virtual AspNetUsers Identity { get; set; }
    }
}