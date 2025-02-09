using SLMPGenerator.Command;
using Xunit;

namespace SLMPGenerator.Tests.Command
{
    public class UnitTest_DeviceCode
    {
        /// <summary>
        /// コンストラクタに有効なパラメータを渡した場合、プロパティが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(new byte[] { 0x01, 0x02 }, "0201", DeviceType.Bit, DeviceNoRange.Dec)]
        [InlineData(new byte[] { 0x03, 0x04 }, "0403", DeviceType.Word, DeviceNoRange.Hex)]
        internal void Constructor_ValidParameters_SetsProperties(byte[] binaryCode, string asciiCode, DeviceType deviceType, DeviceNoRange deviceNoRange)
        {
            // Arrange & Act
            var deviceCode = new DeviceCode(binaryCode, asciiCode, deviceType, deviceNoRange);

            // Assert
            Assert.Equal(binaryCode.Reverse().ToArray(), deviceCode.BinaryCode);
            Assert.Equal(asciiCode, deviceCode.ASCIICode);
            Assert.Equal(deviceType, deviceCode.DeviceType);
            Assert.Equal(deviceNoRange, deviceCode.DeviceNoRange);
        }

        /// <summary>
        /// コンストラクタにnullのbinaryCodeを渡した場合、ArgumentNullExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_NullBinaryCode_ThrowsArgumentNullException()
        {
            // Arrange
            byte[] binaryCode = null;
            string asciiCode = "0201";
            DeviceType deviceType = DeviceType.Bit;
            DeviceNoRange deviceNoRange = DeviceNoRange.Dec;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DeviceCode(binaryCode, asciiCode, deviceType, deviceNoRange));
        }

        /// <summary>
        /// コンストラクタにnullのasciiCodeを渡した場合、ArgumentNullExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_NullAsciiCode_ThrowsArgumentNullException()
        {
            // Arrange
            byte[] binaryCode = new byte[] { 0x01, 0x02 };
            string asciiCode = null;
            DeviceType deviceType = DeviceType.Bit;
            DeviceNoRange deviceNoRange = DeviceNoRange.Dec;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DeviceCode(binaryCode, asciiCode, deviceType, deviceNoRange));
        }

        /// <summary>
        /// 同じASCIICodeを持つDeviceCodeオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameASCIICode_ReturnsTrue()
        {
            // Arrange
            var obj1 = new DeviceCode(new byte[] { 0x01, 0x02 }, "0201", DeviceType.Bit, DeviceNoRange.Dec);
            var obj2 = new DeviceCode(new byte[] { 0x01, 0x02 }, "0201", DeviceType.Bit, DeviceNoRange.Dec);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるASCIICodeを持つDeviceCodeオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentASCIICode_ReturnsFalse()
        {
            // Arrange
            var obj1 = new DeviceCode(new byte[] { 0x01, 0x02 }, "0201", DeviceType.Bit, DeviceNoRange.Dec);
            var obj2 = new DeviceCode(new byte[] { 0x03, 0x04 }, "0403", DeviceType.Word, DeviceNoRange.Hex);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じASCIICodeを持つDeviceCodeオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameASCIICode_ReturnsSameHashCode()
        {
            // Arrange
            var obj1 = new DeviceCode(new byte[] { 0x01, 0x02 }, "0201", DeviceType.Bit, DeviceNoRange.Dec);
            var obj2 = new DeviceCode(new byte[] { 0x01, 0x02 }, "0201", DeviceType.Bit, DeviceNoRange.Dec);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるASCIICodeを持つDeviceCodeオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentASCIICode_ReturnsDifferentHashCode()
        {
            // Arrange
            var obj1 = new DeviceCode(new byte[] { 0x01, 0x02 }, "0201", DeviceType.Bit, DeviceNoRange.Dec);
            var obj2 = new DeviceCode(new byte[] { 0x03, 0x04 }, "0403", DeviceType.Word, DeviceNoRange.Hex);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}



