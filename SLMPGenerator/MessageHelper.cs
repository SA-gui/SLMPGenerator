using SLMPGenerator.Command.Mitsubishi;
using SLMPGenerator.Common;
using SLMPGenerator.Read;
using SLMPGenerator.Read.Domain;
using SLMPGenerator.Read.Infrastructure.Mitsubishi;
using SLMPGenerator.Write;
using SLMPGenerator.Write.Domain;
using SLMPGenerator.Write.Infrastructure.Mitsubishi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator
{
    public class MessageHelper
    {
        private IReadRequestFactory _readFactory;
        private IWriteRequestFactory _writeFactory;
        private ushort _numberOfDevicePoints;
        public MessageType MessageType { get; private set; }
        public PLCType PlcType { get; private set; }
        public DeviceAccessType DevAccessType { get; private set; }
        public ushort ReqNetWorkNo { get; private set; }
        public ushort ReqStationNo { get; private set; }
        public RequestDestModuleIOType ReqIOType { get; private set; }
        public ushort MultiDropStationNo { get; private set; }
        public double TimerSec { get; private set; }


        public MessageHelper(MessageType messageType, PLCType plcType,  ushort reqNetWorkNo, ushort reqStationNo, RequestDestModuleIOType reqIOType, ushort multiDropStationNo, double timerSec)
        {
            MessageType = messageType;
            ValidatePlcType(PlcType);
            PlcType = plcType;
            
            ReqNetWorkNo = reqNetWorkNo;
            ReqStationNo = reqStationNo;
            ReqIOType = reqIOType;
            MultiDropStationNo = multiDropStationNo;
            TimerSec = timerSec;

            switch (PlcType)
            {
                case PLCType.Mitsubishi_Q_Series:
                    _readFactory = new QSeriesReadRequestFactory();
                    _writeFactory = new QSeriesWriteRequestFactory();
                    break;
                case PLCType.Mitsubishi_R_Series:
                    _readFactory = new RSeriesReadRequestFactory();
                    _writeFactory = new RSeriesWriteRequestFactory();
                    break;
                default:
                    throw new ArgumentException("Invalid PLCType");
            }


        }

        private void ValidatePlcType(PLCType plcType)
        {
            if (!Enum.IsDefined(typeof(PLCType), plcType))
            {
                throw new ArgumentException("Invalid PLCType");
            }
        }


        public (byte[], ReadResponseResolver) CreateReadMessage(DeviceAccessType devAccessType, string rawAddress, ushort points)
        {
            DevAccessType = devAccessType;
            _numberOfDevicePoints = points;
            ReadService readService = new ReadService(_readFactory, MessageType, DevAccessType, ReqNetWorkNo, ReqStationNo, ReqIOType, MultiDropStationNo, TimerSec);
            ReadMessage readMessage = readService.Create(rawAddress, points);
            ReadResponseResolver readResponse = new ReadResponseResolver(MessageType, DevAccessType, _numberOfDevicePoints);
            
            byte[] messageBytes;
            switch (MessageType)
            {
                case MessageType.Binary:
                    messageBytes= readMessage.BinaryCode;
                    break;
                case MessageType.ASCII:
                    messageBytes = readMessage.ASCIIBinaryCode;
                    break;
                default:
                    throw new ArgumentException("Invalid MessageType");
            }
            return (messageBytes, readResponse);
        }
        public (byte[], WriteResponseResolver) CreateWriteMessage(string rawAddress, List<short> writeData)
        {
            DevAccessType = DeviceAccessType.Word;
            _numberOfDevicePoints = (ushort)writeData.Count;
            WriteService writeService = new WriteService(_writeFactory, MessageType, ReqNetWorkNo, ReqStationNo, ReqIOType, MultiDropStationNo, TimerSec);
            WriteMessage writeMessage = writeService.Create(rawAddress, writeData);
            WriteResponseResolver writeResponse = new WriteResponseResolver(MessageType, DevAccessType, _numberOfDevicePoints);

            byte[] messageBytes;
            switch (MessageType)
            {
                case MessageType.Binary:
                    messageBytes = writeMessage.BinaryCode;
                    break;
                case MessageType.ASCII:
                    messageBytes = writeMessage.ASCIIBinaryCode;
                    break;
                default:
                    throw new ArgumentException("Invalid MessageType");
            }

            return (messageBytes, writeResponse);
        }
        public (byte[], WriteResponseResolver) CreateWriteMessage(string rawAddress, List<bool> writeData)
        {
            DevAccessType = DeviceAccessType.Bit;
            _numberOfDevicePoints = (ushort)writeData.Count;
            WriteService writeService = new WriteService(_writeFactory, MessageType, ReqNetWorkNo, ReqStationNo, ReqIOType, MultiDropStationNo, TimerSec);
            WriteMessage writeMessage = writeService.Create(rawAddress, writeData);
            WriteResponseResolver writeResponse = new WriteResponseResolver(MessageType, DevAccessType, _numberOfDevicePoints);

            byte[] messageBytes;
            switch (MessageType)
            {
                case MessageType.Binary:
                    messageBytes = writeMessage.BinaryCode;
                    break;
                case MessageType.ASCII:
                    messageBytes = writeMessage.ASCIIBinaryCode;
                    break;
                default:
                    throw new ArgumentException("Invalid MessageType");
            }

            return (messageBytes, writeResponse);
        }
    }
}
