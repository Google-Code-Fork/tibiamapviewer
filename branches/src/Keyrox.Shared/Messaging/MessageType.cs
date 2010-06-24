using System;
using System.ComponentModel;

namespace Keyrox.Shared.Messaging {
    public enum MessageType {

        [Description("Normal")]
        Normal = 0,

        [Description("Information")]
        Info = 1,

        [Description("Warning")]
        Warning = 2,

        [Description("Error")]
        Error = 3,

    }
}
