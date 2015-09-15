using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRChat
{
    [HubName("Chat")]
    public class ChatHub : Hub
    {
        public void SendMessage(string message)
        {
            var msg = $"{Context.ConnectionId}: {message}";
            Clients.All.newMessage(msg);
        }

        public void JoinRoom(string room)
        {
            // NOTE: this is not persisted.
            Groups.Add(Context.ConnectionId, room);
        }

        public void SendMessageToRoom(string room, string message)
        {
            var msg = $"{Context.ConnectionId}: {message}";
            Clients.Group(room).newMessage(msg);
        }

        public void SendMessageData(SendData data)
        {
            // Process incoming data.
            // transform data...
            // Craft new data...
            Clients.All.newData();
        }

        public override Task OnConnected()
        {
            SendMonitoringData("Connected", Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            SendMonitoringData("Disconnected", Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            SendMonitoringData("Reconnected", Context.ConnectionId);
            return base.OnReconnected();
        }

        private void SendMonitoringData(string eventType, string connectionId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MonitorHub>();
            context.Clients.All.newEvent(eventType, connectionId);
        }

        //public Task<int> SendDataAsync()
        //{
        //    // Async ... work... 
        //}
    }

    public class SendData
    {
        public int Id { get; set; }
        public string Data { get; set; }
    }
}