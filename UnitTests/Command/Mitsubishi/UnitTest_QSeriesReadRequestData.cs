using SLMPGenerator.Command;
using SLMPGenerator.Command.Mitsubishi;
using SLMPGenerator.Command.Read;
using Xunit;

namespace SLMPGenerator.Tests.Command.Mitsubishi
{
    public class UnitTest_QSeriesReadRequestData
    {
        /// <summary>
        /// WordUnitReadDataを使用してコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(0x1234, 10, new byte[] { 0x01, 0x04, 0x00, 0x00, 0x34, 0x12, 0x00, 0x00, 0xA8, 0x0a, 0x00 }, "04010000D*00001234000A")]
        [InlineData(0x5678, 20, new byte[] { 0x01, 0x04, 0x00, 0x00, 0x78, 0x56, 0x00, 0x00, 0xA8, 0x14, 0x00 }, "04010000D*000056780014")]
        public void Constructor_WithWordUnitReadData_SetsBinaryCodeAndASCIICode(ushort address, ushort numberOfPoints, byte[] expectedBinaryCode, string expectedASCIICode)
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec);
            var wordRead = new WordUnitReadData(deviceCode, address, numberOfPoints);

            // Act
            var requestData = new QSeriesReadRequestData(deviceCode, wordRead);

            // Assert
            Assert.Equal(expectedBinaryCode, requestData.BinaryCode);
            Assert.Equal(expectedASCIICode, requestData.ASCIICode);
        }

        /// <summary>
        /// BitUnitReadDataを使用してコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(0x1234, 10, new byte[] { 0x01, 0x04, 0x01, 0x00, 0x34, 0x12, 0x00, 0x00, 0xA8, 0x0a, 0x00 }, "04010001D*00001234000A")]
        [InlineData(0x5678, 20, new byte[] { 0x01, 0x04, 0x01, 0x00, 0x78, 0x56, 0x00, 0x00, 0xA8, 0x14, 0x00 }, "04010001D*000056780014")]
        public void Constructor_WithBitUnitReadData_SetsBinaryCodeAndASCIICode(ushort address, ushort numberOfPoints, byte[] expectedBinaryCode, string expectedASCIICode)
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Bit, DeviceNoRange.Dec);
            var bitRead = new BitUnitReadData(deviceCode, address, numberOfPoints);

            // Act
            var requestData = new QSeriesReadRequestData(deviceCode, bitRead);

            // Assert
            Assert.Equal(expectedBinaryCode, requestData.BinaryCode);
            Assert.Equal(expectedASCIICode, requestData.ASCIICode);
        }

        /// <summary>
        /// 同じASCIICodeを持つQSeriesReadRequestDataオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameASCIICode_ReturnsTrue()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var wordRead = new WordUnitReadData(deviceCode, 0x1234, 10);
            var obj1 = new QSeriesReadRequestData(deviceCode, wordRead);
            var obj2 = new QSeriesReadRequestData(deviceCode, wordRead);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるASCIICodeを持つQSeriesReadRequestDataオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentASCIICode_ReturnsFalse()
        {
            // Arrange
            var deviceCode1 = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var wordRead1 = new WordUnitReadData(deviceCode1, 0x1234, 10);
            var obj1 = new QSeriesReadRequestData(deviceCode1, wordRead1);

            var deviceCode2 = new DeviceCode(new byte[] { 0x02 }, "02", DeviceType.Word, DeviceNoRange.Dec);
            var wordRead2 = new WordUnitReadData(deviceCode2, 0x5678, 20);
            var obj2 = new QSeriesReadRequestData(deviceCode2, wordRead2);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じASCIICodeを持つQSeriesReadRequestDataオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameASCIICode_ReturnsSameHashCode()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var wordRead = new WordUnitReadData(deviceCode, 0x1234, 10);
            var obj1 = new QSeriesReadRequestData(deviceCode, wordRead);
            var obj2 = new QSeriesReadRequestData(deviceCode, wordRead);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるASCIICodeを持つQSeriesReadRequestDataオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentASCIICode_ReturnsDifferentHashCode()
        {
            // Arrange
            var deviceCode1 = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var wordRead1 = new WordUnitReadData(deviceCode1, 0x1234, 10);
            var obj1 = new QSeriesReadRequestData(deviceCode1, wordRead1);

            var deviceCode2 = new DeviceCode(new byte[] { 0x02 }, "02", DeviceType.Word, DeviceNoRange.Dec);
            var wordRead2 = new WordUnitReadData(deviceCode2, 0x5678, 20);
            var obj2 = new QSeriesReadRequestData(deviceCode2, wordRead2);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}




