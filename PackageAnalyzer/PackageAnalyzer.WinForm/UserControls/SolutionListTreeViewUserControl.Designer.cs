namespace PackageAnalyzer.WinForm.UserControls
{
    partial class SolutionListTreeViewUserControl
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
            this._tree = new System.Windows.Forms.TreeView();
            this._gbTreeview = new System.Windows.Forms.GroupBox();
            this._gbTreeview.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tree
            // 
            this._tree.CheckBoxes = true;
            this._tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tree.Location = new System.Drawing.Point(3, 16);
            this._tree.Name = "_tree";
            this._tree.Size = new System.Drawing.Size(627, 420);
            this._tree.TabIndex = 0;
            // 
            // _gbTreeview
            // 
            this._gbTreeview.Controls.Add(this._tree);
            this._gbTreeview.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gbTreeview.Location = new System.Drawing.Point(0, 0);
            this._gbTreeview.Name = "_gbTreeview";
            this._gbTreeview.Size = new System.Drawing.Size(633, 439);
            this._gbTreeview.TabIndex = 1;
            this._gbTreeview.TabStop = false;
            this._gbTreeview.Text = "Storage results";
            // 
            // SolutionListTreeViewUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._gbTreeview);
            this.Name = "SolutionListTreeViewUserControl";
            this.Size = new System.Drawing.Size(633, 439);
            this._gbTreeview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView _tree;
        private System.Windows.Forms.GroupBox _gbTreeview;
    }
}
