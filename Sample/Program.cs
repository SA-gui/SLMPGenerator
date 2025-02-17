using SLMPGenerator.Command;
using SLMPGenerator.Common;
using SLMPGenerator.UseCase;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Sample;
class Program
{
    static void Main(string[] args)
    {
        Socket so = null;
        NetworkStream networkStream = null;
        BufferedStream bufferedStream = null;

        try
        {
            SLMPMessage slmpMessage = new SLMPMessage(
                messageType: MessageType.Binary,
                plcType: PLCType.Mitsubishi_R_Series,
                devAccessType: DeviceAccessType.Word,
                reqNetWorkNo: 0,
                reqStationNo: 255,
                reqIOType: RequestDestModuleIOType.OwnStationCPU,
                multiDropStationNo: 0,
                timerSec: 1
            );

            TcpClient client = new TcpClient();
            client.Connect("192.168.101.192", 1192);

            WriteToPLC(client, slmpMessage, "D10", new List<short> { 1, 3, 1 });
            ReadFromPLC(client, slmpMessage, "D10", 3);
        }
        finally
        {
            if (bufferedStream != null) try { bufferedStream.Close(); } catch { }
            if (networkStream != null) try { networkStream.Close(); } catch { }
            if (so != null) try { so.Close(); } catch { }
        }

        Console.WriteLine("完了");
    }

    static void WriteToPLC(TcpClient client, SLMPMessage slmpMessage, string address, List<short> data)
    {
        byte[] writeMsg = slmpMessage.CreateMessage(
            rawAddress: address,
            writeData: data
        );

        Console.WriteLine("送信バイト列: " + BitConverter.ToString(writeMsg).Replace("-", " "));

        NetworkStream stream = client.GetStream();
        stream.Write(writeMsg, 0, writeMsg.Length);

        byte[] res = new byte[50];
        int len = stream.Read(res, 0, res.Length);
        Console.WriteLine("受信バイト列: " + BitConverter.ToString(res).Replace("-", " "));
        SLMPResponse slmpRsponse = new SLMPResponse(slmpMessage);
        List<short> ret = slmpRsponse.Resolve(res, slmpMessage.NumberOfDevicePoints);
    }

    static void ReadFromPLC(TcpClient client, SLMPMessage slmpMessage, string address, ushort numOfDevPoints)
    {
        byte[] readMsg = slmpMessage.CreateMessage(
            rawAddress: address,
            numOfDevPoints: numOfDevPoints
        );

        Console.WriteLine("送信バイト列: " + BitConverter.ToString(readMsg).Replace("-", " "));

        NetworkStream stream = client.GetStream();
        stream.Write(readMsg, 0, readMsg.Length);

        byte[] res = new byte[50];
        int len = stream.Read(res, 0, res.Length);
        Console.WriteLine("受信バイト列: " + BitConverter.ToString(res).Replace("-", " "));

        SLMPResponse slmpRsponse = new SLMPResponse(slmpMessage);
        List<short> ret = slmpRsponse.Resolve(res, slmpMessage.NumberOfDevicePoints);

        int i = 0;
        foreach (short val in ret)
        {
            Console.WriteLine($"受信データ[{i.ToString("00")}]: " + val);
            i++;
        }
    }
}
