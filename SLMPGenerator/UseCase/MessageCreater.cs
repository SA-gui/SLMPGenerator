using SLMPGenerator.InternalMemory;
using SLMPGenerator.Mitsubishi;
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
            PLCType plcType,
            ushort reqNetWorkNo,
            ushort reqStationNo,
            RequestedIOType reqIOType,
            ushort multiDropStationNo,
            double timermilisec,
            CommandType commandType,
            DeviceType deviceType,
            ushort targetDeviceAddress,
            ushort deviceQty)
        {
            ValidateRequestStationNo(reqNetWorkNo, reqStationNo);

            BitLengthType bitLengthType;

            switch (plcType)
            {
                case PLCType.Mitsubishi_Q_Series:
                    bitLengthType = BitLengthType.Address16bit;
                    break;
                case PLCType.Mitsubishi_R_Series:
                    bitLengthType = BitLengthType.Address32bit;
                    break;
                default:
                    throw new ArgumentException("Invalid PLC Type", nameof(plcType));
            }

            IRegister register;

            switch (deviceType)
            {
                case DeviceType.DataRegister:
                    register = new DataRegister(targetDeviceAddress);
                    break;
                default:
                    throw new ArgumentException("Invalid DeviceType", nameof(deviceType));
            }

            byte[] reqData;

            switch (commandType)
            {
                case CommandType.Read:
                    reqData = new ReadCommand(bitLengthType, (DataRegister)register, deviceQty).BinaryCode;
                    break;
                case CommandType.Write:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException("Invalid CommandType", nameof(commandType));
            }

            WatchdogTimer watchdogTimer = new WatchdogTimer(reqIOType, timermilisec / 1000);
            //dataLength[1]はリザーブにより常に0　リザーブもデータ長に含めるので＋１している
            byte[] dataLength = BitConverter.GetBytes((ushort)(watchdogTimer.BinaryCode.Length + reqData.Length+1));


            byte[] bytes = new byte[] { }
                .Concat(new SubHeader().BinaryCode)//subheader
                .Concat(new RequestedNetworkNo(reqNetWorkNo).BinaryCode)//networkNo
                .Concat(new RequestedStationNo(reqStationNo).BinaryCode)//stationNo
                .Concat(BitConverter.GetBytes((ushort)reqIOType))//IOType
                .Concat(new byte[] { BitConverter.GetBytes(multiDropStationNo)[0] })//multiDropStationNo
                .Concat(new byte[] { dataLength[0]})//データ長
                .Concat(new byte[] { dataLength[1]})//リザーブ リファレンスマニュアルの表記はややこしいので注意
                .Concat(watchdogTimer.BinaryCode)
                .Concat(reqData)
                .ToArray();

            return bytes;
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
