using SLMPGenerator.Common;
using SLMPGenerator.Common.Command;
using SLMPGenerator.Read.Domain;
using SLMPGenerator.Read.Domain.ReadData;
using Xunit;

namespace UnitTests.Read.Domain
{
    public class UnitTest_ReadRequestData
    {
        /// <summary>
        /// BitUnitReadDataを使用してコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(1234, 10, new byte[] { 0x01, 0x04, 0x01, 0x00, 0xD2, 0x04, 0x00, 0xA8, 0x0A, 0x00 }, "04010001D*001234000A")]
        [InlineData(5678, 20, new byte[] { 0x01, 0x04, 0x01, 0x00, 0x2E, 0x16, 0x00, 0xA8, 0x14, 0x00 }, "04010001D*0056780014")]
        public void Constructor_WithBitUnitReadData_SetsBinaryCodeAndASCIICode(ushort address, ushort numberOfPoints, byte[] expectedBinaryCode, string expectedASCIICode)
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Bit, DeviceNoRange.Dec);
            var bitRead = new BitUnitReadData(deviceCode, address, numberOfPoints);

            // Act
            var requestData = new ReadRequest(deviceCode, bitRead);

            // Assert
            Assert.Equal(expectedBinaryCode, requestData.BinaryCode);
            Assert.Equal(expectedASCIICode, requestData.ASCIICode);
        }

        /// <summary>
        /// WordUnitReadDataを使用してコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(1234, 10, new byte[] { 0x01, 0x04, 0x00, 0x00, 0xD2, 0x04, 0x00, 0xA8, 0x0A, 0x00 }, "04010000D*001234000A")]
        [InlineData(5678, 20, new byte[] { 0x01, 0x04, 0x00, 0x00, 0x2E, 0x16, 0x00, 0xA8, 0x14, 0x00 }, "04010000D*0056780014")]
        public void Constructor_WithWordUnitReadData_SetsBinaryCodeAndASCIICode(ushort address, ushort numberOfPoints, byte[] expectedBinaryCode, string expectedASCIICode)
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec);
            var wordRead = new WordUnitReadData(deviceCode, address, numberOfPoints);

            // Act
            var requestData = new ReadRequest(deviceCode, wordRead);

            // Assert
            Assert.Equal(expectedBinaryCode, requestData.BinaryCode);
            Assert.Equal(expectedASCIICode, requestData.ASCIICode);
        }

        /// <summary>
        /// 同じASCIICodeを持つReadRequestDataオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameASCIICode_ReturnsTrue()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec);
            var wordRead = new WordUnitReadData(deviceCode, 1234, 10);
            var obj1 = new ReadRequest(deviceCode, wordRead);
            var obj2 = new ReadRequest(deviceCode, wordRead);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるASCIICodeを持つReadRequestDataオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentASCIICode_ReturnsFalse()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec);
            var wordRead1 = new WordUnitReadData(deviceCode, 1234, 10);
            var wordRead2 = new WordUnitReadData(deviceCode, 5678, 20);
            var obj1 = new ReadRequest(deviceCode, wordRead1);
            var obj2 = new ReadRequest(deviceCode, wordRead2);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じASCIICodeを持つReadRequestDataオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameASCIICode_ReturnsSameHashCode()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec);
            var wordRead = new WordUnitReadData(deviceCode, 1234, 10);
            var obj1 = new ReadRequest(deviceCode, wordRead);
            var obj2 = new ReadRequest(deviceCode, wordRead);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるASCIICodeを持つReadRequestDataオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentASCIICode_ReturnsDifferentHashCode()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec);
            var wordRead1 = new WordUnitReadData(deviceCode, 1234, 10);
            var wordRead2 = new WordUnitReadData(deviceCode, 5678, 20);
            var obj1 = new ReadRequest(deviceCode, wordRead1);
            var obj2 = new ReadRequest(deviceCode, wordRead2);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}

