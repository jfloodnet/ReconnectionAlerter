using System.Collections.Generic;
using ReconnectionAlerter.Core.Infrastructure;

namespace ReconnectionAlerter.Tests
{
    public class FakePublisher : IPublisher
    {
        public IList<Message> PublishedMessages= new List<Message>();
        public void Publish(Message message)
        {
            PublishedMessages.Add(message);
        }

        public void Clear()
        {
            PublishedMessages.Clear();
        }
    }
}