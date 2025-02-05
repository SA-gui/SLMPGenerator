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
            ushort deviceAddress = 3;
            ushort deviceQty= 4;
            byte[] sendBytes = MessageCreater.STMessageCreate(
                messageType: MessageType.ASCII,
                plcType: PLCType.Mitsubishi_R_Series,
                reqNetWorkNo: 0,
                reqStationNo: 255,
                reqIOType: RequestDestModuleIOType.OwnStationCPU,
                multiDropStationNo: 0,
                timerSec: 1,
                commandType: CommandType.Device_Read,
                deviceType: DeviceType.DataRegister,
                targetDeviceStartAddress: deviceAddress,
                targetNumberOfDevicePoints: deviceQty
            );

            //https://momomo-97.com/communicate-with-mitsubishi-plc-using-vb-net-mc-protocol/
            //https://plc-memo.com/maketool3/
            /*
            //ST型　要求伝文　バイナリ
            byte[] qseriseBytes = new byte[]
            {
                0x50, 0x00, //サブヘッダ　固定値 3Eフレーム
                0x00, //要求先ネットワーク番号　自局
                0xFF, //要求先局番　
                0xFF, 0x03, //要求先ユニットI/O番号 マルチドロップ　マルチCPU　二重かシステム以外の場合固定値
                0x00, //要求先マルチドロップ局番
                0x0C, 0x00, //要求データ長(リザーブ以降のバイト長)
                0x00, 0x01, //監視タイマー　250ms 例 01 = 250ms
                0x01, 0x04, //要求データ　コマンド　内部メモリ操作コマンド 0401=一括読出し
                0x00, 0x00, //サブコマンド 0000=ワードデバイスから1ワード単位でデータを読み出し
                0x03, 0x00, 0x00,//先頭デバイス番号
                 0xA8, //デバイスコード　D
                0x01, 0x00//デバイス点数1
            };
            byte[] rseriseBytes = new byte[]
            {
            //binary
                //\x50x00x00xffxffx03x00x0ex00x04x00x01x04x02x00x03x00x00x00xa8x00x04x00
                //  50 00 00 FF FF 03 00 0E 00 04 00 01 04 02 00 03 00 00 00 A8 00 04 00
           //ascii
                //  35 30 30 30 30 30 46 46 30 33 46 46 30 30 30 30 30 45 30 30 30 34 30 34 30 31 30 30 30 32 30 30 30 30 30 30 30 33 30 30 41 38 30 30 30 34
                    35 30 30 30 30 30 46 46 30 33 46 46 30 30 30 30 30 45 30 30 30 34 30 34 30 31 30 30 30 32 30 30 30 30 30 30 30 33 30 30 41 38 30 30 30 34
 
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


            };
            */
            // PLCとの接続
            TcpClient client = new TcpClient();
            client.Connect("192.168.101.192", 1192);
            Console.WriteLine("送信バイト列: " + BitConverter.ToString(sendBytes).Replace("-", " "));


            NetworkStream stream = client.GetStream();
            stream.Write(sendBytes, 0, sendBytes.Length);

            //応答受取
            byte[] res = new byte[100];//応答は最大100バイト
            int len = stream.Read(res, 0, res.Length);

            //終了コードチェック Low-Highの順なので8ビットシフト
            int rescode = (res[10] << 8) | res[9];
            
            if (rescode != 0) { 
                throw new Exception("ERROR: " + rescode);
            }

            //エラーがなければ値を読む
            for (int i = 0; i < deviceQty; i++)
            {
                long val = (res[12+i*2] << 8) | res[11 + i * 2];

                Console.WriteLine($"D{deviceAddress+i}=" + val);
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
