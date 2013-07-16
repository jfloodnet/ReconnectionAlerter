using ReconnectionAlerter.Core.Infrastructure;
using ReconnectionAlerter.Core.Messages;

namespace ReconnectionAlerter.Core.States
{
    public class ConnectedState : ConnectionState
    {
        private int _version;
        private readonly IPublisher _publisher;

        public ConnectedState(int version, IPublisher publisher)
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
            return this;
        }

        public override ConnectionState Handle(BeenReconnectingForTooLong message)
        {
            return this;
        }
    }
}
