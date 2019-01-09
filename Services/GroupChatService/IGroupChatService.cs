using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services.GroupChatService
{
    public interface IGroupChatService
    {
        Task AddGroupChatAsync(GroupChatVM groupChatVM);
        Task AddMemberToGroupAsync(string group, string newMember);
        Task<GroupChatVM[]> UserGroups();
    }
}
