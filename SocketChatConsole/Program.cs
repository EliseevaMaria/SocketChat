using System;

namespace SocketChatConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int consolesCount = 2;
            new ConsoleProcessStarter().CreateComminucationConsoles(consolesCount);
            new SocketCommunicator().StartWebSockets().GetAwaiter().GetResult();
        }
    }
}