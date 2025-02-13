using System.ComponentModel;

namespace CoolPixControl
{
    public partial class Form1 : Form
    {
        readonly char check = (char)0x2713;

        public Form1()
        {
            FormClosed += Form_FormClosed;
            InitializeComponent();

            checkLogging();

        }

        void checkLogging()
        {
            if (Program.loggingEnabled())
            {
                enabledToolStripMenuItem.Text = "Enable " + check;
                disabledToolStripMenuItem.Text = "Disable";
            }
            else
            {
                enabledToolStripMenuItem.Text = "Enable ";
                disabledToolStripMenuItem.Text = "Disable" + check;
            }
        }

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
                    p.Name = id;
                    p.Click += (s, e) =>
                    {

                        foreach (Control item in ThumnailPanel.Controls)
                        {
                            ((PictureBox)item).BorderStyle = BorderStyle.None;
                        }
                        Program.setSelectedThumbnail(PacketParser.getIdAsHexString(((PictureBox)s).Name), ((PictureBox)s).Name);
                        ((PictureBox)s).BorderStyle = BorderStyle.Fixed3D;
                    };
                    p.SizeMode = PictureBoxSizeMode.Zoom;
                    ThumnailPanel.Controls.Add(p);
                }
                catch
                {
                    Console.WriteLine("Something Went Wrong");
                }
            }


        }
        private static void Form_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Program.Stop();

        }
        private void refreshPictures(object sender, EventArgs e)
        {
            refreshPictureButton.Enabled = false;
            DownloadFromThumbnail.Enabled = false;
            Program.addPacketToQueue(DefaultPackets.initPackets["GetPictureList"].getData(), NikonOperationCodes.RequestListOfPictures);
        }

        internal void restartThumbThread()
        {
            BeginInvoke(() =>
            {
                enableButtons();
                ThumnailPanel.Controls.Clear();
                Program.addImages();
            });
        }

        private void DownloadFromThumbnail_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            refreshPictureButton.Enabled = false;

            foreach (Control co in ThumnailPanel.Controls)
            {
                co.Enabled = false;
            }


            Program.addPacketToQueue(new OperationPacket()
            {
                hostName = "",
                packetType = (int)TCPPacketType.RequestOperation,
                operationCode = (UInt16)NikonOperationCodes.RequestFullPicture,
                dataLegnth = 0
            }.getData(), NikonOperationCodes.RequestFullPicture);
        }



        public void enableButtons()
        {
            BeginInvoke(() =>
            {
                DownloadFromThumbnail.Enabled = true;
                refreshPictureButton.Enabled = true;
                foreach (Control co in ThumnailPanel.Controls)
                {
                    co.Enabled = true;
                }
            });
        }

        internal void UpdateCameraName(string v)
        {
            BeginInvoke(() =>
            {
                CameraConnection.Text = v;
            });
        }


        public void updateProgress(int prog)
        {
            BeginInvoke(() =>
            {
                ActionProgress.Value = prog;
            });

        }

        private void directoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm form = new OptionsForm();
        }

        private void enabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.enableLogging();
            checkLogging();
        }

        private void disabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.disableLogging();
            checkLogging();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
