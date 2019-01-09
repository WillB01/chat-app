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
           
            if (db.GroupMemberId == null)
            {
                _chatContext.GroupChat.Remove(db);

            }
           
            await _chatContext.SaveChangesAsync();


        }

        public async Task<GroupChatVM[]> GetGroupChatHistoryAsync(string group)
        {
            var user = await _userService.GetloggedinUser();

            var chats = await _chatContext.GroupChat
               .Where(e => e.GroupName == group)
               .Select(e => new GroupChatVM
               {
                   GroupName = e.GroupName,
                   GroupMemberId = e.GroupMemberId,
                   Message = e.Message,
                   Time = (DateTime)e.Time,
                   IsLoggedIn = e.GroupMemberId == user.Id,
                   Name = _userService.GetUserNameById(e.GroupMemberId).Result



               })

               .ToArrayAsync();

            return chats;
        }

        public async Task SaveGroupMessage(string message, string name, DateTime now, string group)
        {
            var user = await _userService.GetloggedinUser();
            var db = await _chatContext.GroupChat
             .Where(e => e.GroupName == group)
             .FirstOrDefaultAsync();
            var groupChat = new GroupChat
            {
                GroupAdminId = db.GroupAdminId,
                GroupMemberId = user.Id,
                Message = message,
                Time = now,
                GroupName = group
            };
            if (db.Message == null)
            {
                _chatContext.GroupChat.Remove(db);

            }
            _chatContext.GroupChat.Add(groupChat);
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
