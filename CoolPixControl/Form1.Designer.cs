﻿namespace CoolPixControl
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            quitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            directoriesToolStripMenuItem = new ToolStripMenuItem();
            debuggingToolStripMenuItem = new ToolStripMenuItem();
            enabledToolStripMenuItem = new ToolStripMenuItem();
            disabledToolStripMenuItem = new ToolStripMenuItem();
            splitContainer2 = new SplitContainer();
            refreshPictureButton = new Button();
            splitContainer3 = new SplitContainer();
            splitContainer4 = new SplitContainer();
            splitContainer5 = new SplitContainer();
            CameraConnection = new Label();
            splitContainer6 = new SplitContainer();
            ActionProgress = new ProgressBar();
            label2 = new Label();
            LivePreview = new PictureBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            ThumbnailDownloadSplitter = new SplitContainer();
            TTextThumbnailsSplitter = new SplitContainer();
            label1 = new Label();
            ThumnailPanel = new FlowLayoutPanel();
            DownloadFromThumbnail = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer4).BeginInit();
            splitContainer4.Panel1.SuspendLayout();
            splitContainer4.Panel2.SuspendLayout();
            splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer5).BeginInit();
            splitContainer5.Panel1.SuspendLayout();
            splitContainer5.Panel2.SuspendLayout();
            splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer6).BeginInit();
            splitContainer6.Panel1.SuspendLayout();
            splitContainer6.Panel2.SuspendLayout();
            splitContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LivePreview).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ThumbnailDownloadSplitter).BeginInit();
            ThumbnailDownloadSplitter.Panel1.SuspendLayout();
            ThumbnailDownloadSplitter.Panel2.SuspendLayout();
            ThumbnailDownloadSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TTextThumbnailsSplitter).BeginInit();
            TTextThumbnailsSplitter.Panel1.SuspendLayout();
            TTextThumbnailsSplitter.Panel2.SuspendLayout();
            TTextThumbnailsSplitter.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(menuStrip1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(800, 450);
            splitContainer1.SplitterDistance = 25;
            splitContainer1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { quitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // quitToolStripMenuItem
            // 
            quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            quitToolStripMenuItem.Size = new Size(97, 22);
            quitToolStripMenuItem.Text = "Quit";
            quitToolStripMenuItem.Click += quitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { directoriesToolStripMenuItem, debuggingToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // directoriesToolStripMenuItem
            // 
            directoriesToolStripMenuItem.Name = "directoriesToolStripMenuItem";
            directoriesToolStripMenuItem.Size = new Size(130, 22);
            directoriesToolStripMenuItem.Text = "Directories";
            directoriesToolStripMenuItem.Click += directoriesToolStripMenuItem_Click;
            // 
            // debuggingToolStripMenuItem
            // 
            debuggingToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { enabledToolStripMenuItem, disabledToolStripMenuItem });
            debuggingToolStripMenuItem.Name = "debuggingToolStripMenuItem";
            debuggingToolStripMenuItem.Size = new Size(130, 22);
            debuggingToolStripMenuItem.Text = "Logging";
            // 
            // enabledToolStripMenuItem
            // 
            enabledToolStripMenuItem.Name = "enabledToolStripMenuItem";
            enabledToolStripMenuItem.Size = new Size(119, 22);
            enabledToolStripMenuItem.Text = "Enabled";
            enabledToolStripMenuItem.Click += enabledToolStripMenuItem_Click;
            // 
            // disabledToolStripMenuItem
            // 
            disabledToolStripMenuItem.Name = "disabledToolStripMenuItem";
            disabledToolStripMenuItem.Size = new Size(119, 22);
            disabledToolStripMenuItem.Text = "Disabled";
            disabledToolStripMenuItem.Click += disabledToolStripMenuItem_Click;
            // 
            // splitContainer2
            // 
            splitContainer2.BorderStyle = BorderStyle.Fixed3D;
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.IsSplitterFixed = true;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(refreshPictureButton);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(splitContainer3);
            splitContainer2.Size = new Size(800, 421);
            splitContainer2.SplitterDistance = 79;
            splitContainer2.TabIndex = 0;
            // 
            // refreshPictureButton
            // 
            refreshPictureButton.Dock = DockStyle.Top;
            refreshPictureButton.Location = new Point(0, 0);
            refreshPictureButton.Name = "refreshPictureButton";
            refreshPictureButton.Size = new Size(75, 43);
            refreshPictureButton.TabIndex = 0;
            refreshPictureButton.Text = "Refresh Pictures";
            refreshPictureButton.UseVisualStyleBackColor = true;
            refreshPictureButton.Click += refreshPictures;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.IsSplitterFixed = true;
            splitContainer3.Location = new Point(0, 0);
            splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(splitContainer4);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(ThumbnailDownloadSplitter);
            splitContainer3.Size = new Size(713, 417);
            splitContainer3.SplitterDistance = 547;
            splitContainer3.TabIndex = 0;
            // 
            // splitContainer4
            // 
            splitContainer4.Dock = DockStyle.Fill;
            splitContainer4.FixedPanel = FixedPanel.Panel2;
            splitContainer4.IsSplitterFixed = true;
            splitContainer4.Location = new Point(0, 0);
            splitContainer4.Name = "splitContainer4";
            splitContainer4.Orientation = Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            splitContainer4.Panel1.Controls.Add(splitContainer5);
            // 
            // splitContainer4.Panel2
            // 
            splitContainer4.Panel2.Controls.Add(tableLayoutPanel1);
            splitContainer4.Size = new Size(547, 417);
            splitContainer4.SplitterDistance = 352;
            splitContainer4.TabIndex = 0;
            // 
            // splitContainer5
            // 
            splitContainer5.Dock = DockStyle.Fill;
            splitContainer5.IsSplitterFixed = true;
            splitContainer5.Location = new Point(0, 0);
            splitContainer5.Name = "splitContainer5";
            splitContainer5.Orientation = Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            splitContainer5.Panel1.Controls.Add(CameraConnection);
            // 
            // splitContainer5.Panel2
            // 
            splitContainer5.Panel2.Controls.Add(splitContainer6);
            splitContainer5.Size = new Size(547, 352);
            splitContainer5.SplitterDistance = 25;
            splitContainer5.TabIndex = 0;
            // 
            // CameraConnection
            // 
            CameraConnection.BorderStyle = BorderStyle.FixedSingle;
            CameraConnection.Dock = DockStyle.Fill;
            CameraConnection.Location = new Point(0, 0);
            CameraConnection.Name = "CameraConnection";
            CameraConnection.Size = new Size(547, 25);
            CameraConnection.TabIndex = 0;
            CameraConnection.Text = "Connecting...";
            CameraConnection.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // splitContainer6
            // 
            splitContainer6.Dock = DockStyle.Fill;
            splitContainer6.FixedPanel = FixedPanel.Panel1;
            splitContainer6.IsSplitterFixed = true;
            splitContainer6.Location = new Point(0, 0);
            splitContainer6.Name = "splitContainer6";
            splitContainer6.Orientation = Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            splitContainer6.Panel1.Controls.Add(ActionProgress);
            splitContainer6.Panel1MinSize = 5;
            // 
            // splitContainer6.Panel2
            // 
            splitContainer6.Panel2.Controls.Add(label2);
            splitContainer6.Panel2.Controls.Add(LivePreview);
            splitContainer6.Size = new Size(547, 323);
            splitContainer6.SplitterDistance = 25;
            splitContainer6.TabIndex = 0;
            // 
            // ActionProgress
            // 
            ActionProgress.Dock = DockStyle.Fill;
            ActionProgress.Location = new Point(0, 0);
            ActionProgress.Name = "ActionProgress";
            ActionProgress.Size = new Size(547, 25);
            ActionProgress.TabIndex = 0;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 20F);
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(547, 294);
            label2.TabIndex = 1;
            label2.Text = "Live Preview Will Be Here";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LivePreview
            // 
            LivePreview.Dock = DockStyle.Fill;
            LivePreview.Location = new Point(0, 0);
            LivePreview.Name = "LivePreview";
            LivePreview.Size = new Size(547, 294);
            LivePreview.TabIndex = 0;
            LivePreview.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 95.25223F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.7477746F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 104F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 18F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 111F));
            tableLayoutPanel1.Controls.Add(button3, 0, 0);
            tableLayoutPanel1.Controls.Add(button4, 2, 0);
            tableLayoutPanel1.Controls.Add(button5, 4, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(547, 61);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // button3
            // 
            button3.Dock = DockStyle.Fill;
            button3.Enabled = false;
            button3.Location = new Point(3, 3);
            button3.Name = "button3";
            button3.Size = new Size(293, 55);
            button3.TabIndex = 0;
            button3.Text = "Capture";
            button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Dock = DockStyle.Fill;
            button4.Enabled = false;
            button4.Location = new Point(316, 3);
            button4.Name = "button4";
            button4.Size = new Size(98, 55);
            button4.TabIndex = 1;
            button4.Text = "Zoom -";
            button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Dock = DockStyle.Fill;
            button5.Enabled = false;
            button5.Location = new Point(438, 3);
            button5.Name = "button5";
            button5.Size = new Size(106, 55);
            button5.TabIndex = 2;
            button5.Text = "Zoom +";
            button5.UseVisualStyleBackColor = true;
            // 
            // ThumbnailDownloadSplitter
            // 
            ThumbnailDownloadSplitter.Dock = DockStyle.Fill;
            ThumbnailDownloadSplitter.Location = new Point(0, 0);
            ThumbnailDownloadSplitter.Name = "ThumbnailDownloadSplitter";
            ThumbnailDownloadSplitter.Orientation = Orientation.Horizontal;
            // 
            // ThumbnailDownloadSplitter.Panel1
            // 
            ThumbnailDownloadSplitter.Panel1.Controls.Add(TTextThumbnailsSplitter);
            // 
            // ThumbnailDownloadSplitter.Panel2
            // 
            ThumbnailDownloadSplitter.Panel2.Controls.Add(DownloadFromThumbnail);
            ThumbnailDownloadSplitter.Size = new Size(162, 417);
            ThumbnailDownloadSplitter.SplitterDistance = 354;
            ThumbnailDownloadSplitter.TabIndex = 0;
            // 
            // TTextThumbnailsSplitter
            // 
            TTextThumbnailsSplitter.Dock = DockStyle.Fill;
            TTextThumbnailsSplitter.IsSplitterFixed = true;
            TTextThumbnailsSplitter.Location = new Point(0, 0);
            TTextThumbnailsSplitter.Name = "TTextThumbnailsSplitter";
            TTextThumbnailsSplitter.Orientation = Orientation.Horizontal;
            // 
            // TTextThumbnailsSplitter.Panel1
            // 
            TTextThumbnailsSplitter.Panel1.Controls.Add(label1);
            // 
            // TTextThumbnailsSplitter.Panel2
            // 
            TTextThumbnailsSplitter.Panel2.Controls.Add(ThumnailPanel);
            TTextThumbnailsSplitter.Size = new Size(162, 354);
            TTextThumbnailsSplitter.SplitterDistance = 28;
            TTextThumbnailsSplitter.TabIndex = 0;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(162, 28);
            label1.TabIndex = 0;
            label1.Text = "Pictures (On Camera)";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ThumnailPanel
            // 
            ThumnailPanel.AutoScroll = true;
            ThumnailPanel.BorderStyle = BorderStyle.Fixed3D;
            ThumnailPanel.Dock = DockStyle.Fill;
            ThumnailPanel.Location = new Point(0, 0);
            ThumnailPanel.Name = "ThumnailPanel";
            ThumnailPanel.Size = new Size(162, 322);
            ThumnailPanel.TabIndex = 0;
            // 
            // DownloadFromThumbnail
            // 
            DownloadFromThumbnail.Dock = DockStyle.Fill;
            DownloadFromThumbnail.Location = new Point(0, 0);
            DownloadFromThumbnail.Name = "DownloadFromThumbnail";
            DownloadFromThumbnail.Size = new Size(162, 59);
            DownloadFromThumbnail.TabIndex = 0;
            DownloadFromThumbnail.Text = "Download Image";
            DownloadFromThumbnail.UseVisualStyleBackColor = true;
            DownloadFromThumbnail.Click += DownloadFromThumbnail_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer1);
            Name = "Form1";
            Text = "Form1";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            splitContainer4.Panel1.ResumeLayout(false);
            splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer4).EndInit();
            splitContainer4.ResumeLayout(false);
            splitContainer5.Panel1.ResumeLayout(false);
            splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer5).EndInit();
            splitContainer5.ResumeLayout(false);
            splitContainer6.Panel1.ResumeLayout(false);
            splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer6).EndInit();
            splitContainer6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)LivePreview).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            ThumbnailDownloadSplitter.Panel1.ResumeLayout(false);
            ThumbnailDownloadSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ThumbnailDownloadSplitter).EndInit();
            ThumbnailDownloadSplitter.ResumeLayout(false);
            TTextThumbnailsSplitter.Panel1.ResumeLayout(false);
            TTextThumbnailsSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)TTextThumbnailsSplitter).EndInit();
            TTextThumbnailsSplitter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private SplitContainer splitContainer3;
        private SplitContainer splitContainer4;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private Button refreshPictureButton;
        private SplitContainer ThumbnailDownloadSplitter;
        private Button DownloadFromThumbnail;
        private TableLayoutPanel tableLayoutPanel1;
        private Button button3;
        private Button button4;
        private Button button5;
        private SplitContainer TTextThumbnailsSplitter;
        private Label label1;
        private FlowLayoutPanel ThumnailPanel;
        private SplitContainer splitContainer5;
        private Label CameraConnection;
        private SplitContainer splitContainer6;
        private ProgressBar ActionProgress;
        private PictureBox LivePreview;
        private ToolStripMenuItem directoriesToolStripMenuItem;
        private ToolStripMenuItem quitToolStripMenuItem;
        private ToolStripMenuItem debuggingToolStripMenuItem;
        private ToolStripMenuItem enabledToolStripMenuItem;
        private ToolStripMenuItem disabledToolStripMenuItem;
        private Label label2;
    }
}
