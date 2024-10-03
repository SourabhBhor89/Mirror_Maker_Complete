using Confluent.Kafka;

public class ServiceB : IDisposable
{
    private readonly IProducer<string, string> _producer;

    public ServiceB(string bootstrapServers)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishMessageAsync(string topic, MessageDto messageDto)
    {
        try
        {
            var payload = $"{{ \"Message\": \"{messageDto.Message}\", \"Origin\": \"{messageDto.Origin}\" }}";

            await _producer.ProduceAsync(topic, new Message<string, string> { Value = payload });
            Console.WriteLine($"Produced message: {payload} to topic: {topic}");
        }
        catch (ProduceException<string, string> ex)
        {
            Console.WriteLine($"Error producing message: {ex.Error.Reason}");
            throw;
        }
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}
