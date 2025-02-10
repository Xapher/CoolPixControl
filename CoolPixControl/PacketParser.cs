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

        static readonly byte[] okOpPacket = new byte[] { 7, 0, 0, 0, 0x01, 0x20 }, tempPicEnd = new byte[] { 0xef, 0x1b, 0, 0, 0x0c, 0, 0, 0, 0x06, 0, 0, 0 };
        static byte[] endData = new byte[6], tempEnd = new byte[12];
        static int tempIndex = 0;
        static int thumbnailIndex = 2;


        static bool isProcessing = false;
        static short requestResponse = 0;
        static bool pictureDownload = false;
        static Dictionary<int, NikonOperationCodes> transactionIdCodes = new Dictionary<int, NikonOperationCodes>();
        static int photoIteration = 1;

        static int readImageBytre = 0;
        static int imageLength = 0;
        static byte[] testData = new byte[4];
        static List<byte> pixFixer = new List<byte>();
        static FileStream fs = new FileStream(@"G:\Github\CoolPixControl\CoolPixControl\bin\Debug\net8.0-windows\Pictures\testFile", FileMode.OpenOrCreate);
        static List<byte> tempData = new List<byte>();
        public static void parsePacket(byte[] b, int length, NikonOperationCodes opcodes)//sender is used for reporting progress
        {
            isProcessing = true;
            //check for any previous packet ending
            ms = new MemoryStream(b);
            r = new BinaryReader(ms);


            Console.WriteLine(transactionIdCodes.Count);
            if(!operationOveride && OverideType != NikonResponseCodes.DownloadPhoto)
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
                                        packetType = (int)TCPPacketType.RequestOperation,
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
                    if(transactionIdCodes.Count > 0)
                    {
                        if (transactionIdCodes[transactionId] == NikonOperationCodes.RequestThumb || transactionIdCodes[transactionId] == NikonOperationCodes.RequestFullPicture)
                        {
                            Console.WriteLine("Picture");
                            Console.WriteLine(transactionIdCodes[transactionId]);
                            Console.WriteLine("Definate Pic");
                            pictureDownload = true;//janky
                        }
                    }

                    if (pictureDownload)
                    {
                        Console.WriteLine("Found Beginning of picture");
                        operationOveride = true;

                        switch(transactionIdCodes[transactionId])
                        {
                            case NikonOperationCodes.RequestThumb:
                                OverideType = NikonResponseCodes.DownloadData;
                                break;
                            case NikonOperationCodes.RequestFullPicture:
                                OverideType = NikonResponseCodes.DownloadPhoto;
                                break;
                        }

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
                            packetType = (int)TCPPacketType.RequestOperation,
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
                            endData[i] = b[((length - 3) - 6) - ((endData.Length - 1) - i)];
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
                            isProcessing = false;

                            transactionIdCodes.Remove(transactionId);


                            
                            if (imagesToGet.Count > 0)
                            {
                                transactionId++;
                                Thread.Sleep(100);
                                Console.WriteLine("Downloading Next Image");
                                transactionIdCodes.Add(transactionId, NikonOperationCodes.RequestThumb);
                                
                                //redo packet
                                Program.addPacketToQueue(new OperationPacket()
                                {
                                    hostName = "",
                                    packetType = (int)TCPPacketType.RequestOperation,
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
                    case NikonResponseCodes.DownloadPhoto:
                        if(imageLength == 0)
                        {
                            //fs = new FileStream(Program.getPhotoPath(), FileMode.OpenOrCreate);
                            imageLength = r.ReadInt32();
                            r.ReadInt32();//Type
                            r.ReadInt32();//Transaction id'
                        }

                        for (int i = 0; i < length; i++)
                        {
                            tempData.Add(b[i]);
                        }
                        readImageBytre += length;

                        if(readImageBytre < imageLength)
                        { 
                            Program.addPacketToQueue(new OperationPacket()
                            {
                                hostName = "",
                                packetType = (int)TCPPacketType.RequestOperation,
                                operationCode = (UInt16)NikonOperationCodes.RequestFullPicture,
                                dataLegnth = 0x00f00000 * (photoIteration + 1)
                            }.getData());
                            photoIteration++;
                        }
                        else if(readImageBytre < 0x00f00000)
                        {

                        }
                        else
                        {
                            Console.WriteLine("End");
                            //fs.Close();

                            //pixFixer = new List<byte>(File.ReadAllBytes(Program.getPhotoPath()));

                            for (int i = 0; i < 12; i++)
                            {
                                tempData.RemoveAt(0);
                            }
                            imageLength = 0;
                            operationOveride = false;
                            readImageBytre = 0;
                            File.WriteAllBytes(Program.getPhotoPath(), tempData.ToArray());
                            //Program.savePhoto(data, Program.getSelectedThumbnail());
                            OverideType = NikonResponseCodes.DataBegin;
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


        static byte[] tempHex = new byte[4];
        public static byte[] getBytesFromFileID(string id)
        {
            for (int i = 0; i < tempHex.Length; i++)
            {
                tempHex[i] = Convert.ToByte(id.Substring(i * 2, 2), 16);
            }
            return tempHex;
        }

        internal static void addRequest(NikonOperationCodes type)
        {
            transactionIdCodes.Add(transactionId, type);
        }
    }
}

/*
 * 
 * if (dataLength == 0)
                        {
                            dataLength = r.ReadInt32();
                        }
                        if (dataCode == 0)
                        {
                            dataCode = r.ReadInt32();
                        }
                        if (dataType == 0)
                        {
                            dataType = r.ReadInt32();
                        }

                        Console.WriteLine(dataLength);
                        Console.WriteLine(dataCode);
                        Console.WriteLine(dataType);


                        for (int i = 0; i < length; i++)
                        {
                            data.Add(r.ReadByte());
                        }
                        tempIndex = 0;
                        endData[0] = 0;
                        try
                        {
                            for (int i = 0; i < endData.Length; i++)
                            {
                                endData[i] = b[((length - 3) - 6) - ((endData.Length - 1) - i)];
                            }
                        }
                        catch
                        {
                            for (int i = 0; i < tempPicEnd.Length; i++)
                            {
                                tempEnd[i] = b[i];
                            }

                            if(tempEnd.SequenceEqual(tempPicEnd))
                            {
                                Console.WriteLine("Possible end pic");
                            }

                            Console.WriteLine();
                            break;
                        }



                        foreach (byte b2 in endData)
                        {
                            Console.Write(b2.ToString("X2") + " ");
                        }
                        Console.WriteLine();


                        if (endData.SequenceEqual(okOpPacket))
                        {
                            Console.WriteLine("Found End Of Packet");
                            if(dataLength == 1048588)
                            {
                                Console.WriteLine("Didn't find end of picture");
                                for (int i = 0; i < 18; i++)
                                {
                                    //remove end "End packet"
                                }
                                Thread.Sleep(100);

                                Program.addPacketToQueue(new OperationPacket()
                                {
                                    hostName = "",
                                    packetType = (int)TCPPacketType.RequestOperation,
                                    operationCode = (UInt16)NikonOperationCodes.RequestFullPicture,
                                    dataLegnth = dataLength * photoIteration
                                }.getData());
                                photoIteration++;
                            }
                            else
                            {
                                if (data[0] != 255)
                                {
                                    for (int i = 0; i < 32; i++)
                                    {
                                        data.RemoveAt(0);
                                    }
                                }

                                Program.savePhoto(data, Program.getSelectedThumbnail());

                                photoIteration = 0;
                                dataCode = 0;
                                dataType = 0;
                                dataLength = 0;
                                data.Clear();
                                imagesToGet.RemoveAt(0);
                                isProcessing = false;

                                transactionIdCodes.Remove(transactionId);
                                transactionId++;

                                Program.enableThumbChecker();
                                operationOveride = false;
                                isProcessing = false;
                            }
                            
                        }
                        else
                        {
                            
                        }
 */