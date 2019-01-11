namespace ChatApp.Models.Entities
{
    public partial class FriendRequest
    {
        public int Id { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public bool? HasAccepted { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
    }
}