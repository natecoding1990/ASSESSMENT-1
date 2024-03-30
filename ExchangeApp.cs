using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

class ExchangeApp
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: exchange <middleware_endpoint>");
            return;
        }

        string middlewareEndpoint = args[0];

        // Connection string for RabbitMQ server
        var factory = new ConnectionFactory() {HostName = "localhost"};

        // Create a connection to RabbitMQ server
        using (var connection = factory.CreateConnection())
        {
            // Create a channel
            using (var channel = connection.CreateModel())
            {
                // Declare the 'orders' and 'trades' queues
                channel.QueueDeclare(queue: "orders",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.QueueDeclare(queue: "trades",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                // Create a consumer for the 'orders' queue
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine("Received order: {0}", message);

                    // Match orders and publish trades
                    // Implement your logic for matching orders and publishing trades here
                    // For simplicity, let's just forward the order to trades queue
                    
                };

                // Start consuming messages from the 'orders' queue
                channel.BasicConsume(queue: "orders",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Exchange is listening for orders. Press [Enter] to exit.");
                Console.ReadLine(); // Wait for user input before exiting
            }
        }
    }
}


