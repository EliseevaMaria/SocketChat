using Models;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChatServer.Managers
{
    public sealed class ConnectionManager
    {
        private readonly ConcurrentDictionary<string, ChatUser> userConnections = new ConcurrentDictionary<string, ChatUser>();
        
        public void AddSocket(WebSocket socket)
        {
            var newUser = new ChatUser(socket);
            this.userConnections.TryAdd(newUser.Id, newUser);
        }

        public ConcurrentDictionary<string, ChatUser> GetAllUserConnections() => this.userConnections;
        public string GetUserId(WebSocket socket) => this.userConnections.FirstOrDefault(connection => connection.Value.Socket == socket).Key;
        public ChatUser GetUser(WebSocket socket) => this.userConnections.FirstOrDefault(connection => connection.Value.Socket == socket).Value;
        public WebSocket GetSocketByUserId(string id) => this.userConnections.TryGetValue(id, out ChatUser user) ? user.Socket : null;

        public async Task<bool> RemoveUserAsync(WebSocket socket)
        {
            string userId = this.GetUserId(socket);
            this.userConnections.TryRemove(userId, out ChatUser removedUser);
            if (removedUser == null)
                return false;

            await removedUser.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "connection closed", CancellationToken.None);
            return true;
        }
    }
}