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
            /*
            //https://momomo-97.com/communicate-with-mitsubishi-plc-using-vb-net-mc-protocol/
            //https://plc-memo.com/maketool3/

            //binary
                //\x50x00x00xffxffx03x00x0ex00x04x00x01x04x02x00x03x00x00x00xa8x00x04x00
                //  50 00 00 FF FF 03 00 0E 00 04 00 01 04 02 00 03 00 00 00 A8 00 04 00
            //ascii
                //  35 30 30 30 30 30 46 46 30 33 46 46 30 30 30 30 30 45 30 30 30 34 30 34 30 31 30 30 30 32 30 30 30 30 30 30 30 33 30 30 41 38 30 30 30 34
                    35 30 30 30 30 30 46 46 30 33 46 46 30 30 30 30 30 45 30 30 30 34 30 34 30 31 30 30 30 32 30 30 30 30 30 30 30 33 30 30 41 38 30 30 30 34
                // 500000FF03FF00001C000404010002D***000000030004
                // 500000FF03FF00001C000404010002D***000000030004"
                // 500000FF03FF00001C000404010002D***000000030004
                0x50, 0x00, // サブヘッダ 固定値 3Eフレーム (hex:30 35 30 30 / dec:48 53 48 48)
                0x00,       // 要求先ネットワーク番号 自局 (hex:30 30 / dec:48 48)
                0xFF,       // 要求先局番 (hex:46 46 / dec:70 70)
                0xFF, 0x03, // 要求先ユニットI/O番号 マルチドロップ マルチCPU 二重化システム以外の場合固定値 (hex:30 33 46 46 / dec:48 51 70 70)
                0x00,       // 要求先マルチドロップ局番 (hex:30 30 / dec:48 48)
                0x0E, 0x00, // 要求データ長 (リザーブ以降のバイト長) (hex:30 30 30 45 / dec:48 48 48 69)
                0x04, 0x00, // 監視タイマー 250ms 例 01 = 250ms (hex:30 30 30 34 / dec:48 48 48 52)
                0x01, 0x04, // 要求データ コマンド 内部メモリ操作コマンド 0401=一括読出し (hex:30 34 30 31 / dec:48 52 48 49)
                0x02, 0x00, // サブコマンド 0000=ワードデバイスから1ワード単位でデータを読み出し (hex:30 30 30 32 / dec:48 48 48 50)
                0x03, 0x00, 0x00, 0x00, // 先頭デバイス番号 (hex:30 30 30 30 30 30 30 33 / dec:48 48 48 48 48 48 48 51)
                0xA8, 0x00, // デバイスコード D (hex:30 30 41 38 / dec:48 48 65 56)
                0x04, 0x00  // デバイス点数 1 (hex:30 30 30 34 / dec:48 48 48 52)
            50 00 00 ff ff 03 00 0e 00 04 00 01 04 02 00 18 00 00 00 a0 00 04 00 python ascii

            50 00 00 ff ff 03 00 0e 00 04 00 01 04 02 00 18 00 00 00 a0 00 04 00 python hex
            50 00 00 FF FF 03 00 0E 00 04 00 01 04 02 00 18 00 00 00 A0 00 04 00
            */
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


            byte[] sendBytes = slmpMessage.CreateMessage(
                rawAddress: "B18",
                numOfDevPoints: 4
            );

            // PLCとの接続
            TcpClient client = new TcpClient();
            client.Connect("192.168.101.192", 1192);
            Console.WriteLine("送信バイト列: " + BitConverter.ToString(sendBytes).Replace("-", " "));


            NetworkStream stream = client.GetStream();
            stream.Write(sendBytes, 0, sendBytes.Length);

            //応答受取
            byte[] res = new byte[50];//応答は最大50バイト?
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
        finally
        {
            if (bufferedStream != null) try { bufferedStream.Close(); } catch { }
             if (networkStream != null) try { networkStream.Close(); } catch { }
            if (so != null) try { so.Close(); } catch { }

        }
        
        Console.WriteLine("完了");


    }


}
