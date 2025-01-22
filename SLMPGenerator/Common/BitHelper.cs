using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal static class BitHelper
    {

        internal static byte[] ConvertToLittleEndian(ushort value)
        {
            // Windows内部ではマルチバイトデータをリトルエンディアンで扱うのでShort型(2Byte)やInteger型(4Byte)の値をそのままコピーすれば都合よく送受信データと合致する
            return BitConverter.GetBytes(value);//2byte
        }

        internal static byte[] ConvertToLittleEndian(int value)
        {
            // Windows内部ではマルチバイトデータをリトルエンディアンで扱うのでShort型(2Byte)やInteger型(4Byte)の値をそのままコピーすれば都合よく送受信データと合致する
            return BitConverter.GetBytes(value);//4byte
        }

        internal static byte[] ConvertToBigEndian(ushort value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();//2byte
        }

        internal static byte[] ConvertToBigEndian(int value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();//4byte
        }
    }
}
