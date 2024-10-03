using Confluent.Kafka;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConsumer<string, string> _consumerA;
    private readonly IProducer<string, string> _producerA;

    public KafkaConsumerService(string bootstrapServers)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = "service-b-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
        };

        _consumerA = new ConsumerBuilder<string, string>(consumerConfig).Build();
        _producerA = new ProducerBuilder<string, string>(producerConfig).Build();

        _consumerA.Subscribe("service-a-topic");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Started consuming messages from service-a-topic...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumerA.Consume(stoppingToken);
                var message = consumeResult.Message.Value;

                if (message.Contains("ServiceA"))
                {
                    Console.WriteLine($"Message consumed from service-a-topic: {message}");

                    
                    await _producerA.ProduceAsync("service-b-topic", new Message<string, string> { Value = message });
                    Console.WriteLine($"Message replicated to service-b-topic: {message}");
                }
            }
            catch (ConsumeException ex)
            {
                Console.WriteLine($"Error consuming message: {ex.Error.Reason}");
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Error producing message: {ex.Error.Reason}");
            }

            await Task.Delay(500, stoppingToken);
        }
    }

    public override void Dispose()
    {
        _consumerA.Close();
        _consumerA.Dispose();
        _producerA.Dispose();
        base.Dispose();
    }
}
