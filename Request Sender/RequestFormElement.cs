using System;

namespace Request_Sender
{
    /// <summary>
    /// for request form
    /// </summary>
    public class RequestFormElement : IDisposable
    {
        public string name;
        public string value;
        /// <summary>
        /// for clear in memory
        /// </summary>
        public void Dispose()
        {
            value = null;
            name = null;
        }
        /// <summary>
        /// ini request element
        /// </summary>
        /// <param name="Name">name of the elemwnt</param>
        /// <param name="Value">value of the element</param>
        public RequestFormElement (string Name, string Value)
        {
            value = Value;
            name = Name;
        }
    }
}
