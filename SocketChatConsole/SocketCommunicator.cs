using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChatConsole
{
    internal class SocketCommunicator
    {
        public async Task StartWebSockets()
        {
            var client = new ClientWebSocket();
            await client.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
            Console.WriteLine($"web socket connection established @ {DateTime.Now:F}");

            Task send = this.SendAsync(client);
            Task receive = this.ReceiveAsync(client);
            await Task.WhenAll(send, receive);
        }

        private Task SendAsync(ClientWebSocket client) => Task.Run(async () =>
        {
            string message;
            while ((message = Console.ReadLine()) != null && message != string.Empty)
                await this.SendMessage(client, message);
            await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        });

        private async Task SendMessage(ClientWebSocket client, string message)
        {
            await client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine();
        }

        private async Task ReceiveAsync(ClientWebSocket client)
        {
            var buffer = new byte[1024 * 4];
            while (true)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, result.Count));
                Console.WriteLine();

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    break;
                }
            }
        }
    }
}