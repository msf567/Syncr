namespace Syncr
{
    partial class SyncrCommandLine
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
            this.commandLine = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // commandLine
            // 
            this.commandLine.BackColor = System.Drawing.Color.Black;
            this.commandLine.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commandLine.ForeColor = System.Drawing.Color.White;
            this.commandLine.Location = new System.Drawing.Point(12, 12);
            this.commandLine.Name = "commandLine";
            this.commandLine.Size = new System.Drawing.Size(689, 36);
            this.commandLine.TabIndex = 0;
            this.commandLine.KeyDown += new System.Windows.Forms.KeyEventHandler(this.commandLine_KeyDown);
            // 
            // SyncrCommandLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(714, 56);
            this.ControlBox = false;
            this.Controls.Add(this.commandLine);
            this.Name = "SyncrCommandLine";
            this.Text = "SCL";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox commandLine;
    }
}