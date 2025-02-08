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
            worker.WorkerSupportsCancellation = true;

            FormClosed += Form_FormClosed;


            worker.RunWorkerAsync();
            InitializeComponent();
        }

        bool picCouldChange = false;
        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if(e.ProgressPercentage == 0 && picCouldChange) // picture list could have changed. Needs to update Thumbnails
            {
                ThumnailPanel.Controls.Clear();
                Program.addImages();
                picCouldChange = false;
            }
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            while (Program.isRunning)
            {
                Thread.Sleep(15000);
                ((BackgroundWorker)sender).ReportProgress(0);
            }
        }

        string tempName = "";
        float widthRatio = 0.4f;
        internal void addThumbnail(string id)
        {
            if (File.Exists(Program.getThumbnailDir() + PacketParser.getIdAsHexString(id) + ".jpg"))
            {
                try
                {
                    //Console.WriteLine("Trying : " + PacketParser.getIdAsHexString(id));
                    PictureBox p = new PictureBox();
                    p.Image = Bitmap.FromFile(Program.getThumbnailDir() + PacketParser.getIdAsHexString(id) + ".jpg");
                    p.Width = (int)(ThumnailPanel.Width * widthRatio);
                    p.Height = p.Width;
                    p.Name = PacketParser.getIdAsHexString(id);
                    p.Click += (s, e) => {

                        foreach (Control item in ThumnailPanel.Controls)
                        {
                            ((PictureBox)item).BorderStyle = BorderStyle.None;
                        }

                        ((PictureBox)s).BorderStyle = BorderStyle.Fixed3D;
                    };
                    p.SizeMode = PictureBoxSizeMode.Zoom;
                    ThumnailPanel.Controls.Add(p);
                }
                catch {
                    Console.WriteLine("Something Went WRong");
                }
            }
            
               
        }
        private static void Form_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Program.Stop();

        }
        private void refreshPictures(object sender, EventArgs e)
        {
            picCouldChange = true;
            worker.CancelAsync();
            Program.addPacketToQueue(DefaultPackets.initPackets["GetPictureList"].getData());
        }

        internal void restartThumbThread()
        {
            worker.RunWorkerAsync();
        }
    }
}
