
using EventStoreReconnectionHandler.Infrastructure;
using EventStoreReconnectionHandler.Messages;

namespace ReconnectionAlerter.Email
{
    public class Alerter : 
        IHandle<AlertReconnectingForTooLong>, 
        IHandle<AlertFalseAlarm>
    {
        public void Handle(AlertReconnectingForTooLong message)
        {
            
        }

        public void Handle(AlertFalseAlarm message)
        {
            
        }
    }
}
