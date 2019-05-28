namespace Syncr
{
    partial class FunctionButton
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
            this.button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button
            // 
            this.button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button.Location = new System.Drawing.Point(3, 3);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(107, 24);
            this.button.TabIndex = 0;
            this.button.Text = "functionName";
            this.button.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button.UseVisualStyleBackColor = true;
            // 
            // FunctionButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.button);
            this.Name = "FunctionButton";
            this.Size = new System.Drawing.Size(113, 55);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button button;
    }
}
