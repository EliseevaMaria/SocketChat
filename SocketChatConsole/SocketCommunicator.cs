using IO.Encoders;
using IO.IOProviders;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChatConsole
{
    internal class SocketCommunicator
    {
        private readonly IMessageEncoder messageEncoder;
        private readonly ITextIOProvider inputOutputProvider;

        public SocketCommunicator(IMessageEncoder messageEncoder, ITextIOProvider inputOutputProvider)
        {
            this.messageEncoder = messageEncoder;
            this.inputOutputProvider = inputOutputProvider;
        }

        public async Task StartWebSockets()
        {
            var client = new ClientWebSocket();
            await client.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
            this.inputOutputProvider.Write($"web socket connection established @ {DateTime.Now:F}");

            Task send = this.SendAsync(client);
            Task receive = this.ReceiveAsync(client);
            await Task.WhenAll(send, receive);
        }

        private Task SendAsync(ClientWebSocket client) => Task.Run(async () =>
        {
            string message;
            while ((message = this.inputOutputProvider.Read()) != null && message != string.Empty)
                await this.SendMessage(client, message);
            await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        });

        private async Task SendMessage(ClientWebSocket client, string message)
        {
            await client.SendAsync(new ArraySegment<byte>(this.messageEncoder.GetByteMessage(message)), WebSocketMessageType.Text, true, CancellationToken.None);
            this.inputOutputProvider.Write();
        }

        private async Task ReceiveAsync(ClientWebSocket client)
        {
            var buffer = new byte[1024 * 4];
            while (true)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                this.inputOutputProvider.Write(this.messageEncoder.GetStringMessage(buffer, result.Count));
                this.inputOutputProvider.Write();

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    break;
                }
            }
        }
    }
}