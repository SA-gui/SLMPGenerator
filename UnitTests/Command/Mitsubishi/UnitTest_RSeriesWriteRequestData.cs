﻿using SLMPGenerator.Command.Mitsubishi;
using SLMPGenerator.Command.Read;
using SLMPGenerator.Command;
using System;
using Xunit;
using SLMPGenerator.Command.Write;

namespace UnitTests.Command.Mitsubishi
{
    public class UnitTest_RSeriesWriteRequestData
    {
        /// <summary>
        /// WordUnitWriteDataを使用してコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(1234, 10, new byte[] { 0x01, 0x14, 0x02, 0x00, 0xD2, 0x04, 0x00, 0x00, 0xA8, 0x00, 0x01, 0x00, 0x0a, 0x00 }, "14010002D***000012340001000A")]
        [InlineData(5678, 20, new byte[] { 0x01, 0x14, 0x02, 0x00, 0x2E, 0x16, 0x00, 0x00, 0xA8, 0x00, 0x01, 0x00, 0x14, 0x00 }, "14010002D***0000567800010014")]
        public void Constructor_WithWordUnitWriteData_SetsBinaryCodeAndASCIICode(ushort address, short writeData, byte[] expectedBinaryCode, string expectedASCIICode)
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x00, 0xA8 }, "D***", DeviceType.Word, DeviceNoRange.Dec);
            var wordWrite = new WordUnitWriteData(deviceCode, address, writeData);

            // Act
            var requestData = new RSeriesWriteRequestData(deviceCode, wordWrite);

            // Assert
            Assert.Equal(expectedBinaryCode, requestData.BinaryCode);
            Assert.Equal(expectedASCIICode, requestData.ASCIICode);
        }

        /// <summary>
        /// BitUnitWriteDataを使用してコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(1234, false, new byte[] { 0x01, 0x14, 0x03, 0x00, 0xD2, 0x04, 0x00, 0x00, 0xA8, 0x00, 0x01, 0x00, 0x00, 0x00 }, "14010003D***0000123400010000")]
        [InlineData(5678, true, new byte[] { 0x01, 0x14, 0x03, 0x00, 0x2E, 0x16, 0x00, 0x00, 0xA8, 0x00, 0x01, 0x00, 0x01, 0x00 }, "14010003D***0000567800010001")]
        public void Constructor_WithBitUnitWriteData_SetsBinaryCodeAndASCIICode(ushort address, bool writeData, byte[] expectedBinaryCode, string expectedASCIICode)
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x00, 0xA8 }, "D***", DeviceType.Bit, DeviceNoRange.Dec);
            var bitWrite = new BitUnitWriteData(deviceCode, address, writeData);

            // Act
            var requestData = new RSeriesWriteRequestData(deviceCode, bitWrite);

            // Assert
            Assert.Equal(expectedBinaryCode, requestData.BinaryCode);
            Assert.Equal(expectedASCIICode, requestData.ASCIICode);
        }

        /// <summary>
        /// 同じASCIICodeを持つRSeriesWriteRequestDataオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameASCIICode_ReturnsTrue()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var wordWrite = new WordUnitWriteData(deviceCode, 0x1234, 10);
            var obj1 = new RSeriesWriteRequestData(deviceCode, wordWrite);
            var obj2 = new RSeriesWriteRequestData(deviceCode, wordWrite);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRSeriesWriteRequestDataオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentASCIICode_ReturnsFalse()
        {
            // Arrange
            var deviceCode1 = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var wordWrite1 = new WordUnitWriteData(deviceCode1, 0x1234, 10);
            var obj1 = new RSeriesWriteRequestData(deviceCode1, wordWrite1);

            var deviceCode2 = new DeviceCode(new byte[] { 0x02 }, "02", DeviceType.Word, DeviceNoRange.Dec);
            var wordWrite2 = new WordUnitWriteData(deviceCode2, 0x5678, 20);
            var obj2 = new RSeriesWriteRequestData(deviceCode2, wordWrite2);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じASCIICodeを持つRSeriesWriteRequestDataオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameASCIICode_ReturnsSameHashCode()
        {
            // Arrange
            var deviceCode = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var wordWrite = new WordUnitWriteData(deviceCode, 0x1234, 10);
            var obj1 = new RSeriesWriteRequestData(deviceCode, wordWrite);
            var obj2 = new RSeriesWriteRequestData(deviceCode, wordWrite);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRSeriesWriteRequestDataオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentASCIICode_ReturnsDifferentHashCode()
        {
            // Arrange
            var deviceCode1 = new DeviceCode(new byte[] { 0x01 }, "01", DeviceType.Word, DeviceNoRange.Dec);
            var wordWrite1 = new WordUnitWriteData(deviceCode1, 0x1234, 10);
            var obj1 = new RSeriesWriteRequestData(deviceCode1, wordWrite1);

            var deviceCode2 = new DeviceCode(new byte[] { 0x02 }, "02", DeviceType.Word, DeviceNoRange.Dec);
            var wordWrite2 = new WordUnitWriteData(deviceCode2, 0x5678, 20);
            var obj2 = new RSeriesWriteRequestData(deviceCode2, wordWrite2);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}
