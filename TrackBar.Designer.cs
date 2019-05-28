namespace Syncr
{
    partial class TrackBar
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
            this.trackProg = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackProg)).BeginInit();
            this.SuspendLayout();
            // 
            // trackProg
            // 
            this.trackProg.Location = new System.Drawing.Point(3, 1);
            this.trackProg.Maximum = 10000;
            this.trackProg.Name = "trackProg";
            this.trackProg.Size = new System.Drawing.Size(1599, 45);
            this.trackProg.TabIndex = 0;
            this.trackProg.Scroll += new System.EventHandler(this.trackProg_Scroll);
            this.trackProg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackProg_MouseDown);
            // 
            // TrackBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1603, 42);
            this.Controls.Add(this.trackProg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrackBar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.TrackBar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackProg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackProg;
    }
}