using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace CoolPixControl
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 

        static Thread TCPThread = new Thread(new ThreadStart(initializeTCP)), initThread = new Thread(new ThreadStart(InitCameraConnection));

        static BackgroundWorker packetWriter = new BackgroundWorker();

        static TcpClient streamClient, connectionClient;
        static NetworkStream stream, conStream;
        static Form1 form;


        [STAThread]
        static void Main()
        {

            initThread.Start();

            TCPThread.Start();

            packetWriter.DoWork += PacketWriter_DoWork;
            packetWriter.RunWorkerAsync();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run((form = new Form1()));

            form.FormClosed += Form_FormClosed;




            //send GGUID?




            //connectionClient = new TcpClient("192.168.1.1", 15740);






        }

        public static bool isRunning = true;

        private static void Form_FormClosed(object? sender, FormClosedEventArgs e)
        {
            isRunning = false;

        }

        static byte[] buffer = new byte[1024];
        static int readBytes = 0;

        static async void initializeTCP()
        {
            
            while(stream == null)
            {

            }

            while (isRunning)
            {
                try
                {
                    readBytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                    PacketParser.parsePacket(buffer, readBytes, NikonOperationCodes.RequestListOfPictures);

                    Thread.Sleep(10);

                    for (int i = 0; i < readBytes; i++)
                    {
                        //Console.Write(buffer[i].ToString("X2") + " ");
                    }
                    //Console.WriteLine();
                    //Console.WriteLine(Encoding.Unicode.GetString(buffer));
                }
                catch
                {

                }
            }
            stream.Flush();
            stream.Close();
            
        }


        static async void InitCameraConnection()
        {
            streamClient = new TcpClient("192.168.1.1", 15740);
            stream = streamClient.GetStream();
            Console.WriteLine("Connected To Client");

            await stream.WriteAsync(DefaultPackets.initPackets["SendGGUID"].getData());
            stream.Flush();
            Thread.Sleep(100);

            Console.WriteLine("GUID");
            while (true)// While Connection number is unknown
            {
                Console.WriteLine(PacketParser.getConNum());

                if(PacketParser.getConNum() != 0)
                {
                    break;
                }
                //Do nothing
            }

            Thread.Sleep(100);

            connectionClient = new TcpClient("192.168.1.1", 15740);
            conStream = connectionClient.GetStream();

            Console.WriteLine("Got Connection Number: " + PacketParser.getConNum() + " - Sending connection Request");

            await conStream.WriteAsync(DefaultPackets.initPackets["Request Connection"].getData());
            conStream.Flush();
            Thread.Sleep(100);


            await stream.WriteAsync(DefaultPackets.initPackets["GetDeviceInfo"].getData());
            stream.Flush();
            Thread.Sleep(100);


            await stream.WriteAsync(DefaultPackets.initPackets["OpenSession"].getData());
            Thread.Sleep(100);
            Console.Clear();
            Console.WriteLine("Session Opened, Camera should be on.");
            //Request second connection. Why, idk that's just what it does            
        }



        static List<byte[]> packetQueue = new List<byte[]>();


        private static async void PacketWriter_DoWork(object? sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if(packetQueue.Count > 0)
                {
                    await stream.WriteAsync(packetQueue[0]);
                    packetQueue.RemoveAt(0);
                    Thread.Sleep(100);
                }
                else
                {
                    Thread.Sleep(200);
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
    }
}