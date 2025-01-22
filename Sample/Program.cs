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
                plcType: PLCType.Mitsubishi_R_Series,
                reqNetWorkNo: 0,
                reqStationNo: 255,
                reqIOType: RequestedIOType.LocalStation,
                multiDropStationNo: 0,
                timermilisec: 250,
                commandType: CommandType.Read,
                deviceType: DeviceType.DataRegister,
                targetDeviceAddress: deviceAddress,
                targetDeviceQty: deviceQty
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
                //\x50x00\x00\xff\xff\x03\x00\x0e\x00\x04\x00\x01\x04\x02\x00\x03\x00\x00\x00\xa8\x00\x04\x00
                //500000ffff03000e0004000104020003000000a8000400
                0x50, 0x00, //サブヘッダ　固定値 3Eフレーム
                0x00, //要求先ネットワーク番号　自局
                0xFF, //要求先局番
                0xFF, 0x03, //要求先ユニットI/O番号 マルチドロップ　マルチCPU　二重かシステム以外の場合固定値
                0x00, //要求先マルチドロップ局番
                0x0e, 0x00, //要求データ長(リザーブ以降のバイト長)
                0x04, 0x00, //監視タイマー　250ms 例 01 = 250ms
                0x01, 0x04, //要求データ　コマンド　内部メモリ操作コマンド 0401=一括読出し
                0x02, 0x00, //サブコマンド 0000=ワードデバイスから1ワード単位でデータを読み出し
                0x03, 0x00, 0x00, 0x00 ,//先頭デバイス番号
                0xA8, 0x00, //デバイスコード　D
                0x01, 0x00//デバイス点数1
            };
            */
            // PLCとの接続
            TcpClient client = new TcpClient();
            client.Connect("192.168.101.192", 1192);

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
