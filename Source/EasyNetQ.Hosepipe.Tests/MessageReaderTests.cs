﻿// ReSharper disable InconsistentNaming

using System;
using NUnit.Framework;

namespace EasyNetQ.Hosepipe.Tests
{
    [TestFixture]
    public class MessageReaderTests
    {
        private IMessageReader messageReader;
        private IConventions conventions;

        [SetUp]
        public void SetUp()
        {
            conventions = new Conventions(new TypeNameSerializer());
            messageReader = new MessageReader();
        }

        /// <summary>
        /// 1. Put some messages in C:\temp\MessageOutput
        /// 2. Run this test
        /// 3. Check the output, you should see your messages.
        /// </summary>
        [Test, Explicit(@"Needs message files in 'C:\temp\MessageOutput'")]
        public void Should_be_able_to_read_messages_from_disk()
        {
            var parameters = new QueueParameters
            {
                MessageFilePath = @"C:\temp\MessageOutput"
            };

            var messages = messageReader.ReadMessages(parameters);
            foreach (var message in messages)
            {
                Console.WriteLine("\nBody:\n{0}\n", message.Body);
                Console.WriteLine("\nProperties:\n{0}\n", message.Properties);
                Console.WriteLine("\nInfo exchange:\n{0}", message.Info.Exchange);
                Console.WriteLine("Info routing key:\n{0}\n", message.Info.RoutingKey);
            }
        }

        [Test, Explicit(@"Needs message files in 'C:\temp\MessageOutput'")]
        public void Should_be_able_to_read_only_error_messages()
        {
            var parameters = new QueueParameters
            {
                MessageFilePath = @"C:\temp\MessageOutput"
            };

            var messages = messageReader.ReadMessages(parameters, conventions.ErrorQueueNamingConvention(new MessageReceivedInfo()));
            foreach (var message in messages)
            {
                Console.WriteLine(message.Body);
            }
        }
    }
}

// ReSharper restore InconsistentNaming