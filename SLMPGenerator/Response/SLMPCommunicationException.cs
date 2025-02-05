using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Response
{
    [Serializable]
    public class SLMPCommunicationException : Exception
    {
        public SLMPCommunicationException()
        {
        }

        public SLMPCommunicationException(string message)
            : base(message)
        {
        }

        public SLMPCommunicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SLMPCommunicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
