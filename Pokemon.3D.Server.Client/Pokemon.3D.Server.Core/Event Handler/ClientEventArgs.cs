using System;

namespace Pokemon_3D_Server_Core.Event
{
    /// <summary>
    /// Event Handler for Client.
    /// </summary>
    public class ClientEventArgs : EventArgs
    {
        /// <summary>
        /// Output Type of the event.
        /// </summary>
        public ClientEvent.Types Type { get; set; }

        /// <summary>
        /// Output Value of the event.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// New Client Event Argument.
        /// </summary>
        /// <param name="Type">Type of the event.</param>
        /// <param name="Output">Output value of the event.</param>
        public ClientEventArgs(ClientEvent.Types Type, string Output)
        {
            this.Type = Type;
            this.Output = Output;
        }
    }
}
