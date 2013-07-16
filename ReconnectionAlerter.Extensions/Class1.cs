using EventStore.ClientAPI;
using EventStoreReconnectionHandler;
using EventStoreReconnectionHandler.Infrastructure;

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

            var connectionHandler = new ReconnectionHandler(mainQueue);
            outputBus.Subscribe(connectionHandler);

            settings.OnConnected(_ => connectionHandler.HandleConnected())
                    .OnReconnecting(_ => connectionHandler.HandleReconnecting());

            mainQueue.Start();

            return settings.KeepReconnecting();
        }
    }
}
