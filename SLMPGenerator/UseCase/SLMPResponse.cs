using SLMPGenerator.Command;
using SLMPGenerator.Common;
using SLMPGenerator.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.UseCase
{
    public class SLMPResponse
    {
        public MessageType MessageType { get; private set; }
        public DeviceAccessType DevAccessType { get; private set; }



        public SLMPResponse(MessageType messageType, DeviceAccessType devAccessType)
        {
            MessageType = messageType;
            DevAccessType = devAccessType;
        }

        public SLMPResponse(SLMPMessage message)
        {
            MessageType = message.MessageType;
            DevAccessType = message.DevAccessType;
        }

        public List<short> Resolve(byte[] response,ushort numberOfDevicePoints)
        {
            ushort responseDataUnitLength = GetResponseDataUnitLength(MessageType, DevAccessType);

            switch (MessageType)
            {
                case MessageType.ASCII:
                    return ResolveASCIIResponse(response, numberOfDevicePoints, responseDataUnitLength);
                case MessageType.Binary:
                    return ResolveBinaryResponse(response, DevAccessType, numberOfDevicePoints, responseDataUnitLength);
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

        private List<short> ResolveASCIIResponse(byte[] rawResponse, ushort numberOfDevicePoints, ushort responseDataUnitLength)
        {
            int resultCodeStartIndex = 18;
            int resultCodeLength = 4;
            string resultCode = Encoding.ASCII.GetString(rawResponse.Skip(resultCodeStartIndex).Take(resultCodeLength).ToArray());

            string normalResponseCode = "0000";

            if (resultCode != normalResponseCode)
            {
                throw new SLMPUnitErrorException($"ErrorCode:{resultCode} Consult your unit's manual for details.");
            }
            int responseDataStartIndex = 22;
            int responseDataLength = responseDataUnitLength * numberOfDevicePoints;

            byte[] res = rawResponse.Skip(responseDataStartIndex).Take(responseDataLength).ToArray();

            List<short> result = new List<short>();
            for (int i = 0; i < res.Length; i += responseDataUnitLength)
            {
                byte[] val = res.Skip(i).Take(responseDataUnitLength).ToArray();
                short convertedValue = Convert.ToInt16(Encoding.ASCII.GetString(val), 16);
                result.Add(convertedValue);
            }

            return result;
        }

        private List<short> ResolveBinaryResponse(byte[] rawResponse, DeviceAccessType devAccsessType, ushort numberOfDevicePoints, ushort responseDataUnitLength)
        {
            int resultCodeStartIndex = 9;
            int resultCodeLength = 2;
            string resultCode = Encoding.ASCII.GetString(rawResponse.Skip(resultCodeStartIndex).Take(resultCodeLength).ToArray());

            string normalResponseCode = "\0\0";

            if (resultCode != normalResponseCode)
            {
                throw new SLMPUnitErrorException($"ErrorCode:{resultCode} Consult your unit's manual for details.");
            }
            int responseDataStartIndex = 11;
            byte[] res;
            int responseDataLength;

            List<short> result = new List<short>();

            if (DeviceAccessType.Bit == devAccsessType)
            {
                responseDataLength = (int)Math.Ceiling((double)responseDataUnitLength / 2 * numberOfDevicePoints);
                res = rawResponse.Skip(responseDataStartIndex).Take(responseDataLength).ToArray();
                string temp = BitHelper.ToString(res).Substring(0, numberOfDevicePoints);
                foreach (char c in temp)
                {
                    short convertedValue = Convert.ToInt16(c.ToString(), 16);
                    result.Add(convertedValue);
                }
                return result;
            }

            responseDataLength = responseDataUnitLength * numberOfDevicePoints;
            res = rawResponse.Skip(responseDataStartIndex).Take(responseDataLength).ToArray();

            for (int i = 0; i < res.Length; i += responseDataUnitLength)
            {
                byte[] val = res.Skip(i).Take(responseDataUnitLength).ToArray();
                short convertedValue = BitConverter.ToInt16(val, 0);
                result.Add(convertedValue);
            }

            return result;
        }


        public override int GetHashCode()
        {
            return MessageType.GetHashCode() ^ DevAccessType.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is SLMPResponse other && MessageType.Equals(other.MessageType) && DevAccessType.Equals(other.DevAccessType);
        }


    }
}
