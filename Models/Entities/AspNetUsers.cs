using System;
using System.Collections.Generic;

namespace ChatApp.Models.Entities
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            ConversationToUser = new HashSet<Conversation>();
            ConversationUser = new HashSet<Conversation>();
            FriendsFriend = new HashSet<Friends>();
            FriendsIdentity = new HashSet<Friends>();
            GroupChatGroupAdmin = new HashSet<GroupChat>();
            GroupChatGroupMember = new HashSet<GroupChat>();
            Message = new HashSet<Message>();
            ProfileImage = new HashSet<ProfileImage>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public virtual ICollection<Conversation> ConversationToUser { get; set; }
        public virtual ICollection<Conversation> ConversationUser { get; set; }
        public virtual ICollection<Friends> FriendsFriend { get; set; }
        public virtual ICollection<Friends> FriendsIdentity { get; set; }
        public virtual ICollection<GroupChat> GroupChatGroupAdmin { get; set; }
        public virtual ICollection<GroupChat> GroupChatGroupMember { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<ProfileImage> ProfileImage { get; set; }
    }
}