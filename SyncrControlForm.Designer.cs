namespace Syncr
{
    partial class SyncrControlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncrControlForm));
            this.stopButton = new System.Windows.Forms.Button();
            this.tempoTimer = new System.Windows.Forms.Timer(this.components);
            this.play = new System.Windows.Forms.Button();
            this.tapButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.colorButton = new System.Windows.Forms.Button();
            this.volumeBar = new System.Windows.Forms.TrackBar();
            this.cmdButton = new System.Windows.Forms.Button();
            this.midiEnabled = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.volumeBar)).BeginInit();
            this.SuspendLayout();
            // 
            // stopButton
            // 
            this.stopButton.BackColor = System.Drawing.Color.Khaki;
            this.stopButton.Location = new System.Drawing.Point(532, 41);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 0;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = false;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // play
            // 
            this.play.BackColor = System.Drawing.Color.Khaki;
            this.play.Location = new System.Drawing.Point(532, 12);
            this.play.Name = "play";
            this.play.Size = new System.Drawing.Size(75, 23);
            this.play.TabIndex = 1;
            this.play.Text = "Play";
            this.play.UseVisualStyleBackColor = false;
            this.play.Click += new System.EventHandler(this.play_Click);
            // 
            // tapButton
            // 
            this.tapButton.BackColor = System.Drawing.Color.Khaki;
            this.tapButton.Location = new System.Drawing.Point(532, 70);
            this.tapButton.Name = "tapButton";
            this.tapButton.Size = new System.Drawing.Size(75, 23);
            this.tapButton.TabIndex = 2;
            this.tapButton.Text = "Tap";
            this.tapButton.UseVisualStyleBackColor = false;
            this.tapButton.Click += new System.EventHandler(this.tapClick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.MaximumSize = new System.Drawing.Size(515, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(515, 248);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // colorButton
            // 
            this.colorButton.BackColor = System.Drawing.Color.Khaki;
            this.colorButton.Location = new System.Drawing.Point(532, 99);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(75, 23);
            this.colorButton.TabIndex = 7;
            this.colorButton.Text = "Color";
            this.colorButton.UseVisualStyleBackColor = false;
            this.colorButton.Click += new System.EventHandler(this.colorButton_Click);
            // 
            // volumeBar
            // 
            this.volumeBar.LargeChange = 20;
            this.volumeBar.Location = new System.Drawing.Point(562, 128);
            this.volumeBar.Maximum = 100;
            this.volumeBar.Name = "volumeBar";
            this.volumeBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.volumeBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.volumeBar.Size = new System.Drawing.Size(45, 104);
            this.volumeBar.SmallChange = 10;
            this.volumeBar.TabIndex = 8;
            this.volumeBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.volumeBar.Value = 100;
            this.volumeBar.Scroll += new System.EventHandler(this.volumeBar_Scroll);
            // 
            // cmdButton
            // 
            this.cmdButton.BackColor = System.Drawing.Color.Khaki;
            this.cmdButton.Location = new System.Drawing.Point(613, 12);
            this.cmdButton.Name = "cmdButton";
            this.cmdButton.Size = new System.Drawing.Size(75, 23);
            this.cmdButton.TabIndex = 9;
            this.cmdButton.Text = "Cmd";
            this.cmdButton.UseVisualStyleBackColor = false;
            this.cmdButton.Click += new System.EventHandler(this.cmdButton_Click);
            // 
            // midiEnabled
            // 
            this.midiEnabled.AutoSize = true;
            this.midiEnabled.Checked = true;
            this.midiEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.midiEnabled.ForeColor = System.Drawing.Color.White;
            this.midiEnabled.Location = new System.Drawing.Point(614, 42);
            this.midiEnabled.Name = "midiEnabled";
            this.midiEnabled.Size = new System.Drawing.Size(87, 17);
            this.midiEnabled.TabIndex = 10;
            this.midiEnabled.Text = "Midi Enabled";
            this.midiEnabled.UseVisualStyleBackColor = true;
            this.midiEnabled.CheckedChanged += new System.EventHandler(this.midiEnabled_CheckedChanged);
            // 
            // SyncrControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(707, 248);
            this.Controls.Add(this.midiEnabled);
            this.Controls.Add(this.cmdButton);
            this.Controls.Add(this.volumeBar);
            this.Controls.Add(this.colorButton);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tapButton);
            this.Controls.Add(this.play);
            this.Controls.Add(this.stopButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SyncrControlForm";
            this.ShowInTaskbar = false;
            this.Text = "Superman";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShowForm_FormClosed);
            this.Load += new System.EventHandler(this.ShowForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.volumeBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Timer tempoTimer;
        private System.Windows.Forms.Button play;
        private System.Windows.Forms.Button tapButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.TrackBar volumeBar;
        private System.Windows.Forms.Button cmdButton;
        private System.Windows.Forms.CheckBox midiEnabled;
    }
}

