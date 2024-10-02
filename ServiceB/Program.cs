using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}










// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Confluent.Kafka;
// using Microsoft.AspNetCore.Mvc;

// public class Program
// {
//     public static void Main(string[] args)
//     {
//         CreateHostBuilder(args).Build().Run();
//     }

//     public static IHostBuilder CreateHostBuilder(string[] args) =>
//         Host.CreateDefaultBuilder(args)
//             .ConfigureWebHostDefaults(webBuilder =>
//             {
//                 webBuilder.UseStartup<Startup>();
//             });
// }

// public class Startup
// {
//     public void ConfigureServices(IServiceCollection services)
//     {
//         services.AddControllers();
//         services.AddSingleton<IConsumer<string, string>>(sp =>
//         {
//             var config = new ConsumerConfig
//             {
//                 BootstrapServers = "localhost:9092",
//                 GroupId = "service-b-group",
//                 AutoOffsetReset = AutoOffsetReset.Earliest,
//             };
//             var consumer = new ConsumerBuilder<string, string>(config).Build();
//             return consumer;
//         });
//     }

//     public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//     {
//         if (env.IsDevelopment())
//         {
//             app.UseDeveloperExceptionPage();
//         }
//         app.UseRouting();
//         app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
//     }
// }

// [ApiController]
// [Route("[controller]")]
// public class ConsumeMessageController : ControllerBase
// {
//     private readonly IConsumer<string, string> _consumer;

//     public ConsumeMessageController(IConsumer<string, string> consumer)
//     {
//         _consumer = consumer;
//         Task.Run(() => ConsumeMessages());
//     }

//     private void ConsumeMessages()
//     {
//         _consumer.Subscribe("service-a-topic");
//         while (true)
//         {
//             var cr = _consumer.Consume(CancellationToken.None);
//             Console.WriteLine($"Message consumed: {cr.Value}");
//         }
//     }
// }
