using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SocketChatServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHostBuilder builder = WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
            builder.Build().Run();
        }
    }
}