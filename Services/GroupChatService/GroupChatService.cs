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
                GroupAdminId = groupChatVM.GroupAdminId,
                ExitGroup = false
                
            };
            _chatContext.GroupChat.Add(dbModel);
            await _chatContext.SaveChangesAsync();
        }

        public async Task AddMemberToGroupAsync(string group, string newMember)
        {
            var db = await _chatContext.GroupChat
                .Where(e => e.GroupName == group)
                .FirstOrDefaultAsync();
            var dbModel = new GroupChat
            {
                GroupName = db.GroupName,
                GroupAdminId = db.GroupAdminId,
                GroupMemberId = newMember,
                ExitGroup = false
                
            };

            _chatContext.GroupChat.Add(dbModel);

            if (db.GroupMemberId == null)
            {
                _chatContext.GroupChat.Remove(db);
            }

            await _chatContext.SaveChangesAsync();
        }

        public async Task DeleteUserFromGroupAsync(string group, string id)
        {

            var userGroup = await _chatContext.GroupChat
                 .Where(e => e.GroupName == group && e.GroupMemberId == id)
                 .ToArrayAsync();
            if (userGroup != null)
            {
                foreach (var item in userGroup)
                {
                    item.ExitGroup = true;
                }
            }

            if (userGroup != null)
            {
                var adminGroup = await _chatContext.GroupChat
                 .Where(e => e.GroupName == group && e.GroupAdminId == id)
                 .ToArrayAsync();

                _chatContext.RemoveRange(adminGroup);
            }

            await _chatContext.SaveChangesAsync();
            
        }

        public async Task<GroupChatVM[]> GetGroupChatHistoryAsync(string group)
        {
            var user = await _userService.GetloggedinUser();
            GroupChatVM[] chats;

            try
            {
                chats = await _chatContext.GroupChat
             .Where(e => e.GroupName == group)
             .Select(e => new GroupChatVM
             {
                 GroupName = e.GroupName,
                 GroupMemberId = e.GroupMemberId,
                 Message = e.Message,
                 Time = e.Time,
                 IsLoggedIn = e.GroupMemberId == user.Id,
                 Name = _userService.GetUserNameById(e.GroupMemberId).Result
             })

             .ToArrayAsync();
            }
            catch (Exception e)
            {
                chats = new GroupChatVM[] { };
            }

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
                GroupName = group,
                ExitGroup = false
                
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
                .Where(p => p.ExitGroup == false)
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