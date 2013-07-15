using EventStoreReconnectionHandler.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EventStoreReconnectionHandler.Tests
{
    public static class AssertionHelpers
    {
        public static void ShouldContainOnly<T>(this IList<Message> messages, Func<T, bool> isExpected = null) where T : Message
        {
            Assert.Equal(1, messages.Count);
            ShouldContain(messages, isExpected);
        }
        public static void ShouldContain<T>(this IList<Message> messages, Func<T, bool> isExpected = null) where T : Message
        {
            Assert.True(messages.Any(m =>
                {
                    return m is T && (isExpected == null || isExpected(m as T));
                }), "Did not contain expected message");
        }

        public static void ShouldBeEmpty(this List<object> messages)
        {
            Assert.Equal(0, messages.Count);
        }
    }
}