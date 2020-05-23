using System.Diagnostics;

namespace SocketChatConsole
{
    public sealed class ConsoleProcessStarter
    {
        readonly int communicationConsolesAmount;

        public ConsoleProcessStarter(int communicationConsolesAmount)
        {
            this.communicationConsolesAmount = communicationConsolesAmount;
        }

        public void CreateComminucationConsoles()
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length >= this.communicationConsolesAmount)
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