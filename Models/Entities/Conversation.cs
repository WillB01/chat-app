namespace ChatApp.Models.Entities
{
    public partial class Conversation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ToUserId { get; set; }
        public int MessageId { get; set; }

        public virtual Message Message { get; set; }
        public virtual AspNetUsers ToUser { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}