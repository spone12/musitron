
namespace MusicApp
{
    partial class Musitron
    {
        private System.ComponentModel.IContainer components = null;
    
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Button openFolderButton;
        private System.Windows.Forms.ListBox musicBox;
        private System.Windows.Forms.TrackBar soundVolume;
        private System.Windows.Forms.TextBox songTimerOutput;
        private System.Windows.Forms.ProgressBar songProgressBar;
        private System.Windows.Forms.PictureBox startPlayButton;
        private System.Windows.Forms.PictureBox prevSongButton;
        private System.Windows.Forms.PictureBox nextSongButton;
        private System.Windows.Forms.PictureBox shuffleMusicButton;
    }
}

