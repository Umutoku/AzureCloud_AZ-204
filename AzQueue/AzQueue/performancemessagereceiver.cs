using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzQueue
{
    internal class performancemessagereceiver
    {
        const string ServiceBusConnectionString = "Endpoint=sb://alexgeddyneil.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LIWIyxs8baqQ0bRf5zJLef6OTfrv0kBEDxFM/ML37Zs=";
        const string TopicName = "salesperformancemessages"; 
        const string SubscriptionName = "Americas"; // abonelik adı

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var client = new ServiceBusClient(ServiceBusConnectionString);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");

            var processorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1, // bu örnekte, yalnızca bir mesaj işlenir
                AutoCompleteMessages = false // işleme başarılı olursa mesajın tamamlanması gerektiğini belirtir
            };

            ServiceBusProcessor processor = client.CreateProcessor(TopicName, SubscriptionName, processorOptions);

            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            await processor.StartProcessingAsync();

            Console.Read();

            await processor.DisposeAsync();
            await client.DisposeAsync();
        }

        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            Console.WriteLine($"Received message: SequenceNumber:{args.Message.SequenceNumber} Body:{args.Message.Body}");
            await args.CompleteMessageAsync(args.Message);
        }

        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {args.FullyQualifiedNamespace}");
            Console.WriteLine($"- Entity Path: {args.EntityPath}");
            Console.WriteLine($"- Executing Action: {args.ErrorSource}");
            return Task.CompletedTask;
        }
    }
}
