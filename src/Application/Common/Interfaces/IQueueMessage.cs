namespace de.WebApi.Application.Common.Interfaces;

public interface IQueueMessage
{
    string Body { get; init; }
    string Handle { get; init; }
    string MessageId { get; init; }
}