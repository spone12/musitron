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
        protected bool isPlayMusic = false;
        Timer songTimer;

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
            setMusicVolume(soundVolume.Value);
            MediaPlayer.controls.play();

            isPlayMusic = true;
            songTimerProgress();
        }

        public void stopMusic()
        {
            if (MediaPlayer != null)
            {
                try
                {
                    MediaPlayer.controls.stop();
                    isPlayMusic = false;
                    songTimerOutput.Text = "00:00";
                }
                catch (Exception) { }
            }
        }

        private void soundVolume_Scroll(object sender, System.EventArgs e)
        {
            setMusicVolume(soundVolume.Value);
        }

        public void setMusicVolume(int volume)
        {
            if (MediaPlayer != null)
            {
                MediaPlayer.settings.volume = volume * 10;
            }
        }

        public void songTimerProgress()
        {
            songTimer = new Timer();
            songTimer.Interval = 1000;
            songTimer.Start();

            double percent = 0;

            songTimer.Tick += (o, e) =>
            {
                percent = ((double)MediaPlayer.controls.currentPosition / MediaPlayer.controls.currentItem.duration);
                songProgressBar.Value = (int)(percent * songProgressBar.Maximum);

                songTimerOutput.Text = MediaPlayer.controls.currentPositionString;
            };
        }

        private void InitializeComponent()
        {
            this.openFolderButton = new System.Windows.Forms.Button();
            this.startPlayButton = new System.Windows.Forms.Button();
            this.musicBox = new System.Windows.Forms.ListBox();
            this.stopMusicButton = new System.Windows.Forms.Button();
            this.soundVolume = new System.Windows.Forms.TrackBar();
            this.songTimerOutput = new System.Windows.Forms.TextBox();
            this.songProgressBar = new System.Windows.Forms.ProgressBar();
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
            this.startPlayButton.Location = new System.Drawing.Point(213, 31);
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
            this.musicBox.Size = new System.Drawing.Size(384, 160);
            this.musicBox.TabIndex = 2;
            // 
            // stopMusicButton
            // 
            this.stopMusicButton.Location = new System.Drawing.Point(346, 31);
            this.stopMusicButton.Name = "stopMusicButton";
            this.stopMusicButton.Size = new System.Drawing.Size(75, 23);
            this.stopMusicButton.TabIndex = 3;
            this.stopMusicButton.Text = "Стоп";
            this.stopMusicButton.UseVisualStyleBackColor = true;
            this.stopMusicButton.Click += new System.EventHandler(this.stopMusicButton_Click);
            // 
            // soundVolume
            // 
            this.soundVolume.Location = new System.Drawing.Point(278, 241);
            this.soundVolume.Name = "soundVolume";
            this.soundVolume.Size = new System.Drawing.Size(143, 50);
            this.soundVolume.TabIndex = 4;
            this.soundVolume.Value = 10;
            this.soundVolume.Scroll += new System.EventHandler(this.soundVolume_Scroll);
            // 
            // songTimerOutput
            // 
            this.songTimerOutput.Enabled = false;
            this.songTimerOutput.Location = new System.Drawing.Point(37, 250);
            this.songTimerOutput.Name = "songTimerOutput";
            this.songTimerOutput.Size = new System.Drawing.Size(54, 20);
            this.songTimerOutput.TabIndex = 5;
            this.songTimerOutput.Text = "00:00";
            this.songTimerOutput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // songProgressBar
            // 
            this.songProgressBar.Location = new System.Drawing.Point(97, 241);
            this.songProgressBar.Name = "songProgressBar";
            this.songProgressBar.Size = new System.Drawing.Size(175, 38);
            this.songProgressBar.TabIndex = 7;
            // 
            // Musitron
            // 
            this.ClientSize = new System.Drawing.Size(457, 301);
            this.Controls.Add(this.songProgressBar);
            this.Controls.Add(this.songTimerOutput);
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
