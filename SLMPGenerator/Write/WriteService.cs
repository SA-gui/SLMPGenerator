using SLMPGenerator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLMPGenerator.Write.Domain;

namespace SLMPGenerator.Write
{
    internal class WriteService
    {
    

        private IWriteRequestFactory _factory;

        public MessageType MessageType { get; private set; }
        public ushort ReqNetWorkNo { get; private set; }
        public ushort ReqStationNo { get; private set; }
        public RequestDestModuleIOType ReqIOType { get; private set; }
        public ushort MultiDropStationNo { get; private set; }
        public double TimerSec { get; private set; }


        public WriteService(IWriteRequestFactory factory, MessageType messageType, ushort reqNetWorkNo, ushort reqStationNo, RequestDestModuleIOType reqIOType, ushort multiDropStationNo, double timerSec)
        {
            ValidateMessageType(messageType);
            ValidateRequestDestModuleIOType(reqIOType);
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            MessageType = messageType;
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

        private static void ValidateRequestDestModuleIOType(RequestDestModuleIOType reqIOType)
        {
            if (!Enum.IsDefined(typeof(RequestDestModuleIOType), reqIOType))
            {
                throw new ArgumentException("Invalid RequestDestModuleIOType");
            }
        }
        
        public WriteMessage Create(string rawAddress, List<short> writeData)
        {

            WriteRequest requestData = _factory.Create(MessageType, rawAddress, writeData);

            MonitoringTimer monitoringTimer = new MonitoringTimer(ReqIOType, TimerSec);

            return new WriteMessage(
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

        public WriteMessage Create(string rawAddress, List<bool> writeData)
        {

            WriteRequest requestData = _factory.Create(MessageType, rawAddress, writeData);

            MonitoringTimer monitoringTimer = new MonitoringTimer(ReqIOType, TimerSec);

            return new WriteMessage(
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
