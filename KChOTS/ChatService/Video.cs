using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;

namespace KChOTS.ChatService {
    public class Video : Hub {
        public void SendStreamVideo(string image) {
            Clients.receiveStreamVideo(image);
        }
    }
}