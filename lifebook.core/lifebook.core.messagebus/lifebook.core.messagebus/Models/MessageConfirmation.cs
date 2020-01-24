using System;

namespace lifebook.core.messagebus.Models
{
    public class MessageConfirmation
    {
        public bool Successful { get; set; }

        internal static MessageConfirmation Ok()
        {
            return new MessageConfirmation() { Successful = true };
        }
    }
}
