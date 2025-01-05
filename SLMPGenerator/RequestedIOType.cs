using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator
{


    public enum RequestedIOType
    {
        LocalStation = 1023,
        ManagementCPU = 1023,
        ControlProcessor = 976,
        StandbyProcessor = 977,
        ProcessorA = 978,
        ProcessorB = 979,
        MultiProcessor1 = 992,
        MultiProcessor2 = 993,
        MultiProcessor3 = 994,
        MultiProcessor4 = 995
    }
}
