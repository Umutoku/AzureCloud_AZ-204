using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzQueue
{
    class Program
    {

        const string ServiceBusConnectionString = "Endpoint=";
        const string QueueName = "salesmessages";

        static void Main(string[] args)
        {
            Console.WriteLine("Sending a message to the Sales Messages queue...");

            SendSalesMessageAsync().GetAwaiter().GetResult();

            Console.WriteLine("Message was sent successfully.");
        }

        static async Task SendSalesMessageAsync()
        {
            // By leveraging "await using", the DisposeAsync method will be called automatically once the client variable goes out of scope. 
            // In more realistic scenarios, you would want to store off a class reference to the client (rather than a local variable) so that it can be used throughout your program.

            await using var client = new ServiceBusClient(ServiceBusConnectionString);

            await using ServiceBusSender sender = client.CreateSender(QueueName);

            try
            {
                string messageBody = $"$10,000 order for bicycle parts from retailer Adventure Works.";
                var message = new ServiceBusMessage(messageBody);
                Console.WriteLine($"Sending message: {messageBody}");
                await sender.SendMessageAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }

            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
