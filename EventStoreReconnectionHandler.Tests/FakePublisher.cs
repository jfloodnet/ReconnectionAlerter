using EventStoreReconnectionHandler.Infrastructure;
using System.Collections.Generic;

namespace EventStoreReconnectionHandler.Tests
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