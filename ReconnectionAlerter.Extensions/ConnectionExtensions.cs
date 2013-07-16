using System;
using EventStore.ClientAPI;
using ReconnectionAlerter.Core;
using ReconnectionAlerter.Core.Common;
using ReconnectionAlerter.Core.Infrastructure;
using ReconnectionAlerter.Core.Messages;
using ReconnectionAlerter.Email;

namespace ReconnectionAlerter.Extensions
{
    public class ReconnectionAlerterNode
    {
        public ReconnectionHandler Start()
        {
            var outputBus = new InMemoryBus("OutputBus");
            var mainQueue = new QueuedHandler(outputBus, "Main Queue");

            // TIMER
            var timer = new TimerService(new ThreadBasedScheduler(new RealTimeProvider()));
            outputBus.Subscribe(timer);

            //ALERTER
            var alerter = new Alerter();
            outputBus.Subscribe<AlertReconnectingForTooLong>(alerter);
            outputBus.Subscribe<AlertFalseAlarm>(alerter);

            var connectionHandler = new ReconnectionHandler(mainQueue);
            outputBus.Subscribe(connectionHandler);
            mainQueue.Start();
            return connectionHandler;
        }
    }

    public static class ConnectionExtensions
    {
        public static ConnectionSettings KeepReconnectingWithAlerts(this ConnectionSettingsBuilder settings, TimeSpan alertAfterReconnectingFor)
        {
            ReconnectionHandlerConfig.Timeout = alertAfterReconnectingFor;
            var connectionHandler = new ReconnectionAlerterNode().Start();

            return settings
                .OnConnected(_ => connectionHandler.HandleConnected())
                .OnReconnecting(_ => connectionHandler.HandleReconnecting())
                .KeepReconnecting();
        }
    }
}
