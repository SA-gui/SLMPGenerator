using SLMPGenerator.Mitsubishi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.InternalMemory
{
    internal class ReadCommand
    {
        private byte[] _command = new byte[] { 0x01, 0x04 };
        private byte[] _subCommand;


        internal byte[] BinaryCode { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitLengthType"></param>
        /// <param name="dataRegister"></param>
        /// <param name="deviceQty">読み出し点数</param>
        /// <exception cref="ArgumentException"></exception>
        internal ReadCommand(BitLengthType bitLengthType, DataRegister dataRegister,ushort deviceQty)
        {

            byte[] address;
            byte[] devCode;

            switch(bitLengthType)
            {
                case BitLengthType.Address16bit:
                    _subCommand = new byte[] { 0x00, 0x00 };
                    address = ToSwap3ByteBinary(dataRegister.GetAddress());
                    devCode = dataRegister.GetBinaryCode16bit();
                    break;
                case BitLengthType.Address32bit:
                    _subCommand = new byte[] { 0x02, 0x00 };
                    address = ToSwap4ByteBinary(dataRegister.GetAddress());
                    devCode = dataRegister.GetBinaryCode32bit();
                    break;
                default:
                    throw new ArgumentException("Invalid BitLengthType", nameof(bitLengthType));
            }


            BinaryCode = new byte[] { }
                .Concat(_command)
                .Concat(_subCommand)
                .Concat(address)
                .Concat(devCode)
                .Concat(To2byteBinary(deviceQty))
                .ToArray();
        }
        private byte[] To2byteBinary(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (bytes.Length == 1)
            {
                return new byte[] { 0x00, bytes[0]};
            }

            return BitConverter.GetBytes(value);
        }


        private byte[] ToSwap3ByteBinary(ushort address)
        {
            byte[] bytes = BitConverter.GetBytes(address);

            if (bytes.Length == 2)
            {
                return new byte[] { bytes[0], bytes[1], 0x00 };
            }

            return bytes;
        }

        private byte[] ToSwap4ByteBinary(ushort address)
        {
            byte[] bytes = BitConverter.GetBytes(address);

            if (bytes.Length == 2)
            {
                return new byte[] { bytes[0], bytes[1], 0x00, 0x00 };
            }

            if (bytes.Length == 3)
            {
                return new byte[] { bytes[0], bytes[1], bytes[2], 0x00 };
            }

            return bytes;
        }
    }
}
