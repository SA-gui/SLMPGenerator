using SLMPGenerator.Command;
using SLMPGenerator.Common;
using SLMPGenerator.Device.Mitsubishi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.UseCase
{
    public class MessageCreater
    {
        //SingleTransmission
        public static byte[] STMessageCreate(
            MessageType messageType,
            PLCType plcType,
            ushort reqNetWorkNo,
            ushort reqStationNo,
            RequestDestModuleIOType reqIOType,
            ushort multiDropStationNo,
            double timerSec,
            CommandType commandType,
            DeviceType deviceType,
            ushort targetDeviceStartAddress,
            ushort targetNumberOfDevicePoints)
        {
            ValidateRequestStationNo(reqNetWorkNo, reqStationNo);

            DataSizeType bitLengthType;

            switch (plcType)
            {
                case PLCType.Mitsubishi_Q_Series:
                    bitLengthType = DataSizeType.OneByte;
                    break;
                case PLCType.Mitsubishi_R_Series:
                    bitLengthType = DataSizeType.TwoBytes;
                    break;
                default:
                    throw new ArgumentException("Invalid PLC Type", nameof(plcType));
            }

            IDevice register;

            switch (deviceType)
            {
                case DeviceType.DataRegister:
                    register = new DataRegister(targetDeviceStartAddress);
                    break;
                default:
                    throw new ArgumentException("Invalid DeviceType", nameof(deviceType));
            }

            IRequestData reqData;

            switch (commandType)
            {
                case CommandType.Device_Read:
                    reqData = new DeviceReadRequest(bitLengthType, (DataRegister)register, targetNumberOfDevicePoints);
                    break;
                case CommandType.Device_Write:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException("Invalid CommandType", nameof(commandType));
            }

            MonitoringTimer monitoringTimer = new MonitoringTimer(reqIOType, timerSec);


            switch(messageType)
            {
                case MessageType.Binary:
                    return new byte[] { }
                            .Concat(new SubHeader().BinaryCode)//subheader
                            .Concat(new RequestDestNetworkNo(reqNetWorkNo).BinaryCode)//networkNo
                            .Concat(new RequestDestStationNo(reqStationNo).BinaryCode)//stationNo
                            .Concat(new RequestDestModuleIONo(reqIOType).BinaryCode)//IONo
                            .Concat(new RequestDestMultiDropStationNo(multiDropStationNo).BinaryCode)//multiDropStationNo
                            .Concat(new RequestDataLength(monitoringTimer, reqData).BinaryCode)//dataLength
                            .Concat(monitoringTimer.BinaryCode)
                            .Concat(reqData.BinaryCode)
                            .ToArray();
                case MessageType.ASCII:
                    string asciiCode = new StringBuilder()
                            .Append(new SubHeader().ASCIICode)
                            .Append(new RequestDestNetworkNo(reqNetWorkNo).ASCIICode)
                            .Append(new RequestDestStationNo(reqStationNo).ASCIICode)
                            .Append(new RequestDestModuleIONo(reqIOType).ASCIICode)
                            .Append(new RequestDestMultiDropStationNo(multiDropStationNo).ASCIICode)
                            .Append(new RequestDataLength(monitoringTimer, reqData).ASCIICode)
                            .Append(monitoringTimer.ASCIICode)
                            .Append(reqData.ASCIICode)
                            .ToString();
                    return Encoding.ASCII.GetBytes(asciiCode);
                default:
                    throw new ArgumentException("Invalid MessageType", nameof(messageType));
            }

        }







        private static void ValidateRequestStationNo(ushort reqNetWorkNo, ushort reqStationNo)
        {
            if (!(reqNetWorkNo == 0 && reqStationNo == 255))
            {
                throw new ArgumentException("Invalid combination of reqNetWorkNo and reqStationNo");
            }
        }
    }
}
