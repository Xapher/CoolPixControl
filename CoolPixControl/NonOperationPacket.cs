using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolPixControl
{
    internal class NonOperationPacket : PTPPacket
    {
        public override void buildBytes()
        {
            if(!string.IsNullOrEmpty(hostName))
            {
                foreach (byte b in BitConverter.GetBytes(packetType))
                {
                    data.Add(b);
                }
                foreach (byte b in DefaultPackets.getGUID())
                {
                    data.Add(b);
                }
                foreach (byte b in Encoding.Unicode.GetBytes(hostName))
                {
                    data.Add(b);
                }
                foreach (byte b in DefaultPackets.getVer())
                {
                    data.Add(b);
                }
            }
            else
            {
                foreach (byte b in BitConverter.GetBytes(packetType))
                {
                    data.Add(b);
                }
                foreach (byte b in BitConverter.GetBytes(PacketParser.getConNum()))
                {
                    data.Add(b);
                }
            }

            foreach (byte b in BitConverter.GetBytes(data.Count + 4).Reverse())
            {
                data.Insert(0, b);
            }
        }
    }
}
