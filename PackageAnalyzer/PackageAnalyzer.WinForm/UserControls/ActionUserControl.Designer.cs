namespace PackageAnalyzer.WinForm.UserControls
{
    partial class ActionUserControl
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
            this._bRun = new System.Windows.Forms.Button();
            this._bRefresh = new System.Windows.Forms.Button();
            this._bSaveToFile = new System.Windows.Forms.Button();
            this._bToggleRenderingOptions = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _bRun
            // 
            this._bRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._bRun.Location = new System.Drawing.Point(3, 4);
            this._bRun.Name = "_bRun";
            this._bRun.Size = new System.Drawing.Size(246, 52);
            this._bRun.TabIndex = 0;
            this._bRun.Text = "Run";
            this._bRun.UseVisualStyleBackColor = true;
            // 
            // _bRefresh
            // 
            this._bRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._bRefresh.Location = new System.Drawing.Point(87, 62);
            this._bRefresh.Name = "_bRefresh";
            this._bRefresh.Size = new System.Drawing.Size(78, 52);
            this._bRefresh.TabIndex = 1;
            this._bRefresh.Text = "Refresh stored results";
            this._bRefresh.UseVisualStyleBackColor = true;
            // 
            // _bSaveToFile
            // 
            this._bSaveToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._bSaveToFile.Location = new System.Drawing.Point(3, 62);
            this._bSaveToFile.Name = "_bSaveToFile";
            this._bSaveToFile.Size = new System.Drawing.Size(78, 52);
            this._bSaveToFile.TabIndex = 2;
            this._bSaveToFile.Text = "Save results to file";
            this._bSaveToFile.UseVisualStyleBackColor = true;
            // 
            // _bToggleRenderingOptions
            // 
            this._bToggleRenderingOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._bToggleRenderingOptions.Location = new System.Drawing.Point(171, 62);
            this._bToggleRenderingOptions.Name = "_bToggleRenderingOptions";
            this._bToggleRenderingOptions.Size = new System.Drawing.Size(78, 52);
            this._bToggleRenderingOptions.TabIndex = 3;
            this._bToggleRenderingOptions.Text = "Toggle rendering options";
            this._bToggleRenderingOptions.UseVisualStyleBackColor = true;
            // 
            // ActionUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._bToggleRenderingOptions);
            this.Controls.Add(this._bSaveToFile);
            this.Controls.Add(this._bRefresh);
            this.Controls.Add(this._bRun);
            this.Name = "ActionUserControl";
            this.Size = new System.Drawing.Size(255, 118);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _bRun;
        private System.Windows.Forms.Button _bRefresh;
        private System.Windows.Forms.Button _bSaveToFile;
        private System.Windows.Forms.Button _bToggleRenderingOptions;
    }
}
