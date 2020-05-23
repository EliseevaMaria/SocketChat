using System;
using System.Net.WebSockets;

namespace Models
{
    public sealed class ChatUser
    {
        public ChatUser(WebSocket socket)
        {
            this.Guid = Guid.NewGuid();
            this.Socket = socket;
        }

        private Guid Guid { get; }
        public string Id => this.Guid.ToString("N");
        public WebSocket Socket { get; set; }

        public string Name { get; set; }

        public override string ToString() => this.Name;
    }
}