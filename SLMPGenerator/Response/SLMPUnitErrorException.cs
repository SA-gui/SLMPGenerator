using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Response
{
    [Serializable]
    public class SLMPUnitErrorException : Exception
    {
        public SLMPUnitErrorException() : base() { }

        public SLMPUnitErrorException(string message) : base(message) { }

        public SLMPUnitErrorException(string message, Exception innerException) : base(message, innerException) { }

        protected SLMPUnitErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
