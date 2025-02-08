using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolPixControl
{
    public static class DefaultPackets
    {
        static string hostName = "wmu/1.6.2.3001 (Android OS 10)";
        static readonly byte[] gguid = new byte[] { 0, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff };
        static readonly byte[] version = new byte[] { 0, 0, 1, 0 };
        static Dictionary<RequestType, int> packetType = new Dictionary<RequestType, int>()
        {
            {RequestType.InitCommandRequestGuid, 1 },
            {RequestType.InitCommandACKPacket, 2},
            {RequestType.InitEventRequestPacketConnection, 3},
            {RequestType.InitEventACK, 4 },
            {RequestType.GetDeviceInfo, 6 },//MAY BE REQUEST OPERATION
            {RequestType.StartDataPacket, 9 },
            {RequestType.EndPacket, 0x0c },
            {RequestType.DeviceInfoResponse, 0x00640000 },
            {RequestType.OperationResponse, 7 },//?????? WHTAT IS THIS
            {RequestType.OpenSession, 6 },
            {RequestType.ProbeRequestPacket, 0x0000000d },
            {RequestType.ProbeResponsePacket, 0x0000000e }
        };



        public static Dictionary<string, PTPPacket> initPackets = new Dictionary<string, PTPPacket>() {
            {
                "SendGGUID", new NonOperationPacket() {
                    hostName = "wmu/1.6.2.3001 (Android OS 10)",
                    packetType = packetType[RequestType.InitCommandRequestGuid]
                }
            },
                {
                "Request Connection", new NonOperationPacket()
                {
                    hostName = "",
                    packetType = packetType[RequestType.InitEventRequestPacketConnection]
                }
            },

            {
                "GetDeviceInfo", new OperationPacket()
                {
                    hostName = "",
                    packetType = packetType[RequestType.GetDeviceInfo],
                    operationCode = (UInt16)NikonOperationCodes.GetDeviceInfo
                    //_PacketBytes = new List<byte>(){ 18, 0, 0, 0, 6, 0, 0, 0, 1, 0, 0, 0, 1, 0x10, 1, 0, 0, 0}
                }
            },
            {
                "OpenSession", new OperationPacket(){
                    hostName = "",
                    packetType = packetType[RequestType.GetDeviceInfo],
                    operationCode = (UInt16)NikonOperationCodes.OpenSession
                    //_PacketBytes = new List<byte>(){ 0x16, 0, 0, 0, 6, 0, 0, 0, 1, 0, 0, 0, 0x02, 0x10, 1, 0, 0, 0}
                }
            },
            {
                "GetPictureList", new OperationPacket(){
                    hostName = "",
                    packetType = 6,
                    operationCode = (UInt16)NikonOperationCodes.RequestListOfPictures
                    //_PacketBytes = new List<byte>(){ 0x1e, 0, 0, 0, 6, 0, 0, 0, 1, 0, 0, 0, 0x07, 0x10, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0x19, 0x11 }
                }
            },
            {
                "RequestPicture", new PTPPacket(){
                    hostName = "",
                    packetType = 6,
                    //_PacketBytes = new List<byte>(){ 0x16, 0, 0, 0, 0x06, 0, 0, 0, 1, 0, 0, 0, 0xc4, 0x90, 1, 0, 0, 0, 0x7e, 0x08, 0x19, 0x29 }
                }
            },
            {
                "RequestFullPicture", new PTPPacket(){
                    hostName = "",
                    packetType = 6,
                    //_PacketBytes = new List<byte>(){ 0x1e, 0, 0, 0, 0x06, 0, 0, 0, 1, 0, 0, 0, 0x1b, 0x10, 1, 0, 0, 0, 0x7e, 0x08, 0x19, 0x29, 0, 0, 0, 0, 0, 0, 0x10, 0 }
                }
            }


        };




        public static byte[] getVer ()
        {
            return version;
        }

        public static byte[] testSync = new byte[] { };

        public static byte[] getGUID()
        {
            return gguid;
        }
    }
}
