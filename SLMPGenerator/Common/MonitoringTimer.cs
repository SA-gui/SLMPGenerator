using System.Text;

namespace SLMPGenerator.Common
{
    //https://qiita.com/inari1047/items/4908926ba581c9297a3b
    internal class MonitoringTimer
    {
        // タイマの単位値
        const double TIMER_UNIT_VALUE = 0.25;

        // ローカルステーションタイマMIN MAX
        const double OWN_ST_MIN_TIMER_VALUE = 0.25;
        const double OWN_ST_MAX_TIMER_VALUE = 10.0;

        const double OTHER_ST_MIN_TIMER_VALUE = 0.5;
        const double OTHER_ST_MAX_TIMER_VALUE = 60.0;

        internal byte[] BinaryCode { get; private set; }
        internal string ASCIICode { get; private set; }


        internal MonitoringTimer(RequestDestModuleIOType destinationIOType, double timerSec)
        {
            ValidatereqIOType(destinationIOType);
            ValidateTimerValue(timerSec);
            ValidateTimerRange(timerSec, destinationIOType);
            ushort timerSetVal = (ushort)(timerSec / TIMER_UNIT_VALUE);
            BinaryCode = BitHelper.ToBytesLittleEndian(timerSetVal);
            ASCIICode = BitHelper.ToReverseString(BinaryCode);
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


        public override int GetHashCode()
        {
            return ASCIICode.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is MonitoringTimer other && ASCIICode.Equals(other.ASCIICode);
        }

    }
}
