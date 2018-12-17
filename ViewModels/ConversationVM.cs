namespace ChatApp.ViewModels
{
    public class ConversationVM
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ToUserId { get; set; }
        public int ChatId { get; set; }

        public virtual MessageVM Chat { get; set; }
        public virtual IdentityUserVM ToUser { get; set; }
        public virtual IdentityUserVM User { get; set; }


    }
}