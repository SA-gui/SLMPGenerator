using SLMPGenerator.Common;
using SLMPGenerator.Device.Mitsubishi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command
{
    internal class DeviceReadRequest : IRequestData
    {
        public byte[] BinaryCode { get; private set; }
        public string ASCIICode { get; private set; }

        public byte[] Command { get; private set; } = (new byte[] { 0x04, 0x01 }).Reverse().ToArray();

        public byte[] SubCommand { get; private set; }

        public byte[] Data { get; private set; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSize"></param>
        /// <param name="dataRegister"></param>
        /// <param name="numberOfDevicePoints">読み出し点数</param>
        /// <exception cref="ArgumentException"></exception>
        internal DeviceReadRequest(DataSizeType dataSize, DataRegister dataRegister, ushort numberOfDevicePoints)
        {
            //dataregisterはワード単位で読み出しなのでサブコマンドは2種類
            //DataRegister以外はオーバーロードで対応する
            switch (dataSize)
            {
                case DataSizeType.OneByte:
                    SubCommand = new byte[] { 0x00, 0x00 }.Reverse().ToArray();
                    break;
                case DataSizeType.TwoBytes:
                    SubCommand = new byte[] { 0x00, 0x02 }.Reverse().ToArray();
                    break;
                default:
                    throw new ArgumentException("Invalid BitLengthType", nameof(dataSize));
            }


            Data = new byte[] { }
                .Concat(dataRegister.GetBinalyAddress(dataSize))
                .Concat(dataRegister.GetBinaryCode(dataSize))
                .Concat(BitHelper.ConvertToBytesLittleEndian(numberOfDevicePoints))
                .ToArray();


            BinaryCode = new byte[] { }
                .Concat(Command)
                .Concat(SubCommand)
                .Concat(Data)
                .ToArray();

            ASCIICode = BitConverter.ToString(Command.Reverse().ToArray()).Replace("-", "")
                                + BitConverter.ToString(SubCommand.Reverse().ToArray()).Replace("-", "")
                                + dataRegister.GetASCIICode(dataSize)
                                + dataRegister.GetASCIIAddress(dataSize)
                                
                                + BitConverter.ToString(BitHelper.ConvertToBytesBigEndian(numberOfDevicePoints)).Replace("-", "");
        }



    }
}
