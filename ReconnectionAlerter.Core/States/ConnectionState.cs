using ReconnectionAlerter.Core.Messages;

namespace ReconnectionAlerter.Core.States
{
    public abstract class ConnectionState
    {
        public abstract ConnectionState HandleReconnecting();
        public abstract ConnectionState HandleConnected();
        public abstract ConnectionState Handle(BeenReconnectingForTooLong message);
    }
}
