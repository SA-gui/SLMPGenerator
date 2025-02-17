using System;
using System.Collections.Generic;
using Xunit;
using SLMPGenerator.Command.Write;
using SLMPGenerator.Command;

namespace SLMPGenerator.Tests.Command.Write
{
    /// <summary>
    /// BitUnitWriteDataクラスの単体テストを提供します。
    /// </summary>
    public class UnitTest_BitUnitWriteData
    {
        /// <summary>
        /// 正常なコンストラクタのテストを行います。
        /// </summary>
        /// <param name="deviceType">デバイスタイプ</param>
        /// <param name="deviceNoRange">デバイス番号範囲</param>
        /// <param name="startAddress">開始アドレス</param>
        /// <param name="writeData">書き込みデータ</param>
        [Theory]
        [InlineData(DeviceType.Bit, DeviceNoRange.Dec, 0, true)]
        [InlineData(DeviceType.Bit, DeviceNoRange.Hex, 65535, false)]
        public void Constructor_ValidParameters_ShouldInitializeProperties(DeviceType deviceType, DeviceNoRange deviceNoRange, ushort startAddress, bool writeData)
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x00 }, "D", deviceType, deviceNoRange);

            // Act
            var bitUnitWriteData = new BitUnitWriteData(deviceCode, startAddress, writeData);

            // Assert
            Assert.Equal(deviceCode, bitUnitWriteData.DeviceCode);
            Assert.Equal(startAddress, bitUnitWriteData.StartAddress);
            Assert.Equal(1, bitUnitWriteData.NumberOfDevicePoints);
            Assert.Single(bitUnitWriteData.WriteDataList);
            Assert.Equal(writeData, bitUnitWriteData.WriteDataList[0]);
        }

        /// <summary>
        /// 無効なコンストラクタのテストを行います（deviceCodeがnullの場合）。
        /// </summary>
        [Fact]
        public void Constructor_NullDeviceCode_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new BitUnitWriteData(null, 0, true));
        }

        /// <summary>
        /// 無効なコンストラクタのテストを行います（writeDataListがnullの場合）。
        /// </summary>
        [Fact]
        public void Constructor_NullWriteDataList_ShouldThrowArgumentNullException()
        {
            // Assert
            var deviceCode = new DeviceCode(new byte[] { 0x00 }, "D", DeviceType.Bit, DeviceNoRange.Dec);
            Assert.Throws<ArgumentNullException>(() => new BitUnitWriteData(deviceCode, 0, null));
        }

        /// <summary>
        /// Equalsメソッドのテストを行います（同一オブジェクトの場合）。
        /// </summary>
        [Fact]
        public void Equals_Same_Object_ShouldReturnTrue()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x00 }, "D", DeviceType.Bit, DeviceNoRange.Dec);
            var bitUnitWriteData1 = new BitUnitWriteData(deviceCode, 0, new List<bool> { true, false });
            var bitUnitWriteData2 = new BitUnitWriteData(deviceCode, 0, new List<bool> { true, false });

            // Assert
            Assert.True(bitUnitWriteData1.Equals(bitUnitWriteData2));
        }

        /// <summary>
        /// Equalsメソッドのテストを行います（異なるオブジェクトの場合）。
        /// </summary>
        [Fact]
        public void Equals_DifferentObject_ShouldReturnFalse()
        {
            // Arrange
            var deviceCode1 = new DeviceCode(new byte[] { 0x00 }, "D", DeviceType.Bit, DeviceNoRange.Dec);
            var deviceCode2 = new DeviceCode(new byte[] { 0x01 }, "D", DeviceType.Bit, DeviceNoRange.Dec);
            var bitUnitWriteData1 = new BitUnitWriteData(deviceCode1, 0, new List<bool> { true, false });
            var bitUnitWriteData2 = new BitUnitWriteData(deviceCode2, 0, new List<bool> { true, false });

            // Assert
            Assert.False(bitUnitWriteData1.Equals(bitUnitWriteData2));
        }
    }
}
