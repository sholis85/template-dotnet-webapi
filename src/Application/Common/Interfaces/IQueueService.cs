using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Application.Common.Interfaces;
public interface IQueueService
{
    Task<string> GetQueueUrlAsync(string queueName);

    Task<bool> PublishToQueueAsync(string queueUrl, string message);

    Task<List<IQueueMessage>> ReceiveMessageAsync(string queueUrl, int maxMessages = 1);

    Task DeleteMessageAsync(string queueUrl, string id);
}