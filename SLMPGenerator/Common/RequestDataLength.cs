﻿using SLMPGenerator.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal class RequestDataLength
    {
        internal byte[] BinaryCode { get; private set; }
        internal string ASCIICode { get; private set; }

        internal RequestDataLength(MonitoringTimer monitoringTimer,IRequestData requestData)
        {
            ushort binarydataLength = (ushort)(monitoringTimer.BinaryCode.Length + requestData.BinaryCode.Length);
            BinaryCode = BitHelper.ToBytesLittleEndian(binarydataLength);
            ushort asciiDataLength = (ushort)(monitoringTimer.ASCIICode.Length + requestData.ASCIICode.Length);
            ASCIICode = BitHelper.ToString(BitHelper.ToBytesBigEndian(asciiDataLength));
        }

        public override int GetHashCode()
        {
            return ASCIICode.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            return obj is RequestDataLength other && ASCIICode.Equals(other.ASCIICode);
        }

    }
}
