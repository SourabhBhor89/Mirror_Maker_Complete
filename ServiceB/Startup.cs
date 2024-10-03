
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

 services.AddSingleton<ServiceB>(sp => new ServiceB("localhost:9092"));

        services.AddHostedService(provider => new KafkaConsumerService("localhost:9092"));


    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
