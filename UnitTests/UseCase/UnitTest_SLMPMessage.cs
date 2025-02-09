using SLMPGenerator.Command;
using SLMPGenerator.Command.Mitsubishi;
using SLMPGenerator.Common;
using SLMPGenerator.UseCase;
using Xunit;

namespace SLMPGenerator.Tests.UseCase
{
    public class UnitTest_SLMPMessage
    {
        /// <summary>
        /// コンストラクタに有効なパラメータを渡した場合、プロパティが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, PLCType.Mitsubishi_R_Series, DeviceAccessType.Bit, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0)]
        [InlineData(MessageType.ASCII, PLCType.Mitsubishi_Q_Series, DeviceAccessType.Word, 0, 255, RequestDestModuleIOType.ControlCPU, 1, 2.0)]
        public void Constructor_ValidParameters_SetsProperties(MessageType messageType, PLCType plcType, DeviceAccessType devAccessType, ushort reqNetWorkNo, ushort reqStationNo, RequestDestModuleIOType reqIOType, ushort multiDropStationNo, double timerSec)
        {
            // Arrange & Act
            var slmpMessage = new SLMPMessage(messageType, plcType, devAccessType, reqNetWorkNo, reqStationNo, reqIOType, multiDropStationNo, timerSec);

            // Assert
            Assert.Equal(messageType, slmpMessage.MessageType);
            Assert.Equal(plcType, slmpMessage.PlcType);
            Assert.Equal(devAccessType, slmpMessage.DevAccessType);
            Assert.Equal(reqNetWorkNo, slmpMessage.ReqNetWorkNo);
            Assert.Equal(reqStationNo, slmpMessage.ReqStationNo);
            Assert.Equal(reqIOType, slmpMessage.ReqIOType);
            Assert.Equal(multiDropStationNo, slmpMessage.MultiDropStationNo);
            Assert.Equal(timerSec, slmpMessage.TimerSec);
        }

        /// <summary>
        /// 無効なネットワーク番号とステーション番号の組み合わせを渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData(1, 255)]
        [InlineData(0, 254)]
        public void Constructor_InvalidNetworkAndStationNo_ThrowsArgumentException(ushort reqNetWorkNo, ushort reqStationNo)
        {
            // Arrange
            MessageType messageType = MessageType.Binary;
            PLCType plcType = PLCType.Mitsubishi_R_Series;
            DeviceAccessType devAccessType = DeviceAccessType.Bit;
            RequestDestModuleIOType reqIOType = RequestDestModuleIOType.OwnStationCPU;
            ushort multiDropStationNo = 0;
            double timerSec = 1.0;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new SLMPMessage(messageType, plcType, devAccessType, reqNetWorkNo, reqStationNo, reqIOType, multiDropStationNo, timerSec));
        }

        /// <summary>
        /// 有効なパラメータでメッセージを作成する場合、正しいバイナリメッセージが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData("D100", 10)]
        [InlineData("M200", 20)]
        public void CreateMessage_ValidParameters_ReturnsCorrectBinaryMessage(string rawAddress, ushort numOfDevPoints)
        {
            // Arrange
            var slmpMessage = new SLMPMessage(MessageType.Binary, PLCType.Mitsubishi_R_Series, DeviceAccessType.Word, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0);

            // Act
            var result = slmpMessage.CreateMessage(rawAddress, numOfDevPoints);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        /// <summary>
        /// 有効なパラメータでメッセージを作成する場合、正しいASCIIメッセージが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData("D100", 10)]
        [InlineData("M200", 20)]
        public void CreateMessage_ValidParameters_ReturnsCorrectASCIIMessage(string rawAddress, ushort numOfDevPoints)
        {
            // Arrange
            var slmpMessage = new SLMPMessage(MessageType.ASCII, PLCType.Mitsubishi_Q_Series, DeviceAccessType.Word, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0);

            // Act
            var result = slmpMessage.CreateMessage(rawAddress, numOfDevPoints);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        /// <summary>
        /// 同じプロパティ値を持つSLMPMessageオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameProperties_ReturnsTrue()
        {
            // Arrange
            var obj1 = new SLMPMessage(MessageType.Binary, PLCType.Mitsubishi_R_Series, DeviceAccessType.Word, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0);
            var obj2 = new SLMPMessage(MessageType.Binary, PLCType.Mitsubishi_R_Series, DeviceAccessType.Word, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるプロパティ値を持つSLMPMessageオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentProperties_ReturnsFalse()
        {
            // Arrange
            var obj1 = new SLMPMessage(MessageType.Binary, PLCType.Mitsubishi_R_Series, DeviceAccessType.Word, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0);
            var obj2 = new SLMPMessage(MessageType.ASCII, PLCType.Mitsubishi_Q_Series, DeviceAccessType.Bit, 0, 255, RequestDestModuleIOType.ControlCPU, 1, 2.0);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じプロパティ値を持つSLMPMessageオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameProperties_ReturnsSameHashCode()
        {
            // Arrange
            var obj1 = new SLMPMessage(MessageType.Binary, PLCType.Mitsubishi_R_Series, DeviceAccessType.Word, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0);
            var obj2 = new SLMPMessage(MessageType.Binary, PLCType.Mitsubishi_R_Series, DeviceAccessType.Word, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるプロパティ値を持つSLMPMessageオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentProperties_ReturnsDifferentHashCode()
        {
            // Arrange
            var obj1 = new SLMPMessage(MessageType.Binary, PLCType.Mitsubishi_R_Series, DeviceAccessType.Word, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0);
            var obj2 = new SLMPMessage(MessageType.ASCII, PLCType.Mitsubishi_Q_Series, DeviceAccessType.Bit, 0, 255, RequestDestModuleIOType.ControlCPU, 1, 2.0);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}




