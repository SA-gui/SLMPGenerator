﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal class SubHeader
    {
        private byte[] _subHeader = new byte[] { 0x00, 0x50 };
        internal byte[] BinaryCode { get; private set; }
        internal string ASCIICode { get; private set; }
        internal SubHeader()
        {
            BinaryCode = _subHeader.Reverse().ToArray();
            ASCIICode = BitConverter.ToString(BinaryCode).Replace("-", "");
        }

        public override int GetHashCode()
        {
            return BinaryCode != null ? BitConverter.ToInt32(BinaryCode, 0) : 0;
        }

        public override bool Equals(object? obj)
        {
            return obj is SubHeader other && BinaryCode.SequenceEqual(other.BinaryCode);
        }

        public static bool operator ==(SubHeader left, SubHeader right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SubHeader left, SubHeader right)
        {
            return !(left == right);
        }
    }
}
