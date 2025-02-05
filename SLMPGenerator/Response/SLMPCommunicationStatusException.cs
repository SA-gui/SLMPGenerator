using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Response
{
    [Serializable]
    public class SLMPCommunicationStatusException : Exception
    {
        public SLMPCommunicationStatusException()
        {
        }

        public SLMPCommunicationStatusException(string message)
            : base(message)
        {
        }

        public SLMPCommunicationStatusException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SLMPCommunicationStatusException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
