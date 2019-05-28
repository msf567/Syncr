namespace Syncr
{
    partial class FunctionButtonString
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
            this.parameterText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button
            // 
            this.button.Size = new System.Drawing.Size(107, 25);
            // 
            // parameterText
            // 
            this.parameterText.Location = new System.Drawing.Point(4, 30);
            this.parameterText.Name = "parameterText";
            this.parameterText.Size = new System.Drawing.Size(106, 20);
            this.parameterText.TabIndex = 1;
            // 
            // FunctionButtonString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.parameterText);
            this.Name = "FunctionButtonString";
            this.Size = new System.Drawing.Size(113, 55);
            this.Controls.SetChildIndex(this.button, 0);
            this.Controls.SetChildIndex(this.parameterText, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox parameterText;
    }
}
