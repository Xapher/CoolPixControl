using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolPixControl
{
    public class OperationPacket : PTPPacket
    {
        byte[] unknownListPictureBytes = new byte[] { 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0x0, 0x0 };

        public string thumbId { get; set; }
        public int dataLegnth { get; set; }
        public override void buildBytes()
        {
            foreach (byte b in BitConverter.GetBytes(packetType))
            {
                data.Add(b);
            }


            if (operationCode != (UInt16)NikonOperationCodes.EndDataTransfer)
            {
                foreach (byte b in BitConverter.GetBytes(1))// Data phase. Always 1? i don't know
                {
                    data.Add(b);
                }
            }


            foreach (byte b in BitConverter.GetBytes(operationCode))
            {
                data.Add(b);
            }

            foreach (byte b in BitConverter.GetBytes(PacketParser.getTID()))
            {
                data.Add(b);
            }

            switch(operationCode)
            {
                case (UInt16)NikonOperationCodes.RequestListOfPictures:
                    foreach (byte b in unknownListPictureBytes)
                    {
                        data.Add(b);
                    }
                    break;
                case (UInt16)NikonOperationCodes.RequestThumb:
                    if(!string.IsNullOrEmpty(thumbId))
                    {
                        foreach (char c in thumbId)
                        {
                            data.Add(((byte)c));
                        }
                    }
                    break;
                case (UInt16)NikonOperationCodes.RequestFullPicture:
                    foreach (byte b in PacketParser.getBytesFromFileID(Program.getSelectedThumbnail()))
                    {
                        data.Add(b);
                    }
                    foreach (byte b in BitConverter.GetBytes(dataLegnth))//I don't know, Possibly totalData lenghth, can only increast by 1.004? mB
                    {
                        data.Add(b);
                    }
                    foreach (byte b in BitConverter.GetBytes(Program.dataSize))//Max Data Size in packet
                    {
                        data.Add(b);
                    }
                    break;
                case (UInt16)NikonOperationCodes.EndDataTransfer:
                    foreach (byte b in BitConverter.GetBytes(PacketParser.getTID() - 1))//end the Previous transaction
                    {
                        data.Add(b);
                    }
                    break;
            }


            foreach (byte b in BitConverter.GetBytes(data.Count + 4).Reverse())
            {
                data.Insert(0, b);
            }
        }

    }
}
