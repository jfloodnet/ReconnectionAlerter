using ReconnectionAlerter.Core.Infrastructure;

namespace ReconnectionAlerter.Core.Messages
{
    public class BeenReconnectingForTooLong : Message
    {
        public readonly int Version;

        public BeenReconnectingForTooLong(int version)
        {
            Version = version;
        }
    }

    public class AlertReconnectingForTooLong : Message
    {
        public readonly int NumberOfAttemptedReconnects;

        public AlertReconnectingForTooLong(int numberOfAttemptedReconnects)
        {
            NumberOfAttemptedReconnects = numberOfAttemptedReconnects;
        }
    }

    public class AlertFalseAlarm : Message
    {

    }
}
