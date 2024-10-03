
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Starting Application");
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                 webBuilder.UseUrls("http://localhost:5157");
                    webBuilder.UseStartup<Startup>();
            });
}

