// Bağımlılıklar
using Azure.Messaging.ServiceBus;

// Bağlantı dizesi ve kuyruk adını ayarlayın
string connectionString = "[Bağlantı Dizisi]";
string queueName = "[Kuyruk Adı]";

// Service Bus istemcisi oluşturun
ServiceBusClient client = new ServiceBusClient(connectionString);

// Kuyruk göndericisi oluşturun
ServiceBusSender sender = client.CreateSender(queueName);

// Mesaj oluşturun
string messageBody = "Merhaba, dünya!";
ServiceBusMessage message = new ServiceBusMessage(messageBody);

// Mesajı gönderin
await sender.SendMessageAsync(message);

// Kuyruk alıcısı oluşturun
ServiceBusReceiver receiver = client.CreateReceiver(queueName);

// Mesajı alın
ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

// Mesaj gövdesini yazdırın
Console.WriteLine(receivedMessage.Body);

// Mesajı tamamlayın
await receiver.CompleteMessageAsync(receivedMessage);

// Alıcıyı ve göndericiyi kapatın
await receiver.CloseAsync();
await sender.CloseAsync();

// İstemciyi kapatın
await client.DisposeAsync();
