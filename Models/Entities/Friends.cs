namespace ChatApp.Models.Entities
{
    public partial class Friends
    {
        public string IdentityId { get; set; }
        public string FriendId { get; set; }

        public virtual AspNetUsers Friend { get; set; }
        public virtual AspNetUsers Identity { get; set; }
    }
}