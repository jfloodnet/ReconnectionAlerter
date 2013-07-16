using ReconnectionAlerter.Core.Infrastructure;
using ReconnectionAlerter.Core.Messages;

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
