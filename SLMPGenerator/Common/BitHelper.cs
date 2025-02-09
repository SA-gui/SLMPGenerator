using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal static class BitHelper
    {

        internal static byte[] ToBytesLittleEndian(ushort value)
        {
            // Windows内部ではマルチバイトデータをリトルエンディアンで扱うのでShort型(2Byte)やInteger型(4Byte)の値をそのままコピーすれば都合よく送受信データと合致する
            return BitConverter.GetBytes(value);//2byte
        }

        internal static byte[] ToBytesLittleEndian(int value)
        {
            // Windows内部ではマルチバイトデータをリトルエンディアンで扱うのでShort型(2Byte)やInteger型(4Byte)の値をそのままコピーすれば都合よく送受信データと合致する
            return BitConverter.GetBytes(value);//4byte
        }

        internal static byte[] ToBytesBigEndian(ushort value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();//2byte
        }

        internal static byte[] ToBytesBigEndian(int value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();//4byte
        }

        internal static string ToString(byte[] binaryCode)
        {
            return BitConverter.ToString(binaryCode).Replace("-", "");
        }

        internal static string ToReverseString(byte[] binaryCode)
        {
            return BitConverter.ToString(binaryCode.Reverse().ToArray()).Replace("-", "");
        }
    }
}
