using System.Text;
using System.Text.Json;
using AzBusServiceShare.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.IdentityModel.Tokens;

const string connectionString = "Endpoint=sb://servicebusdemo-1-basic.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Zep9uzJ8YmKME2QEok4Qxd9BO/0ZezFLq+ASbKH6+m0=";
const string queueName = "personqueue";
QueueClient queueClient = new QueueClient(connectionString, queueName);

var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceiveHandler)
{
    MaxConcurrentCalls = 1,
    AutoComplete = false
};
queueClient.RegisterMessageHandler(ProcessMessageAsync,messageHandlerOptions);
Console.ReadLine();
await queueClient.CloseAsync();
async Task ProcessMessageAsync(Message message, CancellationToken token)
{
    string jsonString = Encoding.UTF8.GetString(message.Body).ToString();
    PersonModal personModel = JsonSerializer.Deserialize<PersonModal>(jsonString) ?? new PersonModal();
    Console.WriteLine($"First name is {personModel.FirstName} and last name is {personModel.LastName}");
    await queueClient.CompleteAsync(message.SystemProperties.LockToken);
}

static Task ExceptionReceiveHandler(ExceptionReceivedEventArgs arg)
{
    Console.WriteLine($"Message Handler Exception {arg.Exception}");
    return Task.CompletedTask;
}
