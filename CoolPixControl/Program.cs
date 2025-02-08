using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

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

        static readonly string defaultJson = "{\"ThumbDir\":\"./Thumbnails/\", \"SaveDir\":\"./Pictures/\"}";

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
                thumbnailDirectory = applicationDir + "Thumbnails\\";
                if (!Directory.Exists(thumbnailDirectory))
                {
                    Directory.CreateDirectory(thumbnailDirectory);
                }
            }
            if (optionJson["SaveDir"].ToString().StartsWith("."))
            {
                saveDirectory = applicationDir + "Picures\\";//Replace with NIKONL....
                if(!Directory.Exists(saveDirectory))
                {
                    Directory.CreateDirectory(saveDirectory);
                }
                
            }
        }

        public static bool isRunning = true;//Used to prevent background threads from continuing after application closed

        

        static byte[] buffer;
        static int readBytes = 0;

        static async void initializeTCP()
        {
            
            while(stream == null && isRunning)
            {
                //do nothing while stream isn't connected
            }
            buffer = new byte[streamClient.ReceiveBufferSize];
            while (isRunning)
            {
                try
                {
                    readBytes = await stream.ReadAsync(buffer, 0, (int)streamClient.ReceiveBufferSize);

                    for (int i = 0; i < readBytes; i++)
                    {
                        Console.Write(buffer[i].ToString("X2") + " ");
                    }
                    Console.WriteLine();

                    PacketParser.parsePacket(buffer, readBytes, NikonOperationCodes.RequestListOfPictures);
                    stream.Flush();
                    Thread.Sleep(20);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
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
            while(isRunning)
            {
                streamClient = new TcpClient();//Connect to the camera
                if (streamClient.ConnectAsync("192.168.1.1", 15740).Wait(100))
                {
                    Console.WriteLine("Connected");
                    break;
                }
                else
                {
                    Console.WriteLine("Not conneted Trying again");
                    Console.WriteLine(isRunning);
                }
                
            }
            if (streamClient.Connected)
            {
                stream = streamClient.GetStream();
                Console.WriteLine("Connected To Client");

                await stream.WriteAsync(DefaultPackets.initPackets["SendGGUID"].getData());//Send and request GUID
                stream.Flush();
                Thread.Sleep(100);

                Console.WriteLine("Sent GUID");
                while (true)// While Connection number is unknown
                {
                    Console.WriteLine(PacketParser.getConNum());

                    if (PacketParser.getConNum() != 0)
                    {
                        break;
                    }
                    //Do nothing
                }

                Thread.Sleep(100);

                connectionClient = new TcpClient("192.168.1.1", 15740);//Connect to the second stream? WHat does this do on the camera side
                conStream = connectionClient.GetStream();

                Console.WriteLine("Got Connection Number: " + PacketParser.getConNum() + " - Sending connection Request");

                await conStream.WriteAsync(DefaultPackets.initPackets["Request Connection"].getData());//Request connection on second stream. What even
                conStream.Flush();
                Thread.Sleep(100);


                await stream.WriteAsync(DefaultPackets.initPackets["GetDeviceInfo"].getData());//Gets device info in unicode
                stream.Flush();
                Thread.Sleep(100);


                await stream.WriteAsync(DefaultPackets.initPackets["OpenSession"].getData());//Starts session on camera
                Thread.Sleep(100);
                Console.Clear();
                Console.WriteLine("Session Opened, Camera should be on.");
                Console.WriteLine(optionFileLoc);
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
                catch(Exception e2)
                {
                    Console.WriteLine(e2.Message);
                }
                
            }

        }



        public static void addPacketToQueue(byte[] b)
        {
            packetQueue.Add(b);
        }

        internal static void addImages()
        {
            Console.WriteLine("Adding Images");
            foreach (string id in PacketParser.getImageIds())
            {
                form.addThumbnail(id);
            }

        }

        internal static void Stop()
        {
            isRunning = false;
        }

        internal static void enableThumbChecker()
        {
            form.restartThumbThread(); 
        }

        internal static void saveThumbnail(List<byte> data, string name)
        {
            File.WriteAllBytes(thumbnailDirectory + name + ".jpg", data.ToArray());
        }

        internal static string getThumbnailDir()
        {
            return thumbnailDirectory;
        }
    }
}