using SLMPGenerator.Command;
using SLMPGenerator.Command.Mitsubishi;
using Xunit;

namespace SLMPGenerator.Tests.Command.Mitsubishi
{
    public class UnitTest_RSeriesReadRequestData
    {
        /// <summary>
        /// WordUnitAccessDataを使用してコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(0x1234, 10, new byte[] { 0x01, 0x04, 0x02, 0x00, 0x34, 0x12, 0x00, 0x00, 0xA8, 0x00, 0x0a, 0x00 }, "04010002D***00001234000A")]
        [InlineData(0x5678, 20, new byte[] { 0x01, 0x04, 0x02, 0x00, 0x78, 0x56, 0x00, 0x00, 0xA8, 0x00, 0x14, 0x00 }, "04010002D***000056780014")]
        public void Constructor_WithWordUnitAccessData_SetsBinaryCodeAndASCIICode(ushort address, ushort numberOfPoints, byte[] expectedBinaryCode,  string expectedASCIICode)
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x00, 0xA8 }, "D***", DeviceType.Word, DeviceNoRange.Dec);
            var wordAccess = new WordUnitAccessData(deviceCode, address, numberOfPoints);

            // Act
            var requestData = new RSeriesReadRequestData(deviceCode, wordAccess);

            // Assert
            Assert.Equal(expectedBinaryCode, requestData.BinaryCode);
            Assert.Equal(expectedASCIICode, requestData.ASCIICode);
        }

        /// <summary>
        /// BitUnitAccessDataを使用してコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(0x1234, 10, new byte[] { 0x01, 0x04, 0x03, 0x00, 0x34, 0x12, 0x00, 0x00, 0xA8, 0x00, 0x0a, 0x00 }, "04010003D***00001234000A")]
        [InlineData(0x5678, 20, new byte[] { 0x01, 0x04, 0x03, 0x00, 0x78, 0x56, 0x00, 0x00, 0xA8, 0x00, 0x14, 0x00 }, "04010003D***000056780014")]
        public void Constructor_WithBitUnitAccessData_SetsBinaryCodeAndASCIICode(ushort address, ushort numberOfPoints, byte[] expectedBinaryCode, string expectedASCIICode)
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x00, 0xA8 }, "D***", DeviceType.Bit, DeviceNoRange.Dec);
            var bitAccess = new BitUnitAccessData(deviceCode, address, numberOfPoints);

            // Act
            var requestData = new RSeriesReadRequestData(deviceCode, bitAccess);

            // Assert
            Assert.Equal(expectedBinaryCode, requestData.BinaryCode);
            Assert.Equal(expectedASCIICode, requestData.ASCIICode);
        }

        /// <summary>
        /// 同じASCIICodeを持つRSeriesReadRequestDataオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameASCIICode_ReturnsTrue()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var wordAccess = new WordUnitAccessData(deviceCode, 0x1234, 10);
            var obj1 = new RSeriesReadRequestData(deviceCode, wordAccess);
            var obj2 = new RSeriesReadRequestData(deviceCode, wordAccess);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRSeriesReadRequestDataオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentASCIICode_ReturnsFalse()
        {
            // Arrange
            var deviceCode1 = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var wordAccess1 = new WordUnitAccessData(deviceCode1, 0x1234, 10);
            var obj1 = new RSeriesReadRequestData(deviceCode1, wordAccess1);

            var deviceCode2 = new DeviceCode(new byte[] { 0x02 }, "02", DeviceType.Word, DeviceNoRange.Dec);
            var wordAccess2 = new WordUnitAccessData(deviceCode2, 0x5678, 20);
            var obj2 = new RSeriesReadRequestData(deviceCode2, wordAccess2);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じASCIICodeを持つRSeriesReadRequestDataオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameASCIICode_ReturnsSameHashCode()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var wordAccess = new WordUnitAccessData(deviceCode, 0x1234, 10);
            var obj1 = new RSeriesReadRequestData(deviceCode, wordAccess);
            var obj2 = new RSeriesReadRequestData(deviceCode, wordAccess);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRSeriesReadRequestDataオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentASCIICode_ReturnsDifferentHashCode()
        {
            // Arrange
            var deviceCode1 = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var wordAccess1 = new WordUnitAccessData(deviceCode1, 0x1234, 10);
            var obj1 = new RSeriesReadRequestData(deviceCode1, wordAccess1);

            var deviceCode2 = new DeviceCode(new byte[] { 0x02 }, "02", DeviceType.Word, DeviceNoRange.Dec);
            var wordAccess2 = new WordUnitAccessData(deviceCode2, 0x5678, 20);
            var obj2 = new RSeriesReadRequestData(deviceCode2, wordAccess2);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}




