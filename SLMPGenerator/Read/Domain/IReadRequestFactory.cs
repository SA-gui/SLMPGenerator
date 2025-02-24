using SLMPGenerator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Read.Domain
{
    internal interface IReadRequestFactory
    {
        public ReadRequest Create(DeviceAccessType devAccessType, MessageType messageType, string rawAddress, ushort points);

    }
}
