using System;
using System.Collections.Generic;
using Xunit;
using SLMPGenerator.Command.Write;
using SLMPGenerator.Command;

namespace SLMPGenerator.Tests.Command.Write
{
    /// <summary>
    /// WordUnitWriteData�N���X�̒P�̃e�X�g��񋟂��܂��B
    /// </summary>
    public class UnitTest_WordUnitWriteData
    {
        /// <summary>
        /// �R���X�g���N�^�̃e�X�g���s���܂�: ����Ȉ����B
        /// </summary>
        /// <param name="deviceType">�f�o�C�X�^�C�v</param>
        /// <param name="deviceNoRange">�f�o�C�X�ԍ��͈�</param>
        /// <param name="startAddress">�J�n�A�h���X</param>
        /// <param name="writeDataList">�������݃f�[�^���X�g</param>
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
        /// �R���X�g���N�^�̃e�X�g���s���܂�: null��DeviceCode�B
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
        /// �R���X�g���N�^�̃e�X�g���s���܂�: null��WriteDataList�B
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
        /// Equals���\�b�h�̃e�X�g���s���܂�: �����I�u�W�F�N�g�B
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
        /// Equals���\�b�h�̃e�X�g���s���܂�: �قȂ�I�u�W�F�N�g�B
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