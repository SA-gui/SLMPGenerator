using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Response
{
    [Serializable]
    public class SLMPparameterException : Exception
    {
        public SLMPparameterException() : base() { }

        public SLMPparameterException(string message) : base(message) { }

        public SLMPparameterException(string message, Exception innerException) : base(message, innerException) { }

        protected SLMPparameterException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
