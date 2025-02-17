using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command
{
    internal class DeviceCode
    {

        public byte[] BinaryCode { get; }
        public string ASCIICode { get; }
        internal DeviceType DeviceType { get; private set; }
        internal DeviceNoRange DeviceNoRange { get; private set; }

        public DeviceCode(byte[] binaryCode, string aSCIICode, DeviceType deviceType, DeviceNoRange deviceNoRange)
        {
            BinaryCode = binaryCode.Reverse().ToArray() ?? throw new ArgumentNullException(nameof(binaryCode));
            ASCIICode = aSCIICode ?? throw new ArgumentNullException(nameof(aSCIICode));
            DeviceType = deviceType;
            DeviceNoRange = deviceNoRange;
        }

        public override int GetHashCode()
        {
            return ASCIICode.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is DeviceCode other && BinaryCode.SequenceEqual(other.BinaryCode) && ASCIICode.Equals(other.ASCIICode) && DeviceType.Equals(other.DeviceType) && DeviceNoRange.Equals(other.DeviceNoRange);
        }
    }
}
