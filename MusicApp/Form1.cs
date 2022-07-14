using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Media;

namespace MusicApp
{
    public partial class Musitron : Form
    {
        protected string selectedFolderPath;
        //MessageBox.Show(selectedFolderPath);

        public Musitron()
        {
            InitializeComponent();
        }

        private void playMusicButton(object sender, EventArgs e)
        {
            if(musicBox.SelectedIndex != -1)
            {
               MessageBox.Show("ind=" + musicBox.SelectedIndex + " item=" + musicBox.SelectedItem.ToString());
            }
        }
            private void openFolder(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();

            if (FBD.ShowDialog() == DialogResult.OK)
            {
                if (FBD.SelectedPath != null)
                {
                    selectedFolderPath = FBD.SelectedPath;
                    musicBox.Items.Clear();

                    string[] files = Directory.GetFiles(FBD.SelectedPath); 
                    foreach (string file in files)
                    {
                        var extension = Path.GetExtension(file);
                        if (extension != null && extension.Contains("mp3")) 
                        { 
                            //MessageBox.Show(file);
                            string fileName = Path.GetFileNameWithoutExtension(file); 

                            ListViewItem item = new ListViewItem(fileName);
                            item.Tag = file;
                            musicBox.Items.Add(item); 

                        }
                    }
                }
                    

            }
        }

        private void ergergergerW(object sender, MouseEventArgs e)
        {
            var senderList = (ListView)sender;
            var clickedItem = senderList.HitTest(e.Location).Item;
            if (clickedItem != null)
            {
                MessageBox.Show("fgsdfsd");
            }
        }

        public void playMusic(string file)
        {
            WMPLib.WindowsMediaPlayer WMP = new WMPLib.WindowsMediaPlayer();
            WMP.URL = file;
            WMP.controls.play();
        }

        private void InitializeComponent()
        {
            this.openFolderButton = new System.Windows.Forms.Button();
            this.startPlayButton = new System.Windows.Forms.Button();
            this.musicBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // openFolderButton
            // 
            this.openFolderButton.Location = new System.Drawing.Point(71, 31);
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size(114, 23);
            this.openFolderButton.TabIndex = 0;
            this.openFolderButton.Text = "Открыть папку";
            this.openFolderButton.UseVisualStyleBackColor = true;
            this.openFolderButton.Click += new System.EventHandler(this.openFolder);
            // 
            // startPlayButton
            // 
            this.startPlayButton.Location = new System.Drawing.Point(283, 31);
            this.startPlayButton.Name = "startPlayButton";
            this.startPlayButton.Size = new System.Drawing.Size(75, 23);
            this.startPlayButton.TabIndex = 1;
            this.startPlayButton.Text = "Запуск";
            this.startPlayButton.UseVisualStyleBackColor = true;
            this.startPlayButton.Click += new System.EventHandler(this.playMusicButton);
            // 
            // musicBox
            // 
            this.musicBox.FormattingEnabled = true;
            this.musicBox.Location = new System.Drawing.Point(37, 75);
            this.musicBox.Name = "musicBox";
            this.musicBox.Size = new System.Drawing.Size(361, 160);
            this.musicBox.TabIndex = 2;
            // 
            // Musitron
            // 
            this.ClientSize = new System.Drawing.Size(418, 259);
            this.Controls.Add(this.musicBox);
            this.Controls.Add(this.startPlayButton);
            this.Controls.Add(this.openFolderButton);
            this.Name = "Musitron";
            this.ResumeLayout(false);

        }
    }
}
