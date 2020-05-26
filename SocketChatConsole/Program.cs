using IO.Encoders;
using IO.IOProviders;

namespace SocketChatConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int consolesCount = 1;
            new ConsoleProcessStarter().CreateComminucationConsoles(consolesCount);

            var messageEncoder = new Utf8MessageEncoder();
            var consoleIOProvider = new ConsoleIOProvider();
            new SocketCommunicator(messageEncoder, consoleIOProvider).StartWebSockets().GetAwaiter().GetResult();
        }
    }
}