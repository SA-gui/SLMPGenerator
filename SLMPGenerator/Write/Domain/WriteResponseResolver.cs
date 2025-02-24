using SLMPGenerator.Common;
using SLMPGenerator.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Write.Domain
{
    public class WriteResponseResolver
    {
        public MessageType MessageType { get; private set; }
        public DeviceAccessType DeviceAccessType { get; private set; }
        public ushort NumberOfDevicePoints { get; private set; }


        public WriteResponseResolver(MessageType messageType, DeviceAccessType devAccessType, ushort numberOfDevicePoints)
        {
            MessageType = messageType;
            DeviceAccessType = devAccessType;
            NumberOfDevicePoints = numberOfDevicePoints;
        }


        public void Resolve(byte[] response)
        {
            ushort responseDataUnitLength = GetResponseDataUnitLength(MessageType, DeviceAccessType);

            switch (MessageType)
            {
                case MessageType.ASCII:
                    ResolveASCIIResponse(response, NumberOfDevicePoints, responseDataUnitLength);
                    break;
                case MessageType.Binary:
                    ResolveBinaryResponse(response, DeviceAccessType, NumberOfDevicePoints, responseDataUnitLength);
                    break;
                default:
                    throw new NotSupportedException("Please specify Ascii or Binary as the message type.");
            }
        }

        private ushort GetResponseDataUnitLength(MessageType messageType, DeviceAccessType devAccsessType)
        {
            return (messageType, devAccsessType) switch
            {
                (MessageType.ASCII, DeviceAccessType.Bit) => 1,
                (MessageType.ASCII, DeviceAccessType.Word) => 4,
                (MessageType.Binary, DeviceAccessType.Bit) => 1,
                (MessageType.Binary, DeviceAccessType.Word) => 2,
                _ => throw new NotSupportedException("This Combinetion is not supported.")
            };
        }

        private void ResolveASCIIResponse(byte[] rawResponse, ushort numberOfDevicePoints, ushort responseDataUnitLength)
        {
            int resultCodeStartIndex = 18;
            int resultCodeLength = 4;
            string resultCode = Encoding.ASCII.GetString(rawResponse.Skip(resultCodeStartIndex).Take(resultCodeLength).ToArray());

            string normalResponseCode = "0000";

            if (resultCode != normalResponseCode)
            {
                throw new SLMPUnitErrorException($"ErrorCode:{resultCode} Consult your unit's manual for details.");
            }
            // ToDO: ここで応答データ長を取得する処理を追加する
        }

        private void ResolveBinaryResponse(byte[] rawResponse, DeviceAccessType devAccsessType, ushort numberOfDevicePoints, ushort responseDataUnitLength)
        {
            int resultCodeStartIndex = 9;
            int resultCodeLength = 2;
            string resultCode = Encoding.ASCII.GetString(rawResponse.Skip(resultCodeStartIndex).Take(resultCodeLength).ToArray());

            string normalResponseCode = "\0\0";

            if (resultCode != normalResponseCode)
            {
                throw new SLMPUnitErrorException($"ErrorCode:{resultCode} Consult your unit's manual for details.");
            }

        }


        public override int GetHashCode()
        {
            return HashCode.Combine(MessageType.GetHashCode(), DeviceAccessType.GetHashCode());
        }

        public override bool Equals(object? obj)
        {
            return obj is WriteResponseResolver other && MessageType.Equals(other.MessageType) && DeviceAccessType.Equals(other.DeviceAccessType);
        }


    }
}
