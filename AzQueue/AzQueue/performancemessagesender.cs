using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzQueue
{
    class performancemessagesender
    {
        const string ServiceBusConnectionString = "Endpoint=sb://example.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=AbCdEfGhIjKlMnOpQrStUvWxYz==";
        const string TopicName = "salesperformancemessages";

        static void Main(string[] args)
        {
            Console.WriteLine("Sending a message to the Sales Performance topic...");
            SendPerformanceMessageAsync().GetAwaiter().GetResult();
            Console.WriteLine("Message was sent successfully.");
        }

        static async Task SendPerformanceMessageAsync()
        {
            // By leveraging "await using", the DisposeAsync method will be called automatically once the client variable goes out of scope.
            // In more realistic scenarios, you would store off a class reference to the client (rather than to a local variable) so that it can be used throughout your program.
            await using var client = new ServiceBusClient(ServiceBusConnectionString);

            await using ServiceBusSender sender = client.CreateSender(TopicName);

            try
            {
                string messageBody = "Total sales for Brazil in August: $13m.";
                var message = new ServiceBusMessage(messageBody);
                Console.WriteLine($"Sending message: {messageBody}");
                await sender.SendMessageAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
