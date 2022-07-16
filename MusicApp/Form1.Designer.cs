
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
        private System.Windows.Forms.Button startPlayButton;
        private System.Windows.Forms.ListBox musicBox;
        private System.Windows.Forms.Button stopMusicButton;
        private System.Windows.Forms.TrackBar soundVolume;
    }
}

