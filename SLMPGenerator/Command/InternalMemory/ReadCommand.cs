using SLMPGenerator.Common;
using SLMPGenerator.Register.Mitsubishi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command.InternalMemory
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
        internal ReadCommand(BitLengthType bitLengthType, DataRegister dataRegister, ushort deviceQty)
        {

            byte[] address;
            byte[] devCode;

            switch (bitLengthType)
            {
                case BitLengthType.Address16bit:
                //case BitLengthType.Address32bit://データレジスタは16bitと兼用する
                    _subCommand = new byte[] { 0x00, 0x00 };
                    address = BitHelper.ConvertToLittleEndian((int)dataRegister.GetAddress()).Take(3).ToArray();//先頭3byte取得
                    devCode = dataRegister.GetBinaryCode16bit();
                    break;
                case BitLengthType.Address32bit:
                    _subCommand = new byte[] { 0x02, 0x00 };
                    address = BitHelper.ConvertToLittleEndian((int)dataRegister.GetAddress());
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
                .Concat(BitHelper.ConvertToLittleEndian(deviceQty))
                .ToArray();
        }



    }
}
