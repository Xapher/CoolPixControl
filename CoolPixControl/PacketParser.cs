using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CoolPixControl
{
    public static class PacketParser
    {
        static MemoryStream ms;
        static BinaryReader r;

        static int length = 0, code = 0, opCode = 0, connectionNum = 0;
        static int transactionId = 0;
        static bool operationOveride = false;
        static NikonResponseCodes OverideType = NikonResponseCodes.ListOfPictureID;
        static List<string> pictureIds = new List<string>();

        static string idBuilder = "";
        static List<byte> data = new List<byte>();

        static List<string> imagesToGet = new List<string>();

        static int dataLength = 0, dataType = 0, dataCode = 0;

        static readonly byte[] okOpPacket = new byte[] { 0x0e, 0, 0, 0, 7, 0, 0, 0, 0x01, 0x20 };
        static byte[] endData = new byte[10];
        static int tempIndex = 0;
        static int thumbnailIndex = 2;


        static bool isProcessing = false;
        static short requestResponse = 0;
        static bool pictureDownload = false;
        static Dictionary<int, NikonOperationCodes> transactionIdCodes = new Dictionary<int, NikonOperationCodes>();

        public static void parsePacket(byte[] b, int length, NikonOperationCodes opcodes)//sender is used for reporting progress
        {
            isProcessing = true;
            //check for any previous packet ending
            ms = new MemoryStream(b);
            r = new BinaryReader(ms);
            if(!operationOveride)
            {
                length = r.ReadInt32();
                code = r.ReadInt32();
                //check code
                if (code == 2 && connectionNum == 0)
                {
                    Console.WriteLine("UID Response");
                    connectionNum = r.ReadInt32();
                }

                Console.WriteLine("Length: " + length);
                Console.WriteLine("Packet Type: " + code);
                Console.WriteLine("Connection Number: " + connectionNum);

                if (code == 7)
                {
                    Console.WriteLine("Found Operation response");
                    requestResponse = r.ReadInt16();
                    Console.WriteLine(requestResponse.ToString("X2"));
                    switch (requestResponse)
                    {
                        case 0:
                            break;
                        case (short)NikonResponseCodes.InvalidObjectHandle:
                            Console.WriteLine("Invalid Object Handle?");
                            Console.WriteLine("Images:" + imagesToGet.Count);
                            if(imagesToGet.Count > 0)
                            {
                                imagesToGet.RemoveAt(0);
                                if(imagesToGet.Count > 0)
                                {
                                    transactionIdCodes.Add(transactionId + 1, NikonOperationCodes.RequestThumb);
                                    Program.addPacketToQueue(new OperationPacket()
                                    {
                                        hostName = "",
                                        packetType = 6,
                                        operationCode = (UInt16)NikonOperationCodes.RequestThumb,
                                        thumbId = imagesToGet[0]
                                    }.getData());
                                }
                            }
                            break;
                    }
                    transactionId++;
                }
                else if (code == 9)
                {

                    for (int i = 0; i < length; i++)
                    {
                        Console.Write(b[i].ToString("X2") + " ");
                    }
                    Console.WriteLine();


                    Console.WriteLine("Found Request Response");
                    r.ReadInt32();///Transid
                    //Console.WriteLine(r.ReadInt16().ToString("X2"));
                    requestResponse = r.ReadInt16();

                    if(length %4 == 0)
                    {
                        Console.WriteLine(length);
                        Console.WriteLine("Possible pic");
                    }

                    Console.WriteLine(requestResponse.ToString("X2"));
                    if(requestResponse > 2000)
                    {
                        Console.WriteLine("Picture");
                        Console.WriteLine(transactionIdCodes[transactionId]);
                        if (transactionIdCodes[transactionId] == NikonOperationCodes.RequestThumb)
                        {
                            Console.WriteLine("Definate Pic");
                            pictureDownload = true;//janky
                        }
                    }

                    if (pictureDownload)
                    {
                        Console.WriteLine("Found Beginning of picture");
                        operationOveride = true;
                        OverideType = NikonResponseCodes.DownloadData;
                        transactionIdCodes.Remove(transactionId);
                        transactionId++;
                    }

                    else
                    {
                        switch (requestResponse)
                        {
                            case (Int16)NikonResponseCodes.ListOfPictureID:
                                Console.WriteLine("Next Packet Containing some or all picture ids");
                                operationOveride = true;
                                OverideType = NikonResponseCodes.ListOfPictureID;
                                transactionId++;
                                break;

                        }
                    }
                    
                }
                isProcessing = false;
            }
            else
            {
                switch(OverideType)
                {
                    case NikonResponseCodes.ListOfPictureID:
                        pictureIds.Clear();
                        imagesToGet.Clear();
                        //Skip start data packet
                        r.ReadInt32();
                        r.ReadInt32();
                        r.ReadInt32();
                        length = r.ReadInt32();
                        Console.WriteLine(length + " Pictures?");
                        for (int i = 0; i < length; i++)
                        {
                            idBuilder = "";
                            for (int x = 0; x < 4; x++)
                            {
                                idBuilder += (char)r.ReadByte();
                            }
                            pictureIds.Add(idBuilder);
                        }
                        
                        for (int i = thumbnailIndex; i < pictureIds.Count; i++)
                        {
                            //Console.WriteLine(getIdAsHexString(id));
                            if (!File.Exists(Program.getThumbnailDir() + getIdAsHexString(pictureIds[i]) + ".jpg") && !imagesToGet.Contains(pictureIds[i]))
                            {
                                imagesToGet.Add(pictureIds[i]);
                                //break;
                            }
                        }


                        
                        //Program.addImages(pictureIds);
                        Console.WriteLine(pictureIds.Count);


                        transactionIdCodes.Add(transactionId + 1, NikonOperationCodes.RequestThumb);
                        Program.addPacketToQueue(new OperationPacket()
                        {
                            hostName = "",
                            packetType = 6,
                            operationCode = (UInt16)NikonOperationCodes.RequestThumb,
                            thumbId = imagesToGet[0]
                        }.getData());


                        operationOveride = false;
                        transactionId++;
                        //((BackgroundWorker)sender).ReportProgress(0);//0 is add thumbs
                        break;
                    case NikonResponseCodes.DownloadData:
                        if(dataLength == 0)
                        {
                            dataLength = r.ReadInt32();
                        }
                        if (dataCode == 0)
                        {
                            dataCode = r.ReadInt32();
                        }
                        if(dataType == 0)
                        {
                            dataType = r.ReadInt32();
                        }




                        for (int i = 0; i < length ; i++)
                        {
                            data.Add(r.ReadByte());
                        }
                        tempIndex = 0;
                        endData[0] = 0;
                        for (int i = 0; i < endData.Length; i++)
                        {
                            endData[i] = b[((length - 3) - 4) - (7 - i)];
                        }

                        


                        if (endData.SequenceEqual(okOpPacket))
                        {
                            Console.WriteLine("Found End Of Packet");

                            if (data[0] != 255)
                            {
                                for (int i = 0; i < 32; i++)
                                {
                                    data.RemoveAt(0);
                                }
                            }

                            Program.saveThumbnail(data, getIdAsHexString(imagesToGet[0]));

                            
                            dataCode = 0;
                            dataType = 0;
                            dataLength = 0;
                            data.Clear();
                            imagesToGet.RemoveAt(0);
                            transactionId++;
                            isProcessing = false;
                            if (imagesToGet.Count > 0)
                            {
                                Thread.Sleep(100);
                                Console.WriteLine("Downloading Next Image");
                                transactionIdCodes.Add(transactionId, NikonOperationCodes.RequestThumb);
                                //redo packet
                                Program.addPacketToQueue(new OperationPacket()
                                {
                                    hostName = "",
                                    packetType = 6,
                                    operationCode = (UInt16)NikonOperationCodes.RequestThumb,
                                    thumbId = imagesToGet[0]
                                }.getData());
                                operationOveride = false;
                                break;
                            }
                            else
                            {
                                Program.enableThumbChecker();
                                operationOveride = false;
                                isProcessing = false;
                                transactionId++;
                            }
                        }
                        break;
                }
            }
            
            
        }


        public static int getConNum()
        {
            return connectionNum;
        }

        internal static int getTID()
        {
            return transactionId;
        }

        internal static List<string> getImageIds()
        {
            return pictureIds;
        }

        static string tempHexString = "";
        public static string getIdAsHexString(string id)
        {
            tempHexString = "";

            foreach (char c in id)
            {
                tempHexString += ((byte)c).ToString("X2");
            }

            return tempHexString;

        }

        public static bool isBusy()
        {
            return isProcessing;
        }
    }
}
