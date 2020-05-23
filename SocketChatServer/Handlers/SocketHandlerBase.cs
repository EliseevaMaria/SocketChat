﻿using Models;
using SocketChatServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChatServer.Handlers
{
    public abstract class SocketHandlerBase
    {
        protected ConnectionManager ConnectionManager { get; set; }
        public SocketHandlerBase(ConnectionManager connectionManager) => this.ConnectionManager = connectionManager;

        public virtual async Task OnConnected(WebSocket socket) => await Task.Run(() => this.ConnectionManager.AddSocket(socket));
        public virtual async Task OnDisconnected(WebSocket socket) => await this.ConnectionManager.RemoveUserAsync(socket);

        public async Task SendMessage(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            byte[] byteMessage = Encoding.ASCII.GetBytes(message);
            await socket.SendAsync(new ArraySegment<byte>(byteMessage, 0, message.Length),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }
        public async Task SendMessage(string id, string message) => await this.SendMessage(this.ConnectionManager.GetSocketByUserId(id), message);
        public async Task SendMessageToAll(string message, string exceptId = null)
        {
            List<WebSocket> addresseeSockets = this.ConnectionManager.GetAllUserConnections()
                .Where(connectionPair => !string.IsNullOrEmpty(connectionPair.Value.Name) && connectionPair.Key != exceptId)
                .Select(connectionPair => connectionPair.Value.Socket).ToList();

            foreach (WebSocket adressee in addresseeSockets)
                await SendMessage(adressee, message);
        }

        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}