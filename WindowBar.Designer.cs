namespace Syncr
{
    partial class WindowBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.topButton = new System.Windows.Forms.Button();
            this.bottomButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // topButton
            // 
            this.topButton.Location = new System.Drawing.Point(4, 4);
            this.topButton.Name = "topButton";
            this.topButton.Size = new System.Drawing.Size(75, 23);
            this.topButton.TabIndex = 0;
            this.topButton.Text = "TOP";
            this.topButton.UseVisualStyleBackColor = true;
            this.topButton.Click += new System.EventHandler(this.topButton_Click);
            // 
            // bottomButton
            // 
            this.bottomButton.Location = new System.Drawing.Point(86, 4);
            this.bottomButton.Name = "bottomButton";
            this.bottomButton.Size = new System.Drawing.Size(75, 23);
            this.bottomButton.TabIndex = 1;
            this.bottomButton.Text = "BOTTOM";
            this.bottomButton.UseVisualStyleBackColor = true;
            this.bottomButton.Click += new System.EventHandler(this.bottomButton_Click);
            // 
            // quitButton
            // 
            this.quitButton.Location = new System.Drawing.Point(168, 4);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(75, 23);
            this.quitButton.TabIndex = 2;
            this.quitButton.Text = "QUIT";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            // 
            // WindowBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.bottomButton);
            this.Controls.Add(this.topButton);
            this.Name = "WindowBar";
            this.Size = new System.Drawing.Size(249, 33);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button topButton;
        private System.Windows.Forms.Button bottomButton;
        private System.Windows.Forms.Button quitButton;
    }
}
