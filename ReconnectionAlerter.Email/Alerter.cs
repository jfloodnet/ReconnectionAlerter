using System.Configuration;
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
            var email = new SMTPMailSender();
            email.SendEmail(
                AlerterConfig.From, 
                AlerterConfig.To, 
                Const.AlertSubject,
                string.Format(
                "The EventStore Client in {0} has been attempted to reconnect {1} times. Please have a lookie.",
                    AlerterConfig.Environment, 
                    message.NumberOfAttemptedReconnects));
        }

        public void Handle(AlertFalseAlarm message)
        {
            var email = new SMTPMailSender();
            email.SendEmail(
                AlerterConfig.From, 
                AlerterConfig.To, 
                Const.FalseAlarmSubject, 
                Const.FalseAlarmSubject);
        }
    }

    internal class Const
    {
        public static readonly string AlertSubject = string.Format("{0}: EventStore.Client Reconnection Alert", AlerterConfig.Environment);
        public static readonly string FalseAlarmSubject = string.Format("{0}: False Alarm - Connection has now been achieved ", AlerterConfig.Environment);
        public static readonly string FalseAlarmBody = "Never mind, carry on!";
    }

    internal class AlerterConfig
    {
        public static readonly string From = ConfigurationManager.AppSettings["ReconnectionAlerter.Email.From"];
        public static readonly string To = ConfigurationManager.AppSettings["ReconnectionAlerter.Email.To"];
        public static readonly string Environment = ConfigurationManager.AppSettings["ReconnectionAlerter.Email.Environment"]; 
    }
}
