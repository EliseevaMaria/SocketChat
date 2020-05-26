using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using SocketChatServer.Handlers;

namespace SocketChatServer.Managers
{
    public sealed class SocketMiddleware
    {
        private readonly SocketHandlerBase handler;

        public SocketMiddleware(RequestDelegate next, SocketHandlerBase handler)
        {
            this.handler = handler;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();
            await handler.OnConnected(socket);
            await this.Receive(socket);
        }

        private async Task Receive(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        await this.handler.Receive(webSocket, result, buffer);
                        break;

                    case WebSocketMessageType.Close:
                        await this.handler.OnDisconnected(webSocket);
                        break;
                }
            }
        }
    }
}