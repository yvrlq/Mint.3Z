﻿namespace Launcher
{
    partial class LMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LMain));
            this.DLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.StartGameButton = new DevExpress.XtraEditors.SimpleButton();
            this.TotalProgressBar = new DevExpress.XtraEditors.ProgressBarControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.StatusLabel = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.DownloadSizeLabel = new DevExpress.XtraEditors.LabelControl();
            this.DownloadSpeedLabel = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PatchNotesHyperlinkControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.RepairButton = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.TotalProgressBar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // DLookAndFeel
            // 
            this.DLookAndFeel.LookAndFeel.SkinName = "Blue";
            // 
            // StartGameButton
            // 
            this.StartGameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StartGameButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("StartGameButton.ImageOptions.Image")));
            this.StartGameButton.Location = new System.Drawing.Point(714, 381);
            this.StartGameButton.Name = "StartGameButton";
            this.StartGameButton.Size = new System.Drawing.Size(117, 50);
            this.StartGameButton.TabIndex = 0;
            this.StartGameButton.Text = "开始游戏";
            this.StartGameButton.Click += new System.EventHandler(this.StartGameButton_Click);
            // 
            // TotalProgressBar
            // 
            this.TotalProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalProgressBar.Location = new System.Drawing.Point(14, 411);
            this.TotalProgressBar.Name = "TotalProgressBar";
            // 
            // 
            // 
            this.TotalProgressBar.Properties.FlowAnimationEnabled = true;
            this.TotalProgressBar.Properties.ShowTitle = true;
            this.TotalProgressBar.Size = new System.Drawing.Size(693, 19);
            this.TotalProgressBar.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl1.Location = new System.Drawing.Point(33, 370);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(28, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "状态:";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StatusLabel.Location = new System.Drawing.Point(80, 370);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(42, 14);
            this.StatusLabel.TabIndex = 3;
            this.StatusLabel.Text = "<没有>";
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl3.Location = new System.Drawing.Point(14, 391);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(28, 14);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "下载:";
            // 
            // DownloadSizeLabel
            // 
            this.DownloadSizeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DownloadSizeLabel.Location = new System.Drawing.Point(80, 391);
            this.DownloadSizeLabel.Name = "DownloadSizeLabel";
            this.DownloadSizeLabel.Size = new System.Drawing.Size(42, 14);
            this.DownloadSizeLabel.TabIndex = 5;
            this.DownloadSizeLabel.Text = "<没有>";
            // 
            // DownloadSpeedLabel
            // 
            this.DownloadSpeedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DownloadSpeedLabel.Location = new System.Drawing.Point(642, 391);
            this.DownloadSpeedLabel.Name = "DownloadSpeedLabel";
            this.DownloadSpeedLabel.Size = new System.Drawing.Size(42, 14);
            this.DownloadSpeedLabel.TabIndex = 7;
            this.DownloadSpeedLabel.Text = "<没有>";
            // 
            // labelControl6
            // 
            this.labelControl6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl6.Location = new System.Drawing.Point(537, 391);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(52, 14);
            this.labelControl6.TabIndex = 6;
            this.labelControl6.Text = "下载速度:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::Launcher.Properties.Resources.PatchHeader;
            this.pictureBox1.Location = new System.Drawing.Point(14, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(817, 331);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // PatchNotesHyperlinkControl
            // 
            this.PatchNotesHyperlinkControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PatchNotesHyperlinkControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PatchNotesHyperlinkControl.Location = new System.Drawing.Point(602, 353);
            this.PatchNotesHyperlinkControl.Name = "PatchNotesHyperlinkControl";
            this.PatchNotesHyperlinkControl.Size = new System.Drawing.Size(72, 14);
            this.PatchNotesHyperlinkControl.TabIndex = 9;
            this.PatchNotesHyperlinkControl.Text = "<href=zuixin.txt>最新补丁说明</href>";
            this.PatchNotesHyperlinkControl.HyperlinkClick += new DevExpress.Utils.HyperlinkClickEventHandler(this.PatchNotesHyperlinkControl_HyperlinkClick);
            // 
            // RepairButton
            // 
            this.RepairButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RepairButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("RepairButton.ImageOptions.Image")));
            this.RepairButton.Location = new System.Drawing.Point(714, 350);
            this.RepairButton.Name = "RepairButton";
            this.RepairButton.Size = new System.Drawing.Size(117, 25);
            this.RepairButton.TabIndex = 15;
            this.RepairButton.Text = "客户端修正";
            this.RepairButton.Click += new System.EventHandler(this.RepairButton_Click);
            // 
            // LMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 444);
            this.Controls.Add(this.RepairButton);
            this.Controls.Add(this.PatchNotesHyperlinkControl);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.DownloadSpeedLabel);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.DownloadSizeLabel);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.TotalProgressBar);
            this.Controls.Add(this.StartGameButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "欢乐世界传奇3登录器QQ群124385013";
            this.Load += new System.EventHandler(this.LMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TotalProgressBar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.LookAndFeel.DefaultLookAndFeel DLookAndFeel;
        private DevExpress.XtraEditors.ProgressBarControl TotalProgressBar;
        private DevExpress.XtraEditors.SimpleButton StartGameButton;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl StatusLabel;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl DownloadSizeLabel;
        private DevExpress.XtraEditors.LabelControl DownloadSpeedLabel;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.HyperlinkLabelControl PatchNotesHyperlinkControl;
        private DevExpress.XtraEditors.SimpleButton RepairButton;
    }
}

