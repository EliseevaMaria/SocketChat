using Models;
using SocketChatServer.Managers;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketChatServer.Handlers
{
    public sealed class IntroducingSocketHandler : SocketHandlerBase
    {
        public IntroducingSocketHandler(ConnectionManager connectionManager) : base(connectionManager) { }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            ChatUser user = this.ConnectionManager.GetUser(socket);
            await this.SendMessage(user.Id, "Please, enter your name to join the chat:");
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            ChatUser user = this.ConnectionManager.GetUser(socket);
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

            if (string.IsNullOrWhiteSpace(user.Name))
            {
                await this.SetUserName(user, message);
                return;
            }

            message = $"{user.Name} says: {message}";
            await this.SendMessageToAll(message, user.Id);
        }

        private async Task SetUserName(ChatUser user, string message)
        {
            user.Name = message;
            await this.SendMessage(user.Id, $"Your name is now \"{message}\"!");
            await this.SendMessageToAll($"New user {user.Name} joined!", user.Id);
        }
    }
}