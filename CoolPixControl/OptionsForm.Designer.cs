namespace CoolPixControl
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            BrowseForSave = new Button();
            BrowseForThumb = new Button();
            PhotoDirPath = new TextBox();
            ThumbNailDirPath = new TextBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76.5625F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.4375F));
            tableLayoutPanel1.Controls.Add(BrowseForSave, 1, 2);
            tableLayoutPanel1.Controls.Add(BrowseForThumb, 1, 0);
            tableLayoutPanel1.Controls.Add(PhotoDirPath, 0, 2);
            tableLayoutPanel1.Controls.Add(ThumbNailDirPath, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 56.1904755F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 43.8095245F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 51F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 12F));
            tableLayoutPanel1.Size = new Size(784, 161);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // BrowseForSave
            // 
            BrowseForSave.Dock = DockStyle.Fill;
            BrowseForSave.Location = new Point(603, 100);
            BrowseForSave.Name = "BrowseForSave";
            BrowseForSave.Size = new Size(178, 45);
            BrowseForSave.TabIndex = 2;
            BrowseForSave.Text = "Browse Save Dir";
            BrowseForSave.UseVisualStyleBackColor = true;
            BrowseForSave.Click += BrowseForSave_Click;
            // 
            // BrowseForThumb
            // 
            BrowseForThumb.Dock = DockStyle.Fill;
            BrowseForThumb.Location = new Point(603, 3);
            BrowseForThumb.Name = "BrowseForThumb";
            BrowseForThumb.Size = new Size(178, 49);
            BrowseForThumb.TabIndex = 3;
            BrowseForThumb.Text = "Browse Thumb";
            BrowseForThumb.UseVisualStyleBackColor = true;
            BrowseForThumb.Click += BrowseForThumb_Click;
            // 
            // PhotoDirPath
            // 
            PhotoDirPath.Anchor = AnchorStyles.Left;
            PhotoDirPath.Location = new Point(3, 111);
            PhotoDirPath.Name = "PhotoDirPath";
            PhotoDirPath.Size = new Size(594, 23);
            PhotoDirPath.TabIndex = 4;
            // 
            // ThumbNailDirPath
            // 
            ThumbNailDirPath.Anchor = AnchorStyles.Left;
            ThumbNailDirPath.Location = new Point(3, 16);
            ThumbNailDirPath.Name = "ThumbNailDirPath";
            ThumbNailDirPath.Size = new Size(594, 23);
            ThumbNailDirPath.TabIndex = 5;
            // 
            // OptionsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 161);
            Controls.Add(tableLayoutPanel1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OptionsForm";
            Text = "OptionsForm";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button BrowseForSave;
        private Button BrowseForThumb;
        private TextBox PhotoDirPath;
        private TextBox ThumbNailDirPath;
    }
}