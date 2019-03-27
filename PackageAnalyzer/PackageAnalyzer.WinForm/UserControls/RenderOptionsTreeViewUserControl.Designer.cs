namespace PackageAnalyzer.WinForm.UserControls
{
    partial class RenderOptionsTreeViewUserControl
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
            this._gbRenderOptions = new System.Windows.Forms.GroupBox();
            this._treeViewRenderingOptions = new System.Windows.Forms.TreeView();
            this._gbRenderOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbRenderOptions
            // 
            this._gbRenderOptions.Controls.Add(this._treeViewRenderingOptions);
            this._gbRenderOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gbRenderOptions.Location = new System.Drawing.Point(0, 0);
            this._gbRenderOptions.Name = "_gbRenderOptions";
            this._gbRenderOptions.Size = new System.Drawing.Size(545, 444);
            this._gbRenderOptions.TabIndex = 0;
            this._gbRenderOptions.TabStop = false;
            this._gbRenderOptions.Text = "Rendering options";
            // 
            // _treeViewRenderingOptions
            // 
            this._treeViewRenderingOptions.CheckBoxes = true;
            this._treeViewRenderingOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeViewRenderingOptions.Location = new System.Drawing.Point(3, 16);
            this._treeViewRenderingOptions.Name = "_treeViewRenderingOptions";
            this._treeViewRenderingOptions.Size = new System.Drawing.Size(539, 425);
            this._treeViewRenderingOptions.TabIndex = 0;
            // 
            // RenderOptionsTreeViewUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._gbRenderOptions);
            this.Name = "RenderOptionsTreeViewUserControl";
            this.Size = new System.Drawing.Size(545, 444);
            this._gbRenderOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox _gbRenderOptions;
        private System.Windows.Forms.TreeView _treeViewRenderingOptions;
    }
}
