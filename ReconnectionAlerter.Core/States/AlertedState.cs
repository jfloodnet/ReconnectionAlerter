using ReconnectionAlerter.Core.Infrastructure;
using ReconnectionAlerter.Core.Messages;

namespace ReconnectionAlerter.Core.States
{
    public class AlertedState : ConnectionState
    {
        private int _version;
        private readonly IPublisher _publisher;

        public AlertedState(int version, IPublisher publisher)
        {
            _version = version;
            _publisher = publisher;
        }

        public override ConnectionState HandleReconnecting()
        {
            return this;
        }

        public override ConnectionState HandleConnected()
        {
            _publisher.Publish(new AlertFalseAlarm());
            return new ConnectedState(_version, _publisher);
        }

        public override ConnectionState Handle(BeenReconnectingForTooLong message)
        {
            return this;
        }
    }
}
