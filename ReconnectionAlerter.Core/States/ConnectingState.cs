using ReconnectionAlerter.Core.Infrastructure;
using ReconnectionAlerter.Core.Messages;

namespace ReconnectionAlerter.Core.States
{
    public class ConnectingState : ConnectionState
    {
        private int _version;
        private readonly IPublisher _publisher;

        public ConnectingState(int version, IPublisher publisher)
        {
            _version = version;
            _publisher = publisher;
        }

        public override ConnectionState HandleReconnecting()
        {
            return new ReconnectingState(++_version, _publisher);
        }

        public override ConnectionState HandleConnected()
        {
            return new ConnectedState(++_version, _publisher);
        }

        public override ConnectionState Handle(BeenReconnectingForTooLong message)
        {
            return this;
        }
    }
}
