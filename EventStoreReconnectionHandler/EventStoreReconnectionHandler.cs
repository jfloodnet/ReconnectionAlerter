using System;
using EventStoreReconnectionHandler.Infrastructure;
using EventStoreReconnectionHandler.Messages;

namespace EventStoreReconnectionHandler
{
    public class EventStoreReconnectionHandler
    {
        private ConnectionState _state;

        public EventStoreReconnectionHandler(IPublisher publisher)
        {
            _state = new ConnectingState(0, publisher);
        }

        public void HandleReconnecting()
        {
            _state = _state.HandleReconnecting();
        }

        public void HandleConnected()
        {
            _state = _state.HandleConnected();
        }

        public void Handle(BeenReconnectingForTooLong message)
        {
            _state = _state.Handle(message);
        }
    }    
}
