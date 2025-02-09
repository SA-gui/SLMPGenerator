using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command
{
    internal enum DeviceType
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

    internal enum DeviceNoRange
    {
        Dec,
        Hex
    }




}
