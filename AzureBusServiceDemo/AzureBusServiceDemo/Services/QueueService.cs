using System.Text;
using System.Text.Json;
using AzBusServiceShare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;

namespace AzureDemo.Services
{
    public class QueueService : IQueueService
    {
        public readonly IConfiguration _config;

        public QueueService(IConfiguration config)
        {
            _config = config;
        }
        
        [HttpPost]
        public async Task SendMessageAsync<T>(T serviceMessage, string queueName)
        {
            var queClient = new QueueClient(_config.GetConnectionString("AzureServiceBus"), queueName);
            var messageBody = JsonSerializer.Serialize(serviceMessage);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await queClient.SendAsync(message);
        }
    }
}
