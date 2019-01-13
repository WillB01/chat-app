using ChatApp.Services;
using ChatApp.Services.FriendService;
using ChatApp.Services.GroupChatService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly IFriendService _friendService;
        private readonly IGroupChatService _groupChatService;

        public ChatHub(IChatService chatService, IUserService userService, IFriendService friendService, IGroupChatService groupChatService)
        {
            _chatService = chatService;
            _userService = userService;
            _friendService = friendService;
            _groupChatService = groupChatService;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendPrivateMessage(string user, string message)
        {
            var toSendId = await _userService.GetUserId(user);
            var loggedInUser = await _userService.GetloggedinUser();

            await _chatService.SaveConversation(loggedInUser.Id, toSendId, message, DateTime.Now);

            await Clients.User(toSendId).ReceiveMessage(message, DateTime.Now, false, loggedInUser.UserName);
            await Clients.Caller.ReceiveMessage(message, DateTime.Now, true, user);
        }

        public string GetUserName() => Context.User.Identity.Name;

        public async Task<MessageVM[]> GetHistory(string value)
        {
            var loggedinUser = await _userService.GetloggedinUser();
            var conversation = await _chatService.GetUserConversation(loggedinUser, value);

            return conversation;
        }

        public async Task SaveGroupDb(string groupName)
        {
            var groupChatVM = new GroupChatVM
            {
                GroupAdminId = await _userService.GetUserId(Context.User.Identity.Name),
                GroupName = groupName
            };

            await _groupChatService.AddGroupChatAsync(groupChatVM);
        }

        public async Task<bool> AddMemberToGroupDb(string group)
        {
            var userId = await _userService.GetUserId(Context.User.Identity.Name);
            var doesUserExist = await _groupChatService.DoesMemberExistAsync(group, userId);
            if (!doesUserExist)
            {
                await _groupChatService.AddMemberToGroupAsync(group, userId);

            }
            return doesUserExist;
        }

        public async Task SendInviteToJoinGroup(string groupName, string[] friendsToAdd)
        {
            var friendsId = new List<string>();
            var friends = await _friendService.GetFriends(await _userService.GetloggedinUser());


            foreach (var name in friendsToAdd)
            {
                var t = friends.Where(e => e.Name == name).Select(e => e.IdentityId).FirstOrDefault();
                var existInGroup = await _groupChatService.DoesMemberExistAsync(groupName, t);
                if (!existInGroup)
                {
                    friendsId.Add(t);
                }
   
            }

            foreach (var id in friendsId)
            {
                await Clients.User(id).ReceiveGroupInvite(groupName);
            }
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendGroupMessage(string group, string message)
        {
            await Clients.Group(group).GroupReceiveMessage(message, Context.User.Identity.Name, DateTime.Now, group);
            await _groupChatService.SaveGroupMessage(message, Context.User.Identity.Name, DateTime.Now, group);
        }

        public async Task RemoveUserFromGroup(string group)
        {
            var user = await _userService.GetloggedinUser();
            await _groupChatService.DeleteUserFromGroupAsync(group, user.Id);
        }
       
        public async Task<MessageVM[]> GetGroupHistory(string group)
        {
            var loggedinUser = await _userService.GetloggedinUser();
            var conversation = await _chatService.GetUserConversation(loggedinUser, group);

            return conversation;
        }

        public async Task<GroupChatVM[]> GetUsersGroupsAsync()
        {
            var groups = await _groupChatService.UserGroups();
            return groups;
        }

        public async Task<GroupChatVM[]> GetGroupChatHistoryAsync(string group)
        {
            var chats = await _groupChatService.GetGroupChatHistoryAsync(group);
            return chats;
        }

        public async Task ResopnseFromGroupInvite(string group, bool inviteResponse)
        {
        }

        public string CreateGroupName(string group)
        {
            var id = Guid.NewGuid().ToString();
            return $"{group}{id}";
        }
    }
}