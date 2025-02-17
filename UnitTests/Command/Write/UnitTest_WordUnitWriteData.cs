using System;
using System.Collections.Generic;
using Xunit;
using SLMPGenerator.Command.Write;
using SLMPGenerator.Command;

namespace SLMPGenerator.Tests.Command.Write
{
    /// <summary>
    /// WordUnitWriteDataクラスの単体テストを提供します。
    /// </summary>
    public class UnitTest_WordUnitWriteData
    {
        /// <summary>
        /// コンストラクタのテストを行います: 正常な引数。
        /// </summary>
        /// <param name="deviceType">デバイスタイプ</param>
        /// <param name="deviceNoRange">デバイス番号範囲</param>
        /// <param name="startAddress">開始アドレス</param>
        /// <param name="writeDataList">書き込みデータリスト</param>
        [Theory]
        [InlineData(DeviceType.Word, DeviceNoRange.Dec, 0, new short[] { 1, 2, 3 })]
        [InlineData(DeviceType.Word, DeviceNoRange.Hex, 65535, new short[] { -1, 0, 32767 })]
        public void Constructor_ValidArguments_ShouldInitializeProperties(DeviceType deviceType, DeviceNoRange deviceNoRange, ushort startAddress, short[] writeDataList)
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x00 }, "D", deviceType, deviceNoRange);

            // Act
            var wordUnitWriteData = new WordUnitWriteData(deviceCode, startAddress, writeDataList);

            // Assert
            Assert.Equal(deviceCode, wordUnitWriteData.DeviceCode);
            Assert.Equal(startAddress, wordUnitWriteData.StartAddress);
            Assert.Equal(writeDataList.Length, wordUnitWriteData.NumberOfDevicePoints);
            Assert.Equal(writeDataList, wordUnitWriteData.WriteDataList);
        }

        /// <summary>
        /// コンストラクタのテストを行います: nullのDeviceCode。
        /// </summary>
        [Fact]
        public void Constructor_NullDeviceCode_ShouldThrowArgumentNullException()
        {
            // Arrange
            DeviceCode deviceCode = null;
            ushort startAddress = 0;
            var writeDataList = new List<short> { 1, 2, 3 };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new WordUnitWriteData(deviceCode, startAddress, writeDataList));
        }

        /// <summary>
        /// コンストラクタのテストを行います: nullのWriteDataList。
        /// </summary>
        [Fact]
        public void Constructor_NullWriteDataList_ShouldThrowArgumentNullException()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x00 }, "D", DeviceType.Word, DeviceNoRange.Dec);
            ushort startAddress = 0;
            List<short> writeDataList = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new WordUnitWriteData(deviceCode, startAddress, writeDataList));
        }

        /// <summary>
        /// Equalsメソッドのテストを行います: 同じオブジェクト。
        /// </summary>
        [Fact]
        public void Equals_SameObject_ShouldReturnTrue()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x00 }, "D", DeviceType.Word, DeviceNoRange.Dec);
            var writeDataList = new List<short> { 1, 2, 3 };
            var wordUnitWriteData1 = new WordUnitWriteData(deviceCode, 0, writeDataList);
            var wordUnitWriteData2 = new WordUnitWriteData(deviceCode, 0, writeDataList);

            // Act
            var result = wordUnitWriteData1.Equals(wordUnitWriteData2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Equalsメソッドのテストを行います: 異なるオブジェクト。
        /// </summary>
        [Fact]
        public void Equals_DifferentObject_ShouldReturnFalse()
        {
            // Arrange
            var deviceCode1 = new DeviceCode(new byte[] { 0x00 }, "D", DeviceType.Word, DeviceNoRange.Dec);
            var deviceCode2 = new DeviceCode(new byte[] { 0x01 }, "D", DeviceType.Word, DeviceNoRange.Dec);
            var writeDataList1 = new List<short> { 1, 2, 3 };
            var writeDataList2 = new List<short> { 4, 5, 6 };
            var wordUnitWriteData1 = new WordUnitWriteData(deviceCode1, 0, writeDataList1);
            var wordUnitWriteData2 = new WordUnitWriteData(deviceCode2, 1, writeDataList2);

            // Act
            var result = wordUnitWriteData1.Equals(wordUnitWriteData2);

            // Assert
            Assert.False(result);
        }
    }
}