namespace Syncr
{
    partial class FunctionPanel
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
            this.groupNameLabel = new System.Windows.Forms.Label();
            this.functionButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // groupNameLabel
            // 
            this.groupNameLabel.AutoSize = true;
            this.groupNameLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupNameLabel.Location = new System.Drawing.Point(3, 0);
            this.groupNameLabel.Name = "groupNameLabel";
            this.groupNameLabel.Size = new System.Drawing.Size(149, 25);
            this.groupNameLabel.TabIndex = 0;
            this.groupNameLabel.Text = "GROUPNAME";
            this.groupNameLabel.Click += new System.EventHandler(this.groupNameLabel_Click);
            // 
            // functionButtonPanel
            // 
            this.functionButtonPanel.AutoSize = true;
            this.functionButtonPanel.Location = new System.Drawing.Point(4, 28);
            this.functionButtonPanel.MaximumSize = new System.Drawing.Size(243, 30000);
            this.functionButtonPanel.Name = "functionButtonPanel";
            this.functionButtonPanel.Size = new System.Drawing.Size(243, 88);
            this.functionButtonPanel.TabIndex = 1;
            // 
            // FunctionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.functionButtonPanel);
            this.Controls.Add(this.groupNameLabel);
            this.MaximumSize = new System.Drawing.Size(250, 0);
            this.Name = "FunctionPanel";
            this.Size = new System.Drawing.Size(250, 119);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label groupNameLabel;
        private System.Windows.Forms.FlowLayoutPanel functionButtonPanel;
    }
}
