using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Response
{
    [Serializable]
    public class SLMPCommandException : Exception
    {
        public SLMPCommandException()
        {
        }

        public SLMPCommandException(string message)
            : base(message)
        {
        }

        public SLMPCommandException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SLMPCommandException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
