using SLMPGenerator.Command;
using Xunit;

namespace SLMPGenerator.Tests.Command
{
    public class UnitTest_DoubleWordUnitAccessData
    {
        /// <summary>
        /// コンストラクタに有効なパラメータを渡した場合、プロパティが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(0x1234, 10)]
        [InlineData(0x5678, 20)]
        public void Constructor_ValidParameters_SetsProperties(ushort address, ushort numberOfDevPoints)
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.DoubleWord, DeviceNoRange.Dec);

            // Act
            var doubleWordUnitAccessData = new DoubleWordUnitAccessData(deviceCode, address, numberOfDevPoints);

            // Assert
            Assert.Equal(deviceCode, doubleWordUnitAccessData.DeviceCode);
            Assert.Equal(address, doubleWordUnitAccessData.Address);
            Assert.Equal(numberOfDevPoints, doubleWordUnitAccessData.NumberOfDevicePoints);
        }

        /// <summary>
        /// コンストラクタにnullのDeviceCodeを渡した場合、ArgumentNullExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_NullDeviceCode_ThrowsArgumentNullException()
        {
            // Arrange
            DeviceCode deviceCode = null;
            ushort address = 0x1234;
            ushort numberOfDevPoints = 10;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DoubleWordUnitAccessData(deviceCode, address, numberOfDevPoints));
        }

        /// <summary>
        /// 同じプロパティ値を持つDoubleWordUnitAccessDataオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameProperties_ReturnsTrue()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.DoubleWord, DeviceNoRange.Dec);
            var obj1 = new DoubleWordUnitAccessData(deviceCode, 0x1234, 10);
            var obj2 = new DoubleWordUnitAccessData(deviceCode, 0x1234, 10);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるプロパティ値を持つDoubleWordUnitAccessDataオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentProperties_ReturnsFalse()
        {
            // Arrange
            var deviceCode1 = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.DoubleWord, DeviceNoRange.Dec);
            var deviceCode2 = new DeviceCode(new byte[] { 0x02 }, "02", DeviceType.DoubleWord, DeviceNoRange.Dec);
            var obj1 = new DoubleWordUnitAccessData(deviceCode1, 0x1234, 10);
            var obj2 = new DoubleWordUnitAccessData(deviceCode2, 0x5678, 20);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じプロパティ値を持つDoubleWordUnitAccessDataオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameProperties_ReturnsSameHashCode()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.DoubleWord, DeviceNoRange.Dec);
            var obj1 = new DoubleWordUnitAccessData(deviceCode, 0x1234, 10);
            var obj2 = new DoubleWordUnitAccessData(deviceCode, 0x1234, 10);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるプロパティ値を持つDoubleWordUnitAccessDataオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentProperties_ReturnsDifferentHashCode()
        {
            // Arrange
            var deviceCode1 = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.DoubleWord, DeviceNoRange.Dec);
            var deviceCode2 = new DeviceCode(new byte[] { 0x02 }, "02", DeviceType.DoubleWord, DeviceNoRange.Dec);
            var obj1 = new DoubleWordUnitAccessData(deviceCode1, 0x1234, 10);
            var obj2 = new DoubleWordUnitAccessData(deviceCode2, 0x5678, 20);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}



