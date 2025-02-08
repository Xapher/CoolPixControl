using System.ComponentModel;

namespace CoolPixControl
{
    public partial class Form1 : Form
    {
        BackgroundWorker worker = new BackgroundWorker();
        public Form1()
        {
            worker.DoWork += Worker_DoWork;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += Worker_ProgressChanged;
            //worker.RunWorkerAsync();
            InitializeComponent();
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if(e.ProgressPercentage == 0)
            {
                ThumnailPanel.Controls.Clear();
                Program.addImages();
            }
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            while(Program.isRunning)
            {
                Thread.Sleep(15000);
                //((BackgroundWorker)sender).ReportProgress(0);
            }
        }

        string tempName = "";

        internal void addThumbnail(string id)
        {
            if(File.Exists(@"C:\TempThumbs\" + PacketParser.getIdAsHexString(id)))
            {
                //Console.WriteLine(id);
                PictureBox p = new PictureBox();
                p.Name = PacketParser.getIdAsHexString(id);
                //p.Image = Bitmap.FromFile(@"G:\car\DSCN2161.JPG");
                p.SizeMode = PictureBoxSizeMode.Zoom;
                ThumnailPanel.Controls.Add(p);
            }
            
               
        }

        private void refreshPictures(object sender, EventArgs e)
        {
            Program.addPacketToQueue(DefaultPackets.initPackets["GetPictureList"].getData());
        }
    }
}
