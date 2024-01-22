
using AzBusServiceShare.Models;

namespace AzureDemo.Services
{
    public interface IQueueService
    {
        Task SendMessageAsync<T>(T serviceMessage, string queueName);
    }
}