using System;
using System.Threading;
using ReconnectionAlerter.Core.Common;
using ReconnectionAlerter.Extensions;

namespace ReconnectionAlerter.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionHandler = new ReconnectionAlerterNode().Start();
            ReconnectionHandlerConfig.Timeout = TimeSpan.FromSeconds(5);
            connectionHandler.HandleConnected();
            
            while (!Console.KeyAvailable)
            {
                Console.WriteLine("Attempting Reconnect");
                connectionHandler.HandleReconnecting();
                Thread.Sleep(1000);
            }

            Console.WriteLine("Connection Successful");
            connectionHandler.HandleConnected();
            Console.ReadKey();
            Console.ReadKey();
        }
    }
}
