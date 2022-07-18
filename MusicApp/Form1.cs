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
        protected Dictionary<int, string> importedSongsPaths = new Dictionary<int, string>();
        WMPLib.WindowsMediaPlayer MediaPlayer;
        protected bool isPlayMusic = false;
        Timer songTimer;
        protected string pathIcons = Environment.CurrentDirectory + "\\..\\..\\img\\";

        public Musitron()
        {
            InitializeComponent();
        }

        private void playMusicButton(object sender, EventArgs e)
        {
           
            if (musicBox.SelectedIndex != -1)
            {
                if (!isPlayMusic)
                {
                    if (MediaPlayer != null)
                    {
                        stopMusic();
                    }
                    else
                    {
                        playMusic();
                        startPlayButton.Image = Image.FromFile(pathIcons + "pause.png");
                    }
                }
                else
                {
                    stopMusic(true);
                }
            }
        }

        private string getMusicPath()
        {
       
            return importedSongsPaths[musicBox.SelectedIndex].ToString();
        }

        private void prevSongButton_Click(object sender, EventArgs e)
        {
            changeSong(false);
        }

        private void nextSongButton_Click(object sender, EventArgs e)
        {
            changeSong();   
        }

        private void changeSong(bool next = true)
        {
            if (musicBox.SelectedIndex != -1)
            {
                int index = musicBox.SelectedIndex + 1;

                if (!next)
                {
                    if(musicBox.SelectedIndex == 0)
                    {
                        index = musicBox.SelectedIndex;
                    }
                    else
                    {
                        index = musicBox.SelectedIndex - 1;
                    }
                }

                musicBox.SelectedIndex = Math.Min(index, musicBox.Items.Count - 1);
                playMusic();
            }
        }

        private void openFolder(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();

            if (FBD.ShowDialog() == DialogResult.OK)
            {
                if (FBD.SelectedPath != null)
                {
                    musicBox.Items.Clear();

                    string[] files = Directory.GetFiles(FBD.SelectedPath); 
                    foreach (string file in files)
                    {
                        var extension = Path.GetExtension(file);
                        if (extension != null && extension.Contains("mp3")) 
                        {
                           
                            string fileName = Path.GetFileNameWithoutExtension(file); 
                            musicBox.Items.Add(fileName);
                            importedSongsPaths.Add(musicBox.Items.Count - 1, FBD.SelectedPath + "\\" + fileName + extension);
                        }
                    }
                }
            }
        }

        public void playMusic()
        {
            string filePath = getMusicPath();

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File " + filePath + " doesn't exist");
            }

            if(MediaPlayer != null)
            {
                MediaPlayer.close();
                isPlayMusic = false;
            }
           
            MediaPlayer = new WMPLib.WindowsMediaPlayer();
            MediaPlayer.URL = filePath;
            setMusicVolume(soundVolume.Value);
            MediaPlayer.controls.play();

            isPlayMusic = true;
            songTimerProgress();
        }

        public void stopMusic(bool isStop = false)
        {
            if (MediaPlayer != null)
            {
                try
                {
                    if (isStop)
                    {
                        startPlayButton.Image = Image.FromFile(pathIcons + "play-button.png");
                        MediaPlayer.controls.pause();
                        isPlayMusic = false;
                    }
                    else
                    {
                        startPlayButton.Image = Image.FromFile(pathIcons + "pause.png");
                        MediaPlayer.controls.play();
                        isPlayMusic = true;
                    }
                   
                   
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Musitron));
            this.openFolderButton = new System.Windows.Forms.Button();
            this.musicBox = new System.Windows.Forms.ListBox();
            this.soundVolume = new System.Windows.Forms.TrackBar();
            this.songTimerOutput = new System.Windows.Forms.TextBox();
            this.songProgressBar = new System.Windows.Forms.ProgressBar();
            this.startPlayButton = new System.Windows.Forms.PictureBox();
            this.prevSongButton = new System.Windows.Forms.PictureBox();
            this.nextSongButton = new System.Windows.Forms.PictureBox();
            this.shuffleMusicButton = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.soundVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPlayButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.prevSongButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextSongButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shuffleMusicButton)).BeginInit();
            this.SuspendLayout();
            // 
            // openFolderButton
            // 
            this.openFolderButton.Location = new System.Drawing.Point(25, 31);
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size(114, 23);
            this.openFolderButton.TabIndex = 0;
            this.openFolderButton.Text = "Открыть папку";
            this.openFolderButton.UseVisualStyleBackColor = true;
            this.openFolderButton.Click += new System.EventHandler(this.openFolder);
            // 
            // musicBox
            // 
            this.musicBox.AllowDrop = true;
            this.musicBox.FormattingEnabled = true;
            this.musicBox.Location = new System.Drawing.Point(25, 60);
            this.musicBox.Name = "musicBox";
            this.musicBox.Size = new System.Drawing.Size(403, 160);
            this.musicBox.TabIndex = 2;
            this.musicBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.musicBox_DragDrop);
            this.musicBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.musicBox_DragEnter);
            // 
            // soundVolume
            // 
            this.soundVolume.Location = new System.Drawing.Point(345, 274);
            this.soundVolume.Name = "soundVolume";
            this.soundVolume.Size = new System.Drawing.Size(83, 50);
            this.soundVolume.TabIndex = 4;
            this.soundVolume.Value = 10;
            this.soundVolume.Scroll += new System.EventHandler(this.soundVolume_Scroll);
            // 
            // songTimerOutput
            // 
            this.songTimerOutput.Enabled = false;
            this.songTimerOutput.Location = new System.Drawing.Point(25, 283);
            this.songTimerOutput.Name = "songTimerOutput";
            this.songTimerOutput.Size = new System.Drawing.Size(54, 20);
            this.songTimerOutput.TabIndex = 5;
            this.songTimerOutput.Text = "00:00";
            this.songTimerOutput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // songProgressBar
            // 
            this.songProgressBar.Location = new System.Drawing.Point(85, 283);
            this.songProgressBar.Name = "songProgressBar";
            this.songProgressBar.Size = new System.Drawing.Size(254, 29);
            this.songProgressBar.TabIndex = 7;
            // 
            // startPlayButton
            // 
            this.startPlayButton.Image = ((System.Drawing.Image)(resources.GetObject("startPlayButton.Image")));
            this.startPlayButton.InitialImage = ((System.Drawing.Image)(resources.GetObject("startPlayButton.InitialImage")));
            this.startPlayButton.Location = new System.Drawing.Point(187, 226);
            this.startPlayButton.Name = "startPlayButton";
            this.startPlayButton.Size = new System.Drawing.Size(60, 51);
            this.startPlayButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.startPlayButton.TabIndex = 9;
            this.startPlayButton.TabStop = false;
            this.startPlayButton.Click += new System.EventHandler(this.playMusicButton);
            // 
            // prevSongButton
            // 
            this.prevSongButton.Image = ((System.Drawing.Image)(resources.GetObject("prevSongButton.Image")));
            this.prevSongButton.Location = new System.Drawing.Point(153, 239);
            this.prevSongButton.Name = "prevSongButton";
            this.prevSongButton.Size = new System.Drawing.Size(28, 29);
            this.prevSongButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.prevSongButton.TabIndex = 10;
            this.prevSongButton.TabStop = false;
            this.prevSongButton.Click += new System.EventHandler(this.prevSongButton_Click);
            // 
            // nextSongButton
            // 
            this.nextSongButton.Image = ((System.Drawing.Image)(resources.GetObject("nextSongButton.Image")));
            this.nextSongButton.Location = new System.Drawing.Point(253, 239);
            this.nextSongButton.Name = "nextSongButton";
            this.nextSongButton.Size = new System.Drawing.Size(28, 30);
            this.nextSongButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.nextSongButton.TabIndex = 11;
            this.nextSongButton.TabStop = false;
            this.nextSongButton.Click += new System.EventHandler(this.nextSongButton_Click);
            // 
            // shuffleMusicButton
            // 
            this.shuffleMusicButton.Image = ((System.Drawing.Image)(resources.GetObject("shuffleMusicButton.Image")));
            this.shuffleMusicButton.Location = new System.Drawing.Point(106, 238);
            this.shuffleMusicButton.Name = "shuffleMusicButton";
            this.shuffleMusicButton.Size = new System.Drawing.Size(33, 30);
            this.shuffleMusicButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.shuffleMusicButton.TabIndex = 12;
            this.shuffleMusicButton.TabStop = false;
            this.shuffleMusicButton.Click += new System.EventHandler(this.shuffleMusicButton_Click);
            // 
            // Musitron
            // 
            this.ClientSize = new System.Drawing.Size(437, 326);
            this.Controls.Add(this.shuffleMusicButton);
            this.Controls.Add(this.nextSongButton);
            this.Controls.Add(this.prevSongButton);
            this.Controls.Add(this.startPlayButton);
            this.Controls.Add(this.songProgressBar);
            this.Controls.Add(this.songTimerOutput);
            this.Controls.Add(this.soundVolume);
            this.Controls.Add(this.musicBox);
            this.Controls.Add(this.openFolderButton);
            this.Name = "Musitron";
            ((System.ComponentModel.ISupportInitialize)(this.soundVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPlayButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.prevSongButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextSongButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shuffleMusicButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void musicBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void musicBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            foreach (string file in files)
            {
                string songName = Path.GetFileName(file);
                musicBox.Items.Add(songName);
                importedSongsPaths.Add(musicBox.Items.Count - 1, file);
            }
        }

        private void shuffleMusicButton_Click(object sender, EventArgs e)
        {
            if (musicBox.SelectedIndex != -1)
            {
                Random random = new Random();
                int randomSong = random.Next(1, musicBox.Items.Count - 1);

                musicBox.SelectedIndex = randomSong;
                playMusic();
            }
        }
    }
}
