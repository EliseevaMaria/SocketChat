using System;

namespace SocketChatConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int consolesCount = 3;
            new ConsoleProcessStarter(consolesCount).CreateComminucationConsoles();
            new SocketCommunicator().StartWebSockets().GetAwaiter().GetResult();
        }
    }
}