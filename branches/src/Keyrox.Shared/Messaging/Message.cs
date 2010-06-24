using System;
using System.Xml.Serialization;
using Keyrox.Shared.Messaging.Contracts;

namespace Keyrox.Shared.Messaging {
    [Serializable]
    public class Message : IMessage {

        public Message() {
            SentDate = DateTime.Now;
        }

        [XmlAttribute]
        public MessageType Type { get; set; }

        [XmlAttribute]
        public string ApplicationName { get; set; }

        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string Content { get; set; }

        [XmlAttribute]
        public DateTime SentDate { get; private set; }

    }
}
