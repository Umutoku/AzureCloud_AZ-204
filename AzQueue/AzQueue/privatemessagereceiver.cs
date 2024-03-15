using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzQueue
{
    internal class privatemessagereceiver
    {
        class Program
        {

            const string ServiceBusConnectionString = "Endpoint=";
            const string QueueName = "salesmessages";

            static void Main(string[] args)
            {

                ReceiveSalesMessageAsync().GetAwaiter().GetResult();

            }

            static async Task ReceiveSalesMessageAsync()
            {

                Console.WriteLine("======================================================");
                Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
                Console.WriteLine("======================================================");


                var client = new ServiceBusClient(ServiceBusConnectionString);

                var processorOptions = new ServiceBusProcessorOptions
                {
                    MaxConcurrentCalls = 1,
                    AutoCompleteMessages = false
                };

                await using ServiceBusProcessor processor = client.CreateProcessor(QueueName, processorOptions);

                processor.ProcessMessageAsync += MessageHandler;
                processor.ProcessErrorAsync += ErrorHandler;

                await processor.StartProcessingAsync();
                Console.Read();

                await processor.CloseAsync();
            }

            // handle received messages
            static async Task MessageHandler(ProcessMessageEventArgs args)
            {
                string body = args.Message.Body.ToString();
                Console.WriteLine($"Received: {body}");

                // complete the message. messages is deleted from the queue. 
                await args.CompleteMessageAsync(args.Message);
            }

            // handle any errors when receiving messages
            static Task ErrorHandler(ProcessErrorEventArgs args)
            {
                Console.WriteLine(args.Exception.ToString());
                return Task.CompletedTask;
            }
        }

    }
}
