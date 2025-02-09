using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal class RequestDestStationNo
    {
        private const ushort MIN_VALUE = 1;
        private const ushort MAX_VALUE = 120;
        private const ushort VALUE1 = 125;
        private const ushort VALUE2 = 126;
        private const ushort VALUE3 = 255;

        internal byte[] BinaryCode { get; private set; }
        internal string ASCIICode { get; private set; }

        internal RequestDestStationNo(ushort stationNo)
        {
            Validate(stationNo);
            BinaryCode = BitConverter.GetBytes(stationNo).Take(1).ToArray();
            ASCIICode = BitHelper.ToString(BinaryCode);
        }


        private void Validate(int stationNo)
        {

            if ((stationNo < MIN_VALUE || stationNo > MAX_VALUE) && stationNo != VALUE1 && stationNo != VALUE2 && stationNo != VALUE3)
            {
                throw new ArgumentException($"Station No must be between {MIN_VALUE}-{MAX_VALUE}, {VALUE1}, or {VALUE2}or {VALUE3}", nameof(stationNo));
            }

        }

        public override int GetHashCode()
        {
            return ASCIICode.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is RequestDestStationNo other && ASCIICode.Equals(other.ASCIICode);
        }


    }
}
