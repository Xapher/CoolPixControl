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
        static List<string> pictureIds = new List<string>();

        static string idBuilder = "";
        static List<byte> data = new List<byte>();

        static List<string> imagesToGet = new List<string>();

        static int dataLength = 0, dataType = 0, dataCode = 0;


        static bool isProcessing = false;
        static short requestResponse = 0;

        static int readImageBytre = 0;
        static int imageLength = 0;
        static byte[] testData = new byte[4];
        static List<byte> pixFixer = new List<byte>();
        //static FileStream fs = new FileStream(@"G:\Github\CoolPixControl\CoolPixControl\bin\Debug\net8.0-windows\Pictures\testFile", FileMode.OpenOrCreate);
        static int ReadData = 0;

        static int pictureCount = 0;

        static byte pictureTag = 0x29, videoTag = 0x59;
        
        static List<NikonOperationCodes> lastRequestType = new List<NikonOperationCodes>();


        //Send request. THEN, when request response is finished increase transactionID
        public static void parsePacket(byte[] b, int byteLength, NikonOperationCodes opcodes)//sender is used for reporting progress
        {
            isProcessing = true;
            //check for any previous packet ending
            ms = new MemoryStream(b);
            r = new BinaryReader(ms);

            

            if(Program.loggingEnabled())
            {
                Logger.log("Reading Packet for :" + lastRequestType[0] + "\n");
                for (int i = 0; i < byteLength; i++)
                {
                    Logger.appendLogToLine(b[i].ToString("X2") + " ");
                }
            }

            if(lastRequestType.Count == 0)
            {
                return;
            }

            switch (lastRequestType[0])
            {
                case NikonOperationCodes.JankyEnd:
                    lastRequestType.RemoveAt(0);
                    break;
                case NikonOperationCodes.RequestListOfPictures:
                    
                    if(byteLength < 30)
                    {
                        if(Program.loggingEnabled())
                            Logger.log("Found response packet, Skipping");
                    }
                    else
                    {
                        pictureIds.Clear();
                        imagesToGet.Clear();
                        //Skip start data packet
                        r.ReadInt32();//len
                        r.ReadInt32();//code
                        r.ReadInt32();//transaction id
                        length = r.ReadInt32();
                        if (Program.loggingEnabled())
                            Logger.log(length + " Pictures");
                        for (int i = 0; i < length; i++)
                        {
                            idBuilder = "";
                            for (int x = 0; x < 4; x++)
                            {
                                idBuilder += (char)r.ReadByte();//Reads 4 bytes, those 4 bytes (in hex) are the picture id
                            }
                            pictureIds.Add(idBuilder);//Adds it as Char to remain numeric for ToString() later for hex conversion
                        }


                        for (int i = 0; i < pictureIds.Count; i++)
                        {
                            if (!File.Exists(Program.getThumbnailDir() + getIdAsHexString(pictureIds[i]) + ".jpg") && !imagesToGet.Contains(pictureIds[i]) && (pictureIds[i][3] == pictureTag || pictureIds[i][3] == videoTag))
                            {
                                //Make sure the thumnail doesn't exist and it's already queued to download
                                imagesToGet.Add(pictureIds[i]);
                            }
                        }
                        Logger.log("Got past thumb Check");
                        lastRequestType.RemoveAt(0);
                        if(imagesToGet.Count > 0)
                        {
                            Program.addPacketToQueue(new OperationPacket()
                            {
                                hostName = "",
                                packetType = (int)TCPPacketType.RequestOperation,
                                operationCode = (UInt16)NikonOperationCodes.RequestThumb,
                                thumbId = imagesToGet[0]
                            }.getData(), NikonOperationCodes.RequestThumb);
                        }
                        else
                        {
                            Program.enableThumbChecker();
                        }
                        //Queue the first thumbnail to be downloaded
                        transactionId++;
                    }
                    break;
                case NikonOperationCodes.RequestThumb:
                    Logger.log("Adding thumb packet");
                    if (dataLength == 0)
                    {
                        dataLength = r.ReadInt32();
                        if(Program.loggingEnabled())
                            Logger.log("Downloading Thumb: " + getIdAsHexString(imagesToGet[0]));
                    }
                    if (dataCode == 0)
                    {
                        dataCode = r.ReadInt32();
                    }
                    if (dataType == 0)
                    {
                        dataType = r.ReadInt32();
                    }

                    if(dataCode != 12 || (imagesToGet[0][3] != pictureTag && imagesToGet[0][3] != videoTag))//Pictures just don't work when the id begins with 0x00?? why? idk, i don't even know if they are pictures
                    {
                        if (Program.loggingEnabled())
                            Logger.log("Found False Response Packet");
                        dataLength = 0;
                        dataCode = 0;
                        dataType = 0;
                        if (imagesToGet[0][3] != pictureTag && imagesToGet[0][3] != videoTag)//skip id tstarting with 0x00
                        {
                            imagesToGet.RemoveAt(0);
                            lastRequestType.RemoveAt(0);
                            if(imagesToGet.Count > 0)
                            {
                                
                                Program.addPacketToQueue(new OperationPacket()
                                {
                                    hostName = "",
                                    packetType = (int)TCPPacketType.RequestOperation,
                                    operationCode = (UInt16)NikonOperationCodes.RequestThumb,
                                    thumbId = imagesToGet[0]
                                }.getData(), NikonOperationCodes.RequestThumb);
                            }
                            else
                            {
                                Program.enableThumbChecker();
                                transactionId++;
                            }
                        }
                    }
                    else
                    {
                        switch (imagesToGet[0][3])
                        {
                            case (char)0x29:
                                if (Program.loggingEnabled())
                                    Logger.log("Downloading thumb for picture");
                                break;
                            case (char)0x59:
                                if (Program.loggingEnabled())
                                    Logger.log("Downloading preview for video");
                                break;
                        }


                        for (int i = (int)r.BaseStream.Position; i < byteLength; i++)
                        {
                            data.Add(b[i]);
                        }

                        if(data.Count < dataLength)
                        {
                            //Logger.log("Didn't get whole image");
                        }
                        else
                        {
                            if (Program.loggingEnabled())
                                Logger.log("Saving: " + getIdAsHexString(imagesToGet[0]));
                            Program.saveThumbnail(data, getIdAsHexString(imagesToGet[0]));
                            pictureCount = pictureIds.Count;
                            //-4 because i think it lista ALL files? 4 respond with invalid handles
                            dataCode = 0;
                            dataType = 0;
                            dataLength = 0;
                            data.Clear();
                            if(imagesToGet.Count > 0)
                            {
                                imagesToGet.RemoveAt(0);
                            }
                            Program.updateProgress((int)(100 * ((pictureCount - imagesToGet.Count) / (float)pictureCount)));
                            if (imagesToGet.Count > 0)
                            {
                                Thread.Sleep(100);
                                lastRequestType.RemoveAt(0);
                                Program.addPacketToQueue(new OperationPacket()
                                {
                                    hostName = "",
                                    packetType = (int)TCPPacketType.RequestOperation,
                                    operationCode = (UInt16)NikonOperationCodes.RequestThumb,
                                    thumbId = imagesToGet[0]
                                }.getData(), NikonOperationCodes.RequestThumb);
                                transactionId++;
                                break;
                            }
                            else
                            {
                                lastRequestType.RemoveAt(0);
                                Program.enableThumbChecker();
                                transactionId++;
                            }
                        }
                    }
                    break;
                case NikonOperationCodes.RequestFullPicture:
                    if (dataLength == 0)
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
                    if (dataCode != 12)//Pictures just don't work when the id begins with 0?? why? idk, i don't even know if they are pictures
                    {
                        if (Program.loggingEnabled())
                            Logger.log("Found False Response Packet");
                        dataLength = 0;
                        dataCode = 0;
                        dataType = 0;
                        imageLength = 0;
                    }
                    else
                    {
                        r.BaseStream.Position = 0;  
                        if (imageLength == 0)
                        {
                            imageLength = r.ReadInt32();
                            r.ReadInt32();//Type
                            r.ReadInt32();//Transaction id'
                        }
                        

                        for (int i = (int)r.BaseStream.Position; i < byteLength; i++)
                        {   if(ReadData < imageLength)
                            {
                                data.Add(b[i]);
                                ReadData++;
                                //Program.updateProgressBar(ReadData, imageLength);
                            }
                            if(ReadData >= imageLength)
                            {
                                break;
                            }
                        }

                        readImageBytre += byteLength;

                        Program.updateProgress((int)(100 * ((float)ReadData / imageLength)));

                        if (Program.loggingEnabled())
                            Logger.log("Current Read Length: " + ReadData);

                        if (ReadData >= imageLength)
                        {
                            if (Program.loggingEnabled())
                                Logger.log("Read full image length. Saving: " + Program.getSelectedThumbnail());
                            readImageBytre = 0;
                            Program.savePhoto(data);
                            data.Clear();
                            dataLength = 0;
                            dataCode = 0;
                            dataType = 0;
                            transactionId++;
                            Thread.Sleep(100);
                            imageLength = 0;
                            lastRequestType.RemoveAt(0);

                            Program.addPacketToQueue(DefaultPackets.initPackets["GetPictureList"].getData(), NikonOperationCodes.JankyEnd);
                            //// ^ This is super janky, but it resets the "uploading" screen
                            /// The normal "end session" code and sending an OK code in response do not work. Replicating the tcp stream exactly doesn't work either for some reason.

                            ReadData = 0;
                            Program.enableDownloadButton();
                        }
                        else if (readImageBytre >= Program.dataSize) 
                        {
                            
                        }
                    }







                    
                    break;
                default:
                    length = r.ReadInt32();
                    code = r.ReadInt32();
                    //check code
                    if (code == 2 && connectionNum == 0)
                    {
                        if (Program.loggingEnabled())
                            Logger.log("UID Response");
                        connectionNum = r.ReadInt32();
                    }
                    if (Program.loggingEnabled())
                    {
                        Logger.log("Length: " + length);
                        Logger.log("Packet Type: " + code);
                        Logger.log("Connection Number: " + connectionNum);
                    }
                        

                    if (code == 7)
                    {
                        if (Program.loggingEnabled())
                            Logger.log("Found Operation response");
                        requestResponse = r.ReadInt16();
                        if (Program.loggingEnabled())
                            Logger.log("Operation Response: " + requestResponse.ToString("X2"));
                        switch (requestResponse)
                        {
                            case 0:
                                break;
                            case (short)NikonResponseCodes.InvalidObjectHandle:
                                if (Program.loggingEnabled())
                                    Logger.log("Invalid Object Handle?");
                                if (imagesToGet.Count > 0)
                                {
                                    imagesToGet.RemoveAt(0);
                                    if (imagesToGet.Count > 0)
                                    {
                                        Program.addPacketToQueue(new OperationPacket()
                                        {
                                            hostName = "",
                                            packetType = (int)TCPPacketType.RequestOperation,
                                            operationCode = (UInt16)NikonOperationCodes.RequestThumb,
                                            thumbId = imagesToGet[0]
                                        }.getData(), NikonOperationCodes.RequestThumb);
                                    }
                                }
                                break;
                        }
                        transactionId++;
                    }
                    else if (code == 9)
                    {
                        if (Program.loggingEnabled())
                            Logger.log("Found Request Response");
                        r.ReadInt32();///Transid
                        requestResponse = r.ReadInt16();
                        Logger.log("Respopnse: " + requestResponse.ToString("X4"));
                    }
                    isProcessing = false;
                    lastRequestType.RemoveAt(0);
                    break;
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
            lastRequestType.Add(type);
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

                        Logger.log(dataLength);
                        Logger.log(dataCode);
                        Logger.log(dataType);


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
                                Logger.log("Possible end pic");
                            }

                            Logger.log();
                            break;
                        }



                        foreach (byte b2 in endData)
                        {
                            Console.Write(b2.ToString("X2") + " ");
                        }
                        Logger.log();


                        if (endData.SequenceEqual(okOpPacket))
                        {
                            Logger.log("Found End Of Packet");
                            if(dataLength == 1048588)
                            {
                                Logger.log("Didn't find end of picture");
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