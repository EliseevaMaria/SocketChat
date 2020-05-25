using System.Diagnostics;

namespace SocketChatConsole
{
    public sealed class ConsoleProcessStarter
    {
        public void CreateComminucationConsoles(int communicationConsolesAmount)
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length >= communicationConsolesAmount)
                return;

            new Process
            {
                StartInfo = new ProcessStartInfo("SocketChatConsole.exe", "-n")
                {
                    UseShellExecute = true
                }
            }.Start();
        }
    }
}