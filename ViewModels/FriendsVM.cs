using ChatApp.Models.Entities;

namespace ChatApp.ViewModels
{
    public class FriendsVM
    {
        public string IdentityId { get; set; }
        public string FriendId { get; set; }
        public int AmountOfFriends { get; set; }
        public string Name { get; set; }
        
        public virtual AspNetUsers Friend { get; set; }
        public virtual AspNetUsers Identity { get; set; }
    }
}