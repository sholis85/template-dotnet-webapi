using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.WebApi.Application.Common.Interfaces;
using de.WebApi.Infrastructure.Auth;
using de.WebApi.Shared.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace de.WebApi.Infrastructure.Notifications;

[Authorize]
public class NotificationHub : Hub, ITransientService
{
    private readonly ILogger<NotificationHub> _logger;
    private readonly INotificationSender _notificationSender;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICurrentUserInitializer _currentUserInitializer;

    public NotificationHub(ILogger<NotificationHub> logger, INotificationSender notificationSender, IServiceProvider serviceProvider, ICurrentUserInitializer currentUserInitializer)
    {
        _logger = logger;
        _notificationSender = notificationSender;
        _serviceProvider = serviceProvider;
        _currentUserInitializer = currentUserInitializer;
    }

    private async Task Welcome()
    {
        var cancellationToken = new CancellationToken();
        _currentUserInitializer.SetCurrentUser(Context.User!);
        var currentUser = _serviceProvider.GetService<ICurrentUser>();
        string? mail = currentUser?.GetUserEmail();
        string? fullName = currentUser?.GetFullName();
        await _notificationSender.SendToUserAsync(new BasicNotification() { Label = BasicNotification.LabelType.Information, Message = $"Hello {fullName} ({mail}), your user id is {Context.UserIdentifier}" }, Context.UserIdentifier!, cancellationToken);
    }

    public async Task Ping(string pingMessage)
    {
        var cancellationToken = new CancellationToken();
        await _notificationSender.SendToGroupAsync(new BasicNotification() { Label = BasicNotification.LabelType.Success, Message = $"Pong {pingMessage}" }, $"ExampleGroup", cancellationToken);
    }

    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"ExampleGroup");

        await base.OnConnectedAsync();

        _logger.LogInformation("A client connected to NotificationHub: {connectionId} - {userId}", Context.ConnectionId, Context.UserIdentifier!);

        await Welcome();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"ExampleGroup");

        await base.OnDisconnectedAsync(exception);

        _logger.LogInformation("A client disconnected from NotificationHub: {connectionId}", Context.ConnectionId);
    }
}