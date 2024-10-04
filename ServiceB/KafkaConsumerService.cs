using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConsumer<string, string> _consumerA;
    private readonly IProducer<string, string> _producerA;
    private readonly ILogger<KafkaConsumerService> _logger;

    public KafkaConsumerService(ILogger<KafkaConsumerService> logger)
    {
        _logger = logger;

        var consumerConfigA = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "service-b-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        _consumerA = new ConsumerBuilder<string, string>(consumerConfigA).Build();
        _producerA = new ProducerBuilder<string, string>(producerConfig).Build();

        _consumerA.Subscribe("service-a-topic");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Started consuming messages from service-a-topic...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = await Task.Run(() => _consumerA.Consume(stoppingToken), stoppingToken);
                var message = consumeResult.Message.Value;

                if (message.Contains("ServiceA"))
                {
                    _logger.LogInformation($"Message consumed from service-a-topic: {message}");

                    await _producerA.ProduceAsync("service-b-topic", new Message<string, string> { Value = $"Response - {message}" });
                    _logger.LogInformation($"Message replicated to service-b-topic: {message}");
                }
            }
            catch (ConsumeException ex)
            {
                _logger.LogError($"Error consuming message: {ex.Error.Reason}");
            }
            catch (ProduceException<string, string> ex)
            {
                _logger.LogError($"Error producing message: {ex.Error.Reason}");
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









// using Confluent.Kafka;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;

// public class KafkaConsumerService : BackgroundService
// {
//     private readonly IConsumer<string, string> _consumerA;
//     private readonly IProducer<string, string> _producerA;
//     private readonly ILogger<KafkaConsumerService> _logger;

   
//     public KafkaConsumerService(string bootstrapServers, ILogger<KafkaConsumerService> logger)
//     {
//         // Set up the logger and Kafka consumers
//         _logger = logger;
        
//         var consumerConfig = new ConsumerConfig
//         {
//             BootstrapServers = bootstrapServers,
//             GroupId = "service-b-group",
//             AutoOffsetReset = AutoOffsetReset.Earliest,
//         };

//         var producerConfig = new ProducerConfig
//         {
//             BootstrapServers = bootstrapServers
//         };

//         _consumerA = new ConsumerBuilder<string, string>(consumerConfig).Build();
//         _producerA = new ProducerBuilder<string, string>(producerConfig).Build();

//         // Subscribe to the Kafka topic
//         _consumerA.Subscribe("service-a-topic");
//     }

//     // Execute method to process messages
//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         _logger.LogInformation("Started consuming messages from service-a-topic...");

//         while (!stoppingToken.IsCancellationRequested)
//         {
//             try
//             {
//                 var consumeResult = _consumerA.Consume(stoppingToken);
//                 var message = consumeResult.Message.Value;

//                 if (message.Contains("ServiceA"))
//                 {
//                     _logger.LogInformation($"Message consumed from service-a-topic: {message}");

//                     await _producerA.ProduceAsync("service-b-topic", new Message<string, string> { Value = $"Response - {message}" });
//                     _logger.LogInformation($"Message replicated to service-b-topic: {message}");
//                 }
//             }
//             catch (ConsumeException ex)
//             {
//                 _logger.LogError($"Error consuming message: {ex.Error.Reason}");
//             }
//             catch (ProduceException<string, string> ex)
//             {
//                 _logger.LogError($"Error producing message: {ex.Error.Reason}");
//             }

//             await Task.Delay(500, stoppingToken);
//         }
//     }

//     public override void Dispose()
//     {
//         _consumerA.Close();
//         _consumerA.Dispose();
//         _producerA.Dispose();
//         base.Dispose();
//     }
// }