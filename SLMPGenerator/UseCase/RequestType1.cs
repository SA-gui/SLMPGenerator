using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.UseCase
{
    public enum RequestType1
    {
        Device_Read,
        Device_Write,
        Device_ReadRandom,
        Device_WriteRandom,
        Device_EntryMonitorDevice,
        Device_ExecuteMonitor,
        Device_ReadBlock,
        Device_WriteBlock,

        Label_ArrayLabelRead,
        Label_ArrayLabelWrite,
        Label_ArrayLabelReadRandom,
        Label_ArrayLabelWriteRandom,

        Memory_Read,
        Memory_Write,

        ExtendUnit_Read,
        ExtendUnit_Write,

        RemoteControl_RemoteRun,
        RemoteControl_RemoteStop,
        RemoteControl_RemotePause,
        RemoteControl_RemoteLatchClear,
        RemoteControl_RemoteReset,
        RemoteControl_ReadTypeName,

        RemotePassword_Lock,
        RemotePassword_Unlock,

        File_ReadDirectory_File,
        File_SearchDirectory_File,
        File_NewFile,
        File_DeleteFile,
        File_CopyFile,
        File_ChangeFileState,
        File_ChangeFileDelete,
        File_OpenFile,
        File_ReadFile,
        File_WriteFile,
        File_CloseFile,

        SelfTest,

        ClearError,

        Ondemand

    }
}
