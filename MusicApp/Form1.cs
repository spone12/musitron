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
        WMPLib.WindowsMediaPlayer MediaPlayer;

        public Musitron()
        {
            InitializeComponent();
        }

        private void playMusicButton(object sender, EventArgs e)
        {
            if(musicBox.SelectedIndex != -1)
            {
                string fullFileNameString = musicBox.SelectedItem.ToString();
                string fileName = fullFileNameString.Split('{').Last().Split('}').First() + ".mp3";

                playMusic(fileName);
            }
        }

        private void stopMusicButton_Click(object sender, EventArgs e)
        {
            stopMusic();
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
                            string fileName = Path.GetFileNameWithoutExtension(file); 

                            ListViewItem item = new ListViewItem(fileName);
                            item.Tag = file;
                            musicBox.Items.Add(item); 

                        }
                    }
                }
            }
        }

        public void playMusic(string file)
        {
            string filePath = this.selectedFolderPath + "\\" + file;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File " + filePath + " doesn't exist");
            }
            stopMusic();

            MediaPlayer = new WMPLib.WindowsMediaPlayer();
            MediaPlayer.URL = filePath;
            MediaPlayer.controls.play();

        }

        public void stopMusic()
        {
            if (MediaPlayer != null)
            {
                try
                {
                    MediaPlayer.controls.stop();
                }
                catch (Exception) { }
            }
        }

        private void soundVolume_Scroll(object sender, System.EventArgs e)
        {
            // Display the trackbar value in the text box.
            setMusicVolume(soundVolume.Value);
        }

        public void setMusicVolume(int volume)
        {
            if (MediaPlayer != null)
            {
                MediaPlayer.settings.volume = volume * 10;
            }
        }

        private void InitializeComponent()
        {
            this.openFolderButton = new System.Windows.Forms.Button();
            this.startPlayButton = new System.Windows.Forms.Button();
            this.musicBox = new System.Windows.Forms.ListBox();
            this.stopMusicButton = new System.Windows.Forms.Button();
            this.soundVolume = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.soundVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // openFolderButton
            // 
            this.openFolderButton.Location = new System.Drawing.Point(37, 31);
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size(114, 23);
            this.openFolderButton.TabIndex = 0;
            this.openFolderButton.Text = "Открыть папку";
            this.openFolderButton.UseVisualStyleBackColor = true;
            this.openFolderButton.Click += new System.EventHandler(this.openFolder);
            // 
            // startPlayButton
            // 
            this.startPlayButton.Location = new System.Drawing.Point(199, 31);
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
            // stopMusicButton
            // 
            this.stopMusicButton.Location = new System.Drawing.Point(323, 31);
            this.stopMusicButton.Name = "stopMusicButton";
            this.stopMusicButton.Size = new System.Drawing.Size(75, 23);
            this.stopMusicButton.TabIndex = 3;
            this.stopMusicButton.Text = "Стоп";
            this.stopMusicButton.UseVisualStyleBackColor = true;
            this.stopMusicButton.Click += new System.EventHandler(this.stopMusicButton_Click);
            // 
            // soundVolume
            // 
            this.soundVolume.Location = new System.Drawing.Point(77, 241);
            this.soundVolume.Name = "soundVolume";
            this.soundVolume.Size = new System.Drawing.Size(283, 50);
            this.soundVolume.TabIndex = 4;
            this.soundVolume.Value = 10;
            this.soundVolume.Scroll += new System.EventHandler(this.soundVolume_Scroll);
            // 
            // Musitron
            // 
            this.ClientSize = new System.Drawing.Size(418, 291);
            this.Controls.Add(this.soundVolume);
            this.Controls.Add(this.stopMusicButton);
            this.Controls.Add(this.musicBox);
            this.Controls.Add(this.startPlayButton);
            this.Controls.Add(this.openFolderButton);
            this.Name = "Musitron";
            ((System.ComponentModel.ISupportInitialize)(this.soundVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
