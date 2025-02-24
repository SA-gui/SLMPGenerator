using SLMPGenerator.Common;
using SLMPGenerator.Read.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Read
{
    internal class ReadService
    {

        private IReadRequestFactory _factory;

        public MessageType MessageType { get; private set; }
        public DeviceAccessType DevAccessType { get; private set; }
        public ushort ReqNetWorkNo { get; private set; }
        public ushort ReqStationNo { get; private set; }
        public RequestDestModuleIOType ReqIOType { get; private set; }
        public ushort MultiDropStationNo { get; private set; }
        public double TimerSec { get; private set; }


        public ReadService(IReadRequestFactory factory,MessageType messageType, DeviceAccessType devAccessType, ushort reqNetWorkNo, ushort reqStationNo, RequestDestModuleIOType reqIOType, ushort multiDropStationNo, double timerSec)
        {
            ValidateMessageType(messageType);
            ValidateDeviceAccessType(devAccessType);
            ValidateRequestDestModuleIOType(reqIOType);
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            MessageType = messageType;
            DevAccessType = devAccessType;
            ReqNetWorkNo = reqNetWorkNo;
            ReqStationNo = reqStationNo;
            ReqIOType = reqIOType;
            MultiDropStationNo = multiDropStationNo;
            TimerSec = timerSec;
        }



        private static void ValidateMessageType(MessageType messageType)
        {
            if (!Enum.IsDefined(typeof(MessageType), messageType))
            {
                throw new ArgumentException("Invalid MessageType");
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

        public ReadMessage Create(string rawAddress, ushort points)
        {

            ReadRequest requestData = _factory.Create(DevAccessType, MessageType, rawAddress, points);

            MonitoringTimer monitoringTimer = new MonitoringTimer(ReqIOType, TimerSec);

            return  new ReadMessage(
                requestData,
                monitoringTimer,
                new SubHeader(), 
                new RequestDestNetworkNo(ReqNetWorkNo), 
                new RequestDestStationNo(ReqStationNo),
                new RequestDestModuleIONo(ReqIOType), 
                new RequestDestMultiDropStationNo(MultiDropStationNo),
                new RequestLength(monitoringTimer, requestData)
                );

        }


    }
}
