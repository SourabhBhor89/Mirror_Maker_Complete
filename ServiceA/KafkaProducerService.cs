using System;
using System.Threading.Tasks;
using Confluent.Kafka;

public class KafkaProducerService
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducerService(string bootstrapServers)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, string message)
    {
        try
        {
            await _producer.ProduceAsync(topic, new Message<string, string> { Value = message });
            Console.WriteLine($"Produced message: {message} to Source topic: {topic}");
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
