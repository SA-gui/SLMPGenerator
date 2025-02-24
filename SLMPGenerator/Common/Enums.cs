using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    public enum DeviceType
    {
        Bit,
        Word,
        DoubleWord
    }

    /// <summary>
    /// デバイスにアクセス際にbitかwordかを判別するための列挙型
    /// SubCommandの判別に使用
    /// </summary>
    public enum DeviceAccessType
    {
        Bit,
        Word
    }

    public enum DeviceNoRange
    {
        Dec,
        Hex
    }


    public enum MessageType
    {
        Binary,
        ASCII
    }

    public enum PLCType
    {
        Mitsubishi_Q_Series,
        Mitsubishi_R_Series
    }

}
