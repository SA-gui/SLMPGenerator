using SLMPGenerator.Command.Mitsubishi;
using SLMPGenerator.Command;
using SLMPGenerator.Common;
using SLMPGenerator.Response;
using System.Text;

namespace SLMPGenerator.UseCase
{
    public class SLMPMessage
    {
        public MessageType MessageType { get; private set; }
        public PLCType PlcType { get; private set; }
        public DeviceAccessType DevAccessType { get; private set; }
        public ushort ReqNetWorkNo { get; private set; }
        public ushort ReqStationNo { get; private set; }
        public RequestDestModuleIOType ReqIOType { get; private set; }
        public ushort MultiDropStationNo { get; private set; }
        public double TimerSec { get; private set; }

        private MonitoringTimer _monitoringTimer;

        public ushort NumberOfDevicePoints { get; private set; }
        private IRequestData? _requestData;

        public SLMPMessage(MessageType messageType, PLCType plcType, DeviceAccessType devAccessType, ushort reqNetWorkNo, ushort reqStationNo, RequestDestModuleIOType reqIOType, ushort multiDropStationNo, double timerSec)
        {
            ValidateRequestStationNo(reqNetWorkNo, reqStationNo);
            ValidateMessageType(messageType);
            ValidatePlcType(plcType);
            ValidateDeviceAccessType(devAccessType);
            ValidateRequestDestModuleIOType(reqIOType);
            _monitoringTimer = new MonitoringTimer(reqIOType, timerSec);
            MessageType = messageType;
            PlcType = plcType;
            DevAccessType = devAccessType;
            ReqNetWorkNo = reqNetWorkNo;
            ReqStationNo = reqStationNo;
            ReqIOType = reqIOType;
            MultiDropStationNo = multiDropStationNo;
            TimerSec = timerSec;
        }

        private static void ValidateRequestStationNo(ushort reqNetWorkNo, ushort reqStationNo)
        {
            if (!(reqNetWorkNo == 0 && reqStationNo == 255))
            {
                throw new ArgumentException("Invalid combination of reqNetWorkNo and reqStationNo");
            }
        }

        private static void ValidateMessageType(MessageType messageType)
        {
            if (!Enum.IsDefined(typeof(MessageType), messageType))
            {
                throw new ArgumentException("Invalid MessageType");
            }
        }

        private static void ValidatePlcType(PLCType plcType)
        {
            if (!Enum.IsDefined(typeof(PLCType), plcType))
            {
                throw new ArgumentException("Invalid PLCType");
            }
        }

        private static void ValidateDeviceAccessType(DeviceAccessType devAccessType)
        {
            if (!Enum.IsDefined(typeof(DeviceAccessType), devAccessType))
            {
                throw new ArgumentException("Invalid DeviceAccessType");
            }
        }

        private static void ValidateRequestDestModuleIOType(RequestDestModuleIOType reqIOType)
        {
            if (!Enum.IsDefined(typeof(RequestDestModuleIOType), reqIOType))
            {
                throw new ArgumentException("Invalid RequestDestModuleIOType");
            }
        }

        //SingleTransmission
        public byte[] CreateMessage(
            string rawAddress,
            ushort numOfDevPoints)
        {

            NumberOfDevicePoints = numOfDevPoints;
            IRequestData requestData;

            switch (PlcType)
            {
                case PLCType.Mitsubishi_R_Series:
                    requestData = RSeriesRequestDataFactory.CreateReadRequestData(DevAccessType, MessageType, rawAddress, numOfDevPoints);
                    break;
                case PLCType.Mitsubishi_Q_Series:
                    requestData = QSeriesRequestDataFactory.CreateReadRequestData(DevAccessType, MessageType, rawAddress, numOfDevPoints);
                    break;
                default:
                    throw new NotSupportedException("This PLC type is not supported.");
            }

            switch (MessageType)
            {
                case MessageType.Binary:
                    return CreateBinaryMessage(requestData);
                case MessageType.ASCII:
                    string asciiCode = CreateASCIIMessage(requestData);
                    return Encoding.ASCII.GetBytes(asciiCode);
                default:
                    throw new NotSupportedException("Please specify Ascii or Binary as the message type.");
            }
        }

