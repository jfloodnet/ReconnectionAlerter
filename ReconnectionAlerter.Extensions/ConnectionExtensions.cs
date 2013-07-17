using System;
using EventStore.ClientAPI;
using ReconnectionAlerter.Core;
using ReconnectionAlerter.Core.Common;
using ReconnectionAlerter.Core.Infrastructure;
using ReconnectionAlerter.Core.Messages;
using ReconnectionAlerter.Email;

namespace ReconnectionAlerter.Extensions
{
    public class ReconnectionAlerterBuilder
    {
        public ReconnectionAlerterBuilder()
        {
            ReconnectionHandlerConfig.Timeout = TimeSpan.FromMinutes(5);
        }

        public ReconnectionAlerterBuilder WithTimeout(TimeSpan timeSpan)
        {
            ReconnectionHandlerConfig.Timeout = timeSpan;
            return this;
        }
        public ReconnectionHandler Build()
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
            var alerter = new ReconnectionAlerterBuilder()
                .WithTimeout(alertAfterReconnectingFor)
                .Build();

            return settings
                .OnConnected(_ => alerter.HandleConnected())
                .OnReconnecting(_ => alerter.HandleReconnecting())
                .KeepReconnecting();
        }
    }
}
