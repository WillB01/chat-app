using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ChatApp.Models.Entities
{
    public partial class Friends
    {
        private ILazyLoader _lazyLoader;

        private AspNetUsers _friend;

        public Friends()
        {
        }

        public Friends(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        public string IdentityId { get; set; }
        public string FriendId { get; set; }

        public virtual AspNetUsers Friend
        {
            get => _lazyLoader.Load(this, ref _friend);
            set => _friend = value;
        }

        public virtual AspNetUsers Identity { get; set; }
    }
}