using ChatApp.ViewModels;
using System;
using System.Threading.Tasks;

namespace ChatApp.Services.GroupChatService
{
    public interface IGroupChatService
    {
        Task AddGroupChatAsync(GroupChatVM groupChatVM);

        Task AddMemberToGroupAsync(string group, string newMember);

        Task<GroupChatVM[]> UserGroups();

        Task SaveGroupMessage(string message, string name, DateTime now, string group);

        Task<GroupChatVM[]> GetGroupChatHistoryAsync(string group);

        Task DeleteUserFromGroupAsync(string group, string id);
    }
}