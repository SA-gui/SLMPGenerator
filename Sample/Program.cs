using SLMPGenerator;
using SLMPGenerator.Common;
using SLMPGenerator.Common.Command;
using SLMPGenerator.Read.Domain;

using SLMPGenerator.Write.Domain;
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
                MessageHelper slmpHelper=new MessageHelper(
                    messageType: MessageType.Binary,
                    plcType: PLCType.Mitsubishi_R_Series,
                    reqNetWorkNo: 0,
                    reqStationNo: 255,
                    reqIOType: RequestDestModuleIOType.OwnStationCPU,
                    multiDropStationNo: 0,
                    timerSec: 1
                );

                TcpClient client = new TcpClient();
                client.Connect("192.168.101.192", 1192);

                WriteToPLC(client, slmpHelper,"D10", new List<short> { 1, 3, 1 });
                ReadFromPLC(client, slmpHelper,DeviceAccessType.Word,"D10", 3);

                WriteToPLC(client, slmpHelper, "D10", new List<short> { 2, 4, 2 });
                ReadFromPLC(client, slmpHelper, DeviceAccessType.Bit, "D10", 8);
        }
            finally
            {
                if (bufferedStream != null) try { bufferedStream.Close(); } catch { }
                if (networkStream != null) try { networkStream.Close(); } catch { }
                if (so != null) try { so.Close(); } catch { }
            }

            Console.WriteLine("完了");
        }

        static void WriteToPLC(TcpClient client, MessageHelper helper, string rawAddress,List<short> writeData)
        {


            (byte[] message, WriteResponseResolver resolver) = helper.CreateWriteMessage(rawAddress, writeData);

            Console.WriteLine("送信バイト列: " + BitConverter.ToString(message).Replace("-", " "));

            NetworkStream stream = client.GetStream();

            stream.Write(message, 0, message.Length);

            byte[] res = new byte[50];
            int len = stream.Read(res, 0, res.Length);
            Console.WriteLine("受信バイト列: " + BitConverter.ToString(res).Replace("-", " "));
            
            resolver.Resolve(res);

        }

        static void ReadFromPLC(TcpClient client, MessageHelper helper, DeviceAccessType devAccessType,string rawAddress,ushort numberOfDevPoints)
        {
            (byte[] message ,ReadResponseResolver resolver)= helper.CreateReadMessage(devAccessType, rawAddress, numberOfDevPoints);

            Console.WriteLine("送信バイト列: " + BitConverter.ToString(message).Replace("-", " "));

            NetworkStream stream = client.GetStream();
            stream.Write(message, 0, message.Length);

            byte[] res = new byte[50];
            int len = stream.Read(res, 0, res.Length);
            Console.WriteLine("受信バイト列: " + BitConverter.ToString(res).Replace("-", " "));
        
            List<short> ret = resolver.Resolve(res);

            int i = 0;
            foreach (short val in ret)
            {
                Console.WriteLine($"受信データ[{i.ToString("00")}]: " + val);
                i++;
            }
        
        }
    }
