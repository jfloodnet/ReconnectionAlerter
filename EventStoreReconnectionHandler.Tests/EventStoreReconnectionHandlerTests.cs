using Xunit;
using Should;
using EventStoreReconnectionHandler.Messages;

namespace EventStoreReconnectionHandler.Tests
{
    public class EventStoreReconnectionHandlerTests
    {
        private FakePublisher fakePublisher;
        private ReconnectionHandler sut;        

        public EventStoreReconnectionHandlerTests()
        {
            //Arrange
            fakePublisher = new FakePublisher();
            sut = new ReconnectionHandler(fakePublisher);
        }

        [Fact]
        public void When_reconnecting_handling_timeout_should_send_alert()
        {     
            sut.HandleReconnecting();
            fakePublisher.Clear();

            //Act 
            sut.Handle(new BeenReconnectingForTooLong(1));

            //Assert
            fakePublisher.PublishedMessages.ShouldContainOnly<AlertReconnectingForTooLong>();
        }

        [Fact]
        public void When_connected_handling_timeout_should_not_send_alert()
        {
            //Arrange
            sut.HandleReconnecting();
            sut.HandleConnected();
            fakePublisher.Clear();

            //Act
            sut.Handle(new BeenReconnectingForTooLong(1));

            //Assert
            fakePublisher.PublishedMessages.ShouldBeEmpty();
        }

        [Fact]
        public void When_reconnecting_handling_timeout_for_previous_reconnect_should_not_send_alert()
        {
            //Arrange
            sut.HandleConnected();
            sut.HandleReconnecting();
            sut.HandleConnected();
            sut.HandleReconnecting();
            fakePublisher.Clear();

            //Act
            sut.Handle(new BeenReconnectingForTooLong(1));

            //Assert
            fakePublisher.PublishedMessages.ShouldBeEmpty();
        }

        [Fact]
        public void When_timeout_received_for_current_reconnect_should_send_alert()
        {
            //Arrange
            sut.HandleConnected();
            sut.HandleReconnecting();
            sut.HandleConnected();
            sut.HandleReconnecting();
            fakePublisher.Clear();

            //Act
            sut.Handle(new BeenReconnectingForTooLong(4));

            //Assert
            fakePublisher.PublishedMessages.ShouldContainOnly<AlertReconnectingForTooLong>();
        }

        [Fact]
        public void When_connection_achieved_after_sending_an_alert_then_should_send_false_alarm()
        {
            //Arrange
            sut.HandleConnected();
            sut.HandleReconnecting();
            sut.Handle(new BeenReconnectingForTooLong(2));
            fakePublisher.Clear();
            
            //Act
            sut.HandleConnected();

            //Assert
            fakePublisher.PublishedMessages.ShouldContainOnly<AlertFalseAlarm>();
        }

        [Fact]
        public void When_sending_alarm_should_indicate_number_of_attempts_to_reconnect()
        {
            //Arrange
            sut.HandleConnected();
            sut.HandleReconnecting();
            sut.HandleReconnecting();
            sut.HandleReconnecting();
            sut.HandleReconnecting();
            sut.HandleReconnecting();
            fakePublisher.Clear();

            //Act
            sut.Handle(new BeenReconnectingForTooLong(2));

            //Assert
            fakePublisher.PublishedMessages.ShouldContain<AlertReconnectingForTooLong>(x => x.NumberOfAttemptedReconnects == 5);
        }

        [Fact]
        public void When_connection_achieved_number_of_attempted_reconnects_should_be_reset()
        {
            //Arrange
            sut.HandleConnected();
            sut.HandleReconnecting();
            sut.HandleReconnecting();
            sut.HandleReconnecting();
            sut.HandleConnected();
            sut.HandleReconnecting();
            sut.HandleReconnecting();

            //Act
            sut.Handle(new BeenReconnectingForTooLong(4));

            //Assert
            fakePublisher.PublishedMessages.ShouldContain<AlertReconnectingForTooLong>(x => x.NumberOfAttemptedReconnects == 2);
        }
    }
}
