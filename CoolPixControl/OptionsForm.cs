using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoolPixControl
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
            ThumbNailDirPath.Text = Program.getThumbnailDir();
            PhotoDirPath.Text = Program.getPhotoPath();
            Visible = true;
        }

        private void BrowseForThumb_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ThumbNailDirPath.Text = dialog.SelectedPath + "\\";
                    Program.updateThumbnailDirectory(dialog.SelectedPath + "\\");
                }
            }
        }

        private void BrowseForSave_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    PhotoDirPath.Text = dialog.SelectedPath + "\\";
                    Program.updateSaveDir(dialog.SelectedPath + "\\");
                }
            }
        }
    }
}
