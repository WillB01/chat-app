using ChatApp.Models.Entities;
using ChatApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services.GroupChatService
{
    public class GroupChatService : IGroupChatService
    {
        private readonly ChatContext _chatContext;
        private readonly IUserService _userService;

        public GroupChatService(ChatContext chatContext, IUserService userService)
        {
            _chatContext = chatContext;
            _userService = userService;
        }

        public async Task AddGroupChatAsync(GroupChatVM groupChatVM)
        {
            var dbModel = new GroupChat
            {
                GroupName = groupChatVM.GroupName,
                GroupAdminId = groupChatVM.GroupAdminId
            };
            _chatContext.GroupChat.Add(dbModel);
            await _chatContext.SaveChangesAsync();
        }

        public async Task AddMemberToGroupAsync(string group, string newMember)
        {
            var db =  await _chatContext.GroupChat
                .Where(e => e.GroupName == group)
                .FirstOrDefaultAsync();

            var dbModel = new GroupChat
            {
                GroupName = db.GroupName,
                GroupAdminId = db.GroupAdminId,
                GroupMemberId = newMember
                
            };
            _chatContext.GroupChat.Add(dbModel);
            await _chatContext.SaveChangesAsync();


        }

        public async Task<GroupChatVM[]> UserGroups()
        {
            var user = await _userService.GetloggedinUser();
            var groups = await _chatContext.GroupChat
                .Where(e => e.GroupMemberId == user.Id || e.GroupAdminId == user.Id)
                .Select(p => new GroupChatVM
                {
                    GroupName = p.GroupName
                })
                .Distinct()
                .ToArrayAsync();
            return groups;
        }
    }
}
