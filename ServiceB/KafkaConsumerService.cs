using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IProducer<string, string> _producer;

    public KafkaConsumerService()
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "service-b-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        _producer = new ProducerBuilder<string, string>(producerConfig).Build();

        _consumer.Subscribe("service-a-topic");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Started consuming messages...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                Console.WriteLine($"Message consumed from Source: {message}");

                // Replicating message to service-b-topic
                var producerResult = await _producer.ProduceAsync("service-b-topic", new Message<string, string> { Value = message });
                Console.WriteLine($"Message replicated to Consumer Topic: {message}");
            }
            catch (ConsumeException ex)
            {
                Console.WriteLine($"Error consuming message: {ex.Error.Reason}");
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Error producing message: {ex.Error.Reason}");
            }

            await Task.Delay(500, stoppingToken); // Delay to avoid tight loop
        }
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        _producer.Dispose();
        base.Dispose();
    }
}















// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using Confluent.Kafka;
// using Microsoft.Extensions.Hosting;

// public class KafkaConsumerService : BackgroundService
// {
//     private readonly IConsumer<string, string> _consumer;

//     public KafkaConsumerService()
//     {
//         var config = new ConsumerConfig
//         {
//             BootstrapServers = "localhost:9092",
//             GroupId = "service-b-group",
//             AutoOffsetReset = AutoOffsetReset.Earliest,
//         };

//         _consumer = new ConsumerBuilder<string, string>(config).Build();
//         _consumer.Subscribe("service-a-topic");
//     }

//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         Console.WriteLine("Started consuming messages...");

//         while (!stoppingToken.IsCancellationRequested)
//         {
//             try
//             {
//                 var consumeResult = _consumer.Consume(stoppingToken);
//                 Console.WriteLine($"Message consumed: {consumeResult.Message.Value}");
                
//             }
//             catch (ConsumeException ex)
//             {
//                 Console.WriteLine($"Error consuming message from Source : {ex.Error.Reason}");
//             }

//             await Task.Delay(500, stoppingToken); // Delay to avoid tight loop
//         }
//     }

//     public override void Dispose()
//     {
//         _consumer.Close();
//         _consumer.Dispose();
//         base.Dispose();
//     }
// }
