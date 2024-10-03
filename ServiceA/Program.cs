
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
                webBuilder.UseStartup<Startup>()
                          .UseUrls("http://localhost:5157"); 
            });
}















// using System;
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
//         var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
//         services.AddSingleton<IProducer<string, string>>(new ProducerBuilder<string, string>(config).Build());
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
// public class MessageController : ControllerBase
// {
//     private readonly IProducer<string, string> _producer;

//     public MessageController(IProducer<string, string> producer)
//     {
//         _producer = producer;
//     }

//     [HttpPost]
//     public async Task<IActionResult> SendMessage([FromBody] string message)
//     {
//         await _producer.ProduceAsync("service-a-topic", new Message<string, string> { Key = null, Value = message });
//         return Ok("Message sent to Service A");
//     }
// }
