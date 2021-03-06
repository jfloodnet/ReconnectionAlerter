﻿using System;
using ReconnectionAlerter.Core.Common;
using ReconnectionAlerter.Core.Infrastructure;
using ReconnectionAlerter.Core.Messages;

namespace ReconnectionAlerter.Core.States
{
    public class ReconnectingState : ConnectionState
    {
        private int _version;
        private readonly IPublisher _publisher;
        private int _numberOfAttemptedReconnects;

        public ReconnectingState(int version, IPublisher publisher)
        {
            _version = version;
            _publisher = publisher;
            _numberOfAttemptedReconnects = 1;

            _publisher.Publish(
                TimerMessage.Schedule.Create(
                ReconnectionHandlerConfig.Timeout,
                new PublishEnvelope(_publisher),
                replyMessage: new BeenReconnectingForTooLong(_version)));
        }

        public override ConnectionState HandleReconnecting()
        {
            _numberOfAttemptedReconnects++;
            return this;
        }

        public override ConnectionState HandleConnected()
        {
            return new ConnectedState(++_version, _publisher);
        }

        public override ConnectionState Handle(BeenReconnectingForTooLong message)
        {
            if (_version == message.Version)
            {
                _publisher.Publish(new AlertReconnectingForTooLong(_numberOfAttemptedReconnects));
                return new AlertedState(++_version, _publisher);
            }

            return this;
        }
    }
}
