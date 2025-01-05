namespace SLMPGenerator
{
    //https://qiita.com/inari1047/items/4908926ba581c9297a3b
    internal class WatchdogTimer
    {
        const int TIMER_VALUE_BIT = 16;
        // タイマの単位値
        const double TIMER_UNIT_VALUE = 0.25;

        // ローカルステーションタイマMIN MAX
        const double LOCAL_ST_MIN_TIMER_VALUE = 0.25;
        const double LOCAL_ST_MAX_TIMER_VALUE = 10.0;

        const double REMOTE_ST_MIN_TIMER_VALUE = 0.5;
        const double REMOTE_ST_MAX_TIMER_VALUE = 60.0;

        private double _timerValue;
        private RequestedIOType _reqIOType;

        internal byte[] BinaryCode { get; private set; }


        internal WatchdogTimer(RequestedIOType reqIOType,double timerValue)
        {
            ValidatereqIOType(reqIOType);
            _reqIOType = reqIOType;
            ValidateTimerValue(timerValue);
            ValidateTimerRange(timerValue, reqIOType);
            _timerValue = timerValue;
            BinaryCode = ToSwapBinary();
        }


        private void ValidatereqIOType(RequestedIOType reqIOType)
        {
            if(!Enum.IsDefined(typeof(RequestedIOType), reqIOType))
            {
                throw new ArgumentException("Invalid RequestedIO Type", nameof(reqIOType));
            }
        }


        private void ValidateTimerValue(double timerValue)
        {
            if (timerValue < 0 || timerValue % TIMER_UNIT_VALUE != 0)
            {
                throw new ArgumentException($"Timer value must be greater than or equal to 0.Timer value must be a multiple of {TIMER_UNIT_VALUE}.", nameof(timerValue));
            }
        }

        private void ValidateTimerRange(double timerValue, RequestedIOType reqIOType)
        {

            double minValue, maxValue;

            //自局と他局でタイマの範囲が異なる
            switch(reqIOType)
            {
                case RequestedIOType.LocalStation://RequestedIOType.ManagementCPUも値が同じなので含まれる
                    minValue = LOCAL_ST_MIN_TIMER_VALUE;
                    maxValue = LOCAL_ST_MAX_TIMER_VALUE;
                    break;
                default:
                    minValue = REMOTE_ST_MIN_TIMER_VALUE;
                    maxValue = REMOTE_ST_MAX_TIMER_VALUE;
                    break;
            }

            //タイマの範囲チェック
            if (timerValue < minValue || timerValue > maxValue)
            {
                throw new ArgumentException($"Timer value must be between {minValue} and {maxValue} for {reqIOType} Station", nameof(timerValue));
            }
        }

        private string ToAscii()
        {
            byte[] bytes = BitConverter.GetBytes((ushort)(_timerValue / TIMER_UNIT_VALUE));
            return bytes[1].ToString("X2") + bytes[0].ToString("X2");
        }

        private byte[] ToAsciiBinary()
        {
            byte[] bytes = BitConverter.GetBytes((ushort)(_timerValue / TIMER_UNIT_VALUE));
            // 上位バイト　下位バイトの順にする
            byte highByte = bytes[1];
            byte lowByte = bytes[0];
            return new byte[] { (byte)(highByte >> 4), (byte)(highByte & 0x0F), (byte)(lowByte >> 4), (byte)(lowByte & 0x0F) };
        }

        private byte[] ToSwapBinary()
        {
            byte[] bytes = BitConverter.GetBytes((ushort)(_timerValue / TIMER_UNIT_VALUE));
            // 下位バイト　上位バイトの順にする
            return new byte[] { bytes[1], bytes[0] };
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(_timerValue, _reqIOType);
        }

        public override bool Equals(object obj)
        {
            return obj is WatchdogTimer timer &&
               _timerValue == timer._timerValue &&
               _reqIOType == timer._reqIOType;
        }

        public static bool operator ==(WatchdogTimer left, WatchdogTimer right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(WatchdogTimer left, WatchdogTimer right)
        {
            return !(left.Equals(right));
        }

    }
}
