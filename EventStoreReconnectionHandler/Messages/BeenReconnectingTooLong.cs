using EventStoreReconnectionHandler.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStoreReconnectionHandler.Messages
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
