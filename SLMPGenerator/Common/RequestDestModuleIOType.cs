using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{


    public enum RequestDestModuleIOType : ushort
    {
        // 0x0000 - 0x03FF
        // ushortにしているのはBitHelperにて扱うため
        OwnStationCPU = 0x03FF,
        ControlCPU = 0x03FF,
        MultipleSystemCPU1 = 0x03E0,
        MultipleSystemCPU2 = 0x03E1,
        MultipleSystemCPU3 = 0x03E2,
        MultipleSystemCPU4 = 0x03E3,
        MultidropConnectionCPU = 0x0000, // 0x0000 to 0x01FF range not directly representable
        ControlSystemCPU = 0x03D0,
        StandbySystemCPU = 0x03D1,
        SystemACPU = 0x03D2,
        SystemBCPU = 0x03D3,

        CCLinkIE_OwnStation = 0x03FF,
        CCLinkIE_FieldNetworkHead1 = 0x03E0,
        CCLinkIE_FieldNetworkHead2 = 0x03E1,
        CCLinkIE_MultidropConnection = 0x0000, // 0x0000 to 0x01FF range not directly representable
        CCLinkIE_ControlSystem = 0x03D0,
        CCLinkIE_StandbySystem = 0x03D1
    }
}
