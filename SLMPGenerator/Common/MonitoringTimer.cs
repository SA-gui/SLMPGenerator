using System.Text;

namespace SLMPGenerator.Common
{
    //https://qiita.com/inari1047/items/4908926ba581c9297a3b
    internal class MonitoringTimer
    {
        // タイマの単位値
        const double TIMER_UNIT_VALUE = 0.25;

        // ローカルステーションタイマMIN MAX
        const double OWN_ST_MIN_TIMER_VALUE = 0.2;
        const double OWN_ST_MAX_TIMER_VALUE = 10.0;

        const double OTHER_ST_MIN_TIMER_VALUE = 0.5;
        const double OTHER_ST_MAX_TIMER_VALUE = 60.0;

        private double _timerSec;
        private RequestDestModuleIOType _reqIOType;

        internal byte[] BinaryCode { get; private set; }
        internal string ASCIICode { get; private set; }


        internal MonitoringTimer(RequestDestModuleIOType requestIOType, double timerSec)
        {
            ValidatereqIOType(requestIOType);
            _reqIOType = requestIOType;
            ValidateTimerValue(timerSec);
            ValidateTimerRange(timerSec, requestIOType);
            _timerSec = timerSec;
            ushort timerSetVal = (ushort)(_timerSec / TIMER_UNIT_VALUE);
            BinaryCode = BitHelper.ConvertToBytesLittleEndian(timerSetVal);
            ASCIICode = BitConverter.ToString(BitHelper.ConvertToBytesBigEndian(timerSetVal)).Replace("-", "");
        }


        private void ValidatereqIOType(RequestDestModuleIOType requestIOType)
        {
            if (!Enum.IsDefined(typeof(RequestDestModuleIOType), requestIOType))
            {
                throw new ArgumentException("Invalid RequestedIO Type", nameof(requestIOType));
            }
        }


        private void ValidateTimerValue(double timerValue)
        {
            if (timerValue < 0 || timerValue % TIMER_UNIT_VALUE != 0)
            {
                throw new ArgumentException($"Timer value must be greater than or equal to 0.Timer value must be a multiple of {TIMER_UNIT_VALUE}.", nameof(timerValue));
            }
        }

        private void ValidateTimerRange(double timerValue, RequestDestModuleIOType reqIOType)
        {

            double minValue, maxValue;

            //自局と他局でタイマの範囲が異なる
            switch (reqIOType)
            {
                case RequestDestModuleIOType.OwnStationCPU:
                    minValue = OWN_ST_MIN_TIMER_VALUE;
                    maxValue = OWN_ST_MAX_TIMER_VALUE;
                    break;
                default:
                    minValue = OTHER_ST_MIN_TIMER_VALUE;
                    maxValue = OTHER_ST_MAX_TIMER_VALUE;
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
            byte[] bytes = BitConverter.GetBytes((ushort)(_timerSec / TIMER_UNIT_VALUE));
            return bytes[1].ToString("X2") + bytes[0].ToString("X2");
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(_timerSec, _reqIOType);
        }

        public override bool Equals(object? obj)
        {
            return obj is MonitoringTimer timer &&
               _timerSec == timer._timerSec &&
               _reqIOType == timer._reqIOType;
        }

        public static bool operator ==(MonitoringTimer left, MonitoringTimer right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MonitoringTimer left, MonitoringTimer right)
        {
            return !left.Equals(right);
        }

    }
}
