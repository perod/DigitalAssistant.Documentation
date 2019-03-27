namespace PackageAnalyzer.WinForm.UserControls
{
    partial class ViewerUserControl
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
            this._gbResults = new System.Windows.Forms.GroupBox();
            this._webBrowser = new System.Windows.Forms.WebBrowser();
            this._gbResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbResults
            // 
            this._gbResults.Controls.Add(this._webBrowser);
            this._gbResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gbResults.Location = new System.Drawing.Point(0, 0);
            this._gbResults.Name = "_gbResults";
            this._gbResults.Size = new System.Drawing.Size(670, 557);
            this._gbResults.TabIndex = 0;
            this._gbResults.TabStop = false;
            this._gbResults.Text = "Results";
            // 
            // _webBrowser
            // 
            this._webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._webBrowser.Location = new System.Drawing.Point(3, 16);
            this._webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this._webBrowser.Name = "_webBrowser";
            this._webBrowser.Size = new System.Drawing.Size(664, 538);
            this._webBrowser.TabIndex = 0;
            // 
            // ViewerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._gbResults);
            this.Name = "ViewerUserControl";
            this.Size = new System.Drawing.Size(670, 557);
            this._gbResults.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox _gbResults;
        private System.Windows.Forms.WebBrowser _webBrowser;
    }
}
