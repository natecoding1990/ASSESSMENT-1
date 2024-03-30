using RabbitMQ.Client;
using System;
using System.Text;

class SendOrderApp
{
    static void Main(string[] args)
    {
        if (args.Length < 5)
         {
            Console.WriteLine("Usage: sendOrder <username> <middleware_endpoint> <SIDE> <QUANTITY> <PRICE>");
            return;
        }

        string username = args[0];
        string middlewareEndpoint = args[1];
        string side = args[2];
        int quantity = Convert.ToInt32(args[3]);
        double price = Convert.ToDouble(args[4]);

        // Create a connection factory
        var factory = new ConnectionFactory() {HostName = "localhost"};

        // Create a connection to RabbitMQ server
        using (var connection = factory.CreateConnection())
        {
            // Create a channel
            using (var channel = connection.CreateModel())
            {
                // Declare the 'orders' queue
                channel.QueueDeclare(queue: "orders",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                // Prepare order message
                string orderMessage = $"{username},{side},{quantity},{price}";
                var body = Encoding.UTF8.GetBytes(orderMessage);

                // Publish the order message to the 'orders' queue
                channel.BasicPublish(exchange: "",
                                     routingKey: "orders",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine("Order sent successfully.");
            }
        }
    }
}



