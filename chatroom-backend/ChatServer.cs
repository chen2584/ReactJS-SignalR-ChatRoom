using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using chatroom_backend.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Microsoft.Extensions.Primitives;

namespace chatroom_backend
{
    public class ChatServer : Hub
    {
        private static ConcurrentDictionary<string, UserInfo> userInfo = new ConcurrentDictionary<string, UserInfo>();
        private static readonly List<string> colorName = new List<string> {"#FF0000","#0000FF","#00FF00","#FFFF0"}; //สีชื่อ
        private static Random rnd = new Random();

        public override Task OnConnectedAsync()
        {
            int ColorIndex = rnd.Next(0, colorName.Count);
            string name = Context.Connection.GetHttpContext().Request.Query["userName"];
            if(name == null || name == "")
            {
                Context.Connection.Abort();
            }

            userInfo.TryAdd(Context.ConnectionId, new UserInfo { Name = name, NameColor = colorName[ColorIndex]} );
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception ex)
        {
            userInfo.TryRemove(Context.ConnectionId, out UserInfo dummy);
            return base.OnDisconnectedAsync(ex);
        }
        public Task onSendMessage(string message)
        {
            string ConnectionId = Context.ConnectionId;
            if(userInfo.ContainsKey(ConnectionId) && message != null && message != "")
            {
                return Clients.All.InvokeAsync("onReceiveMessage", userInfo[ConnectionId].Name, userInfo[ConnectionId].NameColor, message);
            }
            return Task.CompletedTask;
        }
    }
}