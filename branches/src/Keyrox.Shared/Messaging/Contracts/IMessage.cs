using System;
using System.Xml.Serialization;

namespace Keyrox.Shared.Messaging.Contracts {
    public interface IMessage {

        [XmlAttribute]
        MessageType Type { get; set; }

        [XmlAttribute]
        string ApplicationName { get; set; }

        [XmlAttribute]
        string Title { get; set; }

        [XmlAttribute]
        string Content { get; set; }

        [XmlAttribute]
        DateTime SentDate { get; }

    }
}
