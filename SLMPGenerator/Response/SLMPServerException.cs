using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Response
{
    [Serializable]
    public class SLMPServerException : Exception
    {
        public SLMPServerException() : base()
        {
        }
        public SLMPServerException(string message) : base(message)
        {
        }

        public SLMPServerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SLMPServerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // Custom deserialization logic can be added here if needed
        }


    }
}
