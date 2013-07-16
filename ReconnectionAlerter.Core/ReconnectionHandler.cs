using ReconnectionAlerter.Core.Infrastructure;
using ReconnectionAlerter.Core.Messages;
using ReconnectionAlerter.Core.States;

namespace ReconnectionAlerter.Core
{
    public class ReconnectionHandler : IHandle<BeenReconnectingForTooLong>
    {
        private ConnectionState _state;

        public ReconnectionHandler(IPublisher publisher)
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
