using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStoreReconnectionHandler.Messages;

namespace EventStoreReconnectionHandler
{
    public abstract class ConnectionState
    {
        public abstract ConnectionState HandleReconnecting();
        public abstract ConnectionState HandleConnected();
        public abstract ConnectionState Handle(BeenReconnectingForTooLong message);
    }
}
