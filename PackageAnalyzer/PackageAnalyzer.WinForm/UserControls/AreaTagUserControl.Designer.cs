namespace PackageAnalyzer.WinForm.UserControls
{
    partial class AreaTagUserControl
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
            this._gbAreaTags = new System.Windows.Forms.GroupBox();
            this._cbAreaTags = new System.Windows.Forms.CheckedListBox();
            this._gbAreaTags.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbAreaTags
            // 
            this._gbAreaTags.Controls.Add(this._cbAreaTags);
            this._gbAreaTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gbAreaTags.Location = new System.Drawing.Point(0, 0);
            this._gbAreaTags.Name = "_gbAreaTags";
            this._gbAreaTags.Size = new System.Drawing.Size(150, 150);
            this._gbAreaTags.TabIndex = 0;
            this._gbAreaTags.TabStop = false;
            this._gbAreaTags.Text = "Area tags";
            // 
            // _cbAreaTags
            // 
            this._cbAreaTags.CheckOnClick = true;
            this._cbAreaTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this._cbAreaTags.FormattingEnabled = true;
            this._cbAreaTags.Location = new System.Drawing.Point(3, 16);
            this._cbAreaTags.Name = "_cbAreaTags";
            this._cbAreaTags.Size = new System.Drawing.Size(144, 131);
            this._cbAreaTags.TabIndex = 0;
            // 
            // AreaTagUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._gbAreaTags);
            this.Name = "AreaTagUserControl";
            this._gbAreaTags.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox _gbAreaTags;
        private System.Windows.Forms.CheckedListBox _cbAreaTags;
    }
}
