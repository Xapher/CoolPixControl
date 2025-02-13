using Microsoft.VisualBasic.Logging;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace CoolPixControl
{
    internal static class Program
    {

        static Thread ReadThread = new Thread(new ThreadStart(initializeTCP)), initThread = new Thread(new ThreadStart(InitCameraConnection));

        static BackgroundWorker packetWriter = new BackgroundWorker();

        static TcpClient streamClient, connectionClient;// What is ConnectionClient used for? none of the packets were written or read from it after initial 
        static NetworkStream stream, conStream;
        static Form1 form;
        static readonly string applicationDir = Application.ExecutablePath.Replace(Path.GetFileName(Application.ExecutablePath), "");
        static string optionFileLoc = Application.ExecutablePath.Replace(Path.GetFileName(Application.ExecutablePath), "CoolPixOptions.json");
        


        static JObject optionJson;

        static readonly string defaultJson = "{\"ThumbDir\":\"./Thumbnails/\", \"SaveDir\":\"./Pictures/\", \"Logging\":0}";
        public static readonly int dataSize = 0x00f00000;//15?mb
        static string thumbnailDirectory = "", saveDirectory = "";

        [STAThread]
        static void Main()
        {
            if (!File.Exists(optionFileLoc))
            {
                File.WriteAllText(optionFileLoc, defaultJson);
                optionJson = JObject.Parse(defaultJson);
            }
            else
            {
                optionJson = JObject.Parse(File.ReadAllText(optionFileLoc));
            }


            initThread.Start();
            ReadThread.Start();
            packetWriter.DoWork += PacketWriter_DoWork;
            packetWriter.RunWorkerAsync();

            checkOptionsFile();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(form = new Form1());

        }



        private static void checkOptionsFile()
        {
            if (optionJson["ThumbDir"].ToString().StartsWith("."))
            {
                thumbnailDirectory = applicationDir + optionJson["ThumbDir"].ToString().Replace("./", "").Replace("/", "") + "\\";
                if (!Directory.Exists(thumbnailDirectory))
                {
                    Directory.CreateDirectory(thumbnailDirectory);
                }
            }
            else
            {
                thumbnailDirectory = optionJson["ThumbDir"].ToString();
            }
            if (optionJson["SaveDir"].ToString().StartsWith("."))
            {
                saveDirectory = applicationDir + optionJson["SaveDir"].ToString().Replace("./", "").Replace("/", "") + "\\";//Replace with NIKONL....
                if (!Directory.Exists(saveDirectory))
                {
                    Directory.CreateDirectory(saveDirectory);
                }

            }
            else
            {
                saveDirectory = optionJson["SaveDir"].ToString();
            }
        }

        public static bool isRunning = true;//Used to prevent background threads from continuing after application closed



        static byte[] buffer;
        static int readBytes = 0;

        static async void initializeTCP()
        {

            while (stream == null && isRunning)
            {
                //do nothing while stream isn't connected
            }
            buffer = new byte[streamClient.ReceiveBufferSize];
            while (isRunning)
            {
                try
                {
                    readBytes = await stream.ReadAsync(buffer, 0, (int)streamClient.ReceiveBufferSize);
                    if (loggingEnabled())
                    {
                        Logger.log("New Read packet:\n");
                        for (int i = 0; i < readBytes; i++)
                        {
                            Logger.appendLogToLine(buffer[i].ToString("X2") + " ");
                        }
                    }
                    PacketParser.parsePacket(buffer, readBytes, NikonOperationCodes.RequestListOfPictures);
                    stream.Flush();
                    Thread.Sleep(20);
                }
                catch (Exception e)
                {
                    if (loggingEnabled())
                    {
                        Logger.log(e.Message);
                        Logger.log(e.StackTrace.ToString());
                    }
                }
            }
            if (stream != null)
            {
                stream.Flush();
                stream.Close();//Close stream when done
            }
        }


        static async void InitCameraConnection()
        {
            streamClient = new TcpClient();//Connect to the camera
            while (isRunning)
            {
                streamClient = new TcpClient();//Connect to the camera
                if (streamClient.ConnectAsync("192.168.1.1", 15740).Wait(100))
                {
                    break;
                }
                else
                {
                    Thread.Sleep(150);
                }

            }

            if (streamClient.Connected)
            {
                stream = streamClient.GetStream();
                Logger.log("Connected To Client");
                PacketParser.addRequest(NikonOperationCodes.GetDeviceInfo);
                await stream.WriteAsync(DefaultPackets.initPackets["SendGGUID"].getData());//Send and request GUID
                stream.Flush();
                Thread.Sleep(100);

                Logger.log("Sent GUID");
                while (true && isRunning)// While Connection number is unknown
                {
                    if (PacketParser.getConNum() != 0)
                    {
                        break;
                    }
                    //Do nothing
                }

                Thread.Sleep(100);

                connectionClient = new TcpClient("192.168.1.1", 15740);//Connect to the second stream? WHat does this do on the camera side
                conStream = connectionClient.GetStream();
                Logger.log("Got Connection Number: " + PacketParser.getConNum() + " - Sending connection Request");

                await conStream.WriteAsync(DefaultPackets.initPackets["Request Connection"].getData());//Request connection on second stream. What even
                conStream.Flush();
                Thread.Sleep(100);


                await stream.WriteAsync(DefaultPackets.initPackets["GetDeviceInfo"].getData());//Gets device info in unicode
                stream.Flush();
                Thread.Sleep(100);

                await stream.WriteAsync(DefaultPackets.initPackets["OpenSession"].getData());//Starts session on camera
                Thread.Sleep(100);
                Logger.log("Session Opened, Camera should be on.");
                Logger.log(optionFileLoc);
                form.UpdateCameraName("Nikon Coolpix L840");//Means it's connected and session has started. Hard coded to L840 for now due to only camera i have
                //Request second connection. Why, idk that's just what it does        
            }
        }



        static List<byte[]> packetQueue = new List<byte[]>();


        private static async void PacketWriter_DoWork(object? sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    if (packetQueue.Count > 0)
                    {
                        Thread.Sleep(100);
                        await stream.WriteAsync(packetQueue[0]);
                        packetQueue.RemoveAt(0);

                    }
                    else
                    {
                        Thread.Sleep(200);
                    }

                }
                catch (Exception e2)
                {
                    //Logger.log(e2.Message);
                }

            }

        }



        public static void addPacketToQueue(byte[] b, NikonOperationCodes type)
        {
            PacketParser.addRequest(type);
            packetQueue.Add(b);
        }

        internal static void addImages()
        {
            foreach (string id in PacketParser.getImageIds())
            {
                form.addThumbnail(id);
            }

        }

        internal static void Stop()
        {
            addPacketToQueue(new OperationPacket() {
                hostName = "",
                packetType = (int)TCPPacketType.RequestOperation,
                operationCode = (UInt16)NikonOperationCodes.CloseSession
            }.getData(), NikonOperationCodes.JankyEnd);//try to end session on camera
            isRunning = false;

            if (loggingEnabled())
            {
                Logger.saveLog();
            }
        }

        internal static void enableThumbChecker()
        {
            form.restartThumbThread();
        }

        internal static void saveThumbnail(List<byte> data, string name)
        {
            Logger.log("Saving Thumbnail: " + name);
            File.WriteAllBytes(thumbnailDirectory + name + ".jpg", data.ToArray());
        }

        internal static string getThumbnailDir()
        {
            return thumbnailDirectory;
        }


        static string selectedThumb = "", nonHexName = "";

        internal static string getSelectedThumbnail()
        {
            return selectedThumb;
        }

        internal static void setSelectedThumbnail(string name, string nonHex)
        {
            Logger.log("Setting id to: " + name);
            nonHexName = nonHex;
            selectedThumb = name;
        }

        internal static void savePhoto(List<byte> data)
        {
            File.WriteAllBytes(saveDirectory + getPictureName(nonHexName) + ".jpg", data.ToArray());
        }



        static byte[] intConverter = new byte[2];
        static string getPictureName(string n)
        {
            intConverter[1] = (byte)n[1];
            intConverter[0] = (byte)n[0];
            //Hard Coded DSCN because i'm not 100% sure where it gets that from
            return "DSCN" + BitConverter.ToUInt16(intConverter);
        }








        internal static string getPhotoPath()
        {
            return saveDirectory;
        }

        internal static void enableDownloadButton()
        {
            form.enableButtons();
        }

        internal static string getLogPath()
        {
            return applicationDir + "Log-" + DateTime.Now.ToString().Replace("/", "-").Replace(":", "-") + ".txt";
        }

        internal static bool loggingEnabled()
        {
            return (int)optionJson["Logging"] == 1;
        }

        internal static void updateProgress(int v)
        {
            form.updateProgress(v);
        }

        internal static void updateThumbnailDirectory(string selectedPath)
        {
            thumbnailDirectory = selectedPath;
            writeAndSaveOptions();
        }

        internal static void updateSaveDir(string v)
        {
            saveDirectory = v;
            writeAndSaveOptions();
        }


        public static void writeAndSaveOptions()
        {
            optionJson["ThumbDir"] = thumbnailDirectory;
            optionJson["SaveDir"] = saveDirectory;
            File.WriteAllText(optionFileLoc, optionJson.ToString(Newtonsoft.Json.Formatting.None));
        }

        internal static void disableLogging()
        {
            optionJson["Logging"] = 0;
            writeAndSaveOptions();
        }

        internal static void enableLogging()
        {
            optionJson["Logging"] = 1;
            writeAndSaveOptions();
        }
    }
}