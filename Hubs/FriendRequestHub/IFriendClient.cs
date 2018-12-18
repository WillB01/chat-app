﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs.FriendRequestHub
{
    public interface IFriendClient
    {
        Task ReceiveFriendRequest(bool sentRequest);

    }
}
