using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolPixControl
{
    internal enum RequestType
    {
        blank,
        InitCommandRequestGuid,
        InitCommandACKPacket,
        InitEventRequestPacketConnection,
        InitEventACK,
        GetDeviceInfo,
        StartDataPacket,
        EndPacket,
        OperationResponse,
        ProbeRequestPacket,
        ProbeResponsePacket,
        DeviceInfoResponse,
        OpenSession
    }
}