        public byte[] CreateMessage(
            string rawAddress,
            List<short> writeData)
        {

            NumberOfDevicePoints = (ushort)writeData.Count;
            IRequestData requestData;

            switch (PlcType)
            {
                case PLCType.Mitsubishi_R_Series:
                    requestData = RSeriesRequestDataFactory.CreateWriteRequestData(MessageType, rawAddress, writeData);
                    break;
                case PLCType.Mitsubishi_Q_Series:
                    requestData = QSeriesRequestDataFactory.CreateWriteRequestData(MessageType, rawAddress, writeData);
                    break;
                default:
                    throw new NotSupportedException("This PLC type is not supported.");
            }

            switch (MessageType)
            {
                case MessageType.Binary:
                    return CreateBinaryMessage(requestData);
                case MessageType.ASCII:
                    string asciiCode = CreateASCIIMessage(requestData);
                    return Encoding.ASCII.GetBytes(asciiCode);
                default:
                    throw new NotSupportedException("Please specify Ascii or Binary as the message type.");
            }
        }
        public byte[] CreateMessage(
            string rawAddress,
            List<bool> writeData)
        {

            NumberOfDevicePoints = (ushort)writeData.Count;
            IRequestData requestData;

            switch (PlcType)
            {
                case PLCType.Mitsubishi_R_Series:
                    requestData = RSeriesRequestDataFactory.CreateWriteRequestData(MessageType, rawAddress, writeData);
                    break;
                case PLCType.Mitsubishi_Q_Series:
                    requestData = QSeriesRequestDataFactory.CreateWriteRequestData(MessageType, rawAddress, writeData);
                    break;
                default:
                    throw new NotSupportedException("This PLC type is not supported.");
            }

            switch (MessageType)
            {
                case MessageType.Binary:
                    return CreateBinaryMessage(requestData);
                case MessageType.ASCII:
                    string asciiCode = CreateASCIIMessage(requestData);
                    return Encoding.ASCII.GetBytes(asciiCode);
                default:
                    throw new NotSupportedException("Please specify Ascii or Binary as the message type.");
            }
        }

        private byte[] CreateBinaryMessage(IRequestData requestData)
        {
            return new byte[] { }
                            .Concat(new SubHeader().BinaryCode)//subheader
                            .Concat(new RequestDestNetworkNo(ReqNetWorkNo).BinaryCode)//networkNo
                            .Concat(new RequestDestStationNo(ReqStationNo).BinaryCode)//stationNo
                            .Concat(new RequestDestModuleIONo(ReqIOType).BinaryCode)//IONo
                            .Concat(new RequestDestMultiDropStationNo(MultiDropStationNo).BinaryCode)//multiDropStationNo
                            .Concat(new RequestDataLength(_monitoringTimer, requestData).BinaryCode)//dataLength
                            .Concat(_monitoringTimer.BinaryCode)
                            .Concat(requestData.BinaryCode)
                            .ToArray();
        }

        private string CreateASCIIMessage(IRequestData requestData)
        {
            return new StringBuilder()
                    .Append(new SubHeader().ASCIICode)
                    .Append(new RequestDestNetworkNo(ReqNetWorkNo).ASCIICode)
                    .Append(new RequestDestStationNo(ReqStationNo).ASCIICode)
                    .Append(new RequestDestModuleIONo(ReqIOType).ASCIICode)
                    .Append(new RequestDestMultiDropStationNo(MultiDropStationNo).ASCIICode)
                    .Append(new RequestDataLength(_monitoringTimer, requestData).ASCIICode)
                    .Append(_monitoringTimer.ASCIICode)
                    .Append(requestData.ASCIICode)
                    .ToString();
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(
                (MessageType.ToString() +
                PlcType.ToString() +
                DevAccessType.ToString() +
                ReqNetWorkNo.ToString() +
                ReqStationNo.ToString() +
                ReqIOType.ToString() +
                MultiDropStationNo.ToString() +
                TimerSec.ToString()).GetHashCode(),
                _monitoringTimer.GetHashCode(),
                _requestData?.GetHashCode() ?? 0
            );
        }

        public override bool Equals(object? obj)
        {
            return obj is SLMPMessage other &&
                   MessageType == other.MessageType &&
                   PlcType == other.PlcType &&
                   DevAccessType == other.DevAccessType &&
                   ReqNetWorkNo == other.ReqNetWorkNo &&
                   ReqStationNo == other.ReqStationNo &&
                   ReqIOType == other.ReqIOType &&
                   MultiDropStationNo == other.MultiDropStationNo &&
                   TimerSec == other.TimerSec &&
                   _monitoringTimer.Equals(other._monitoringTimer) &&
                   (_requestData?.Equals(other._requestData) ?? other._requestData == null);
        }
    }
}
