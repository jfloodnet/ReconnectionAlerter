using EventStore.ClientAPI;
using ReconnectionAlerter.Core;
using ReconnectionAlerter.Core.Infrastructure;
using ReconnectionAlerter.Core.Messages;
using ReconnectionAlerter.Email;

namespace ReconnectionAlerter.Extensions
{
    public static class ConnectionExtensions
    {
        public static ConnectionSettings KeepReconnectingWithAlerts(this ConnectionSettingsBuilder settings)
        {
            var outputBus = new InMemoryBus("OutputBus");
            var mainQueue = new QueuedHandler(outputBus, "Main Queue");

            // TIMER
            var timer = new TimerService(new ThreadBasedScheduler(new RealTimeProvider()));
            outputBus.Subscribe(timer);

            var alerter = new Alerter();
            outputBus.Subscribe<AlertReconnectingForTooLong >(alerter);
            outputBus.Subscribe<AlertFalseAlarm>(alerter);

            var connectionHandler = new ReconnectionHandler(mainQueue);
            outputBus.Subscribe(connectionHandler);
            mainQueue.Start();

            return settings
                .OnConnected(_ => connectionHandler.HandleConnected())
                .OnReconnecting(_ => connectionHandler.HandleReconnecting())
                .KeepReconnecting();
        }
    }
}
