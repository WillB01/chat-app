﻿namespace ChatApp.ViewModels
{
    public class FriendRequestVM
    {
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public bool? HasAccepted { get; set; }

        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
    }
}