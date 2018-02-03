using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Speercs.Server {
    public class Program {
        public static void main(string[] args) {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}