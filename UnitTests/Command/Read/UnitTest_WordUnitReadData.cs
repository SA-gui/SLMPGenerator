using SLMPGenerator.Command;
using SLMPGenerator.Command.Read;
using Xunit;

namespace UnitTests.Command.Read
{
    public class UnitTest_WordUnitReadData
    {
        /// <summary>
        /// コンストラクタに有効なパラメータを渡した場合、プロパティが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(1234, 10)]
        [InlineData(5678, 20)]
        public void Constructor_ValidParameters_SetsProperties(ushort address, ushort numberOfPoints)
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);

            // Act
            var wordUnitReadData = new WordUnitReadData(deviceCode, address, numberOfPoints);

            // Assert
            Assert.Equal(deviceCode, wordUnitReadData.DeviceCode);
            Assert.Equal(address, wordUnitReadData.StartAddress);
            Assert.Equal(numberOfPoints, wordUnitReadData.NumberOfDevicePoints);
        }

        /// <summary>
        /// コンストラクタにnullのDeviceCodeを渡した場合、ArgumentNullExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_NullDeviceCode_ThrowsArgumentNullException()
        {
            // Arrange
            DeviceCode deviceCode = null;
            ushort address = 1234;
            ushort numberOfPoints = 10;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new WordUnitReadData(deviceCode, address, numberOfPoints));
        }

        /// <summary>
        /// コンストラクタに無効なnumberOfPointsを渡した場合、ArgumentOutOfRangeExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_InvalidNumberOfPoints_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new WordUnitReadData(deviceCode, 1234, 0));
        }

        /// <summary>
        /// 同じプロパティ値を持つWordUnitReadDataオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameProperties_ReturnsTrue()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var obj1 = new WordUnitReadData(deviceCode, 1234, 10);
            var obj2 = new WordUnitReadData(deviceCode, 1234, 10);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるプロパティ値を持つWordUnitReadDataオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentProperties_ReturnsFalse()
        {
            // Arrange
            var deviceCode1 = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var deviceCode2 = new DeviceCode(new byte[] { 0x02 }, "02", DeviceType.Word, DeviceNoRange.Dec);
            var obj1 = new WordUnitReadData(deviceCode1, 1234, 10);
            var obj2 = new WordUnitReadData(deviceCode2, 5678, 20);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じプロパティ値を持つWordUnitReadDataオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameProperties_ReturnsSameHashCode()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var obj1 = new WordUnitReadData(deviceCode, 1234, 10);
            var obj2 = new WordUnitReadData(deviceCode, 1234, 10);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるプロパティ値を持つWordUnitReadDataオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentProperties_ReturnsDifferentHashCode()
        {
            // Arrange
            var deviceCode1 = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var deviceCode2 = new DeviceCode(new byte[] { 0x02 }, "02", DeviceType.Word, DeviceNoRange.Dec);
            var obj1 = new WordUnitReadData(deviceCode1, 1234, 10);
            var obj2 = new WordUnitReadData(deviceCode2, 5678, 20);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}



