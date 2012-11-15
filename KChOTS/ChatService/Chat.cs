using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR;
using SignalR.Hubs;
using KChOTS.Security;

namespace KChOTS.ChatService {
    public class Chat : Hub {
        public void NotifyUsers(string data) {
            Clients.onReceiveMessage(HttpUtility.HtmlEncode(data));
        }

        public void NotifyConnection(string data) {
            Clients.onReceiveMessage(data);
        }

        public void SendStreamVideo(string image) {
            Clients.receiveStreamVideo(image);
        }
    }
}