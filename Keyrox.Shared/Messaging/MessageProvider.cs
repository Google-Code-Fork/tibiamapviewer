using System;
using System.Messaging;
using Keyrox.Shared.Messaging.Contracts;

namespace Keyrox.Shared.Messaging {
    public class MessageProvider {

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProvider"/> class.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        public MessageProvider(string applicationName) {
            Initialize(applicationName);
        }

        #region "[rgn] Public Properties   "
        private MessageQueue Queue { get; set; }
        public string ApplicationName { get; private set; }
        public bool IsConnected { get; set; }
        public bool CanRead { get { return Queue != null ? Queue.CanRead : false; } }
        public bool CanWrite { get { return Queue != null ? Queue.CanWrite : false; } }
        #endregion

        #region "[rgn] Provider Singleton  "
        private static MessageProvider current;
        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        public static MessageProvider Current {
            get {
                if (current == null) {
                    current = new MessageProvider(System.Reflection.Assembly.GetCallingAssembly().GetName().Name);
                }
                return current;
            }
        }
        #endregion

        /// <summary>
        /// Initializes the specified application name.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        private void Initialize(string applicationName) {
            ApplicationName = applicationName;
            string queueName = string.Format(@"SRV-MESSAGING\", applicationName);
            if (MessageQueue.Exists(queueName)) {
                Queue = new MessageQueue(queueName);
            }
            else { Queue = MessageQueue.Create(queueName, false); }
            IsConnected = true;
        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        public IMessage Read() {
            if (CanRead) {
                var message = Queue.Receive();
                message.Formatter = new System.Messaging.XmlMessageFormatter(new Type[1] { typeof(IMessage) });
                return (IMessage)message.Body;
            }
            return null;
        }

        /// <summary>
        /// Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Write(IMessage message) {
            if (CanWrite) {
                Queue.Send(message);
            }
        }


    }
}
