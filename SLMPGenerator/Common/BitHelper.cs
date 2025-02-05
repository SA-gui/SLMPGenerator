using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal static class BitHelper
    {

        internal static byte[] ConvertToBytesLittleEndian(ushort value)
        {
            // Windows内部ではマルチバイトデータをリトルエンディアンで扱うのでShort型(2Byte)やInteger型(4Byte)の値をそのままコピーすれば都合よく送受信データと合致する
            return BitConverter.GetBytes(value);//2byte
        }

        internal static byte[] ConvertToBytesLittleEndian(int value)
        {
            // Windows内部ではマルチバイトデータをリトルエンディアンで扱うのでShort型(2Byte)やInteger型(4Byte)の値をそのままコピーすれば都合よく送受信データと合致する
            return BitConverter.GetBytes(value);//4byte
        }

        internal static byte[] ConvertToBytesBigEndian(ushort value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();//2byte
        }

        internal static byte[] ConvertToBytesBigEndian(int value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();//4byte
        }
    }
}
