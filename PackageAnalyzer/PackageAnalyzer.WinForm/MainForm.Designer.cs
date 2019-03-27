namespace PackageAnalyzer.WinForm
{
    partial class MainForm
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
            this._mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this._menuSplitContainer = new System.Windows.Forms.SplitContainer();
            this._viewerAndOptionsSplitContainer = new System.Windows.Forms.SplitContainer();
            this._solutionListTreeViewUserControl = new PackageAnalyzer.WinForm.UserControls.SolutionListTreeViewUserControl();
            this._actionUserControl = new PackageAnalyzer.WinForm.UserControls.ActionUserControl();
            this._areaTagUserControl = new PackageAnalyzer.WinForm.UserControls.AreaTagUserControl();
            this._viewerUserControl = new PackageAnalyzer.WinForm.UserControls.ViewerUserControl();
            this._renderOptionsTreeViewUserControl = new PackageAnalyzer.WinForm.UserControls.RenderOptionsTreeViewUserControl();
            ((System.ComponentModel.ISupportInitialize)(this._mainSplitContainer)).BeginInit();
            this._mainSplitContainer.Panel1.SuspendLayout();
            this._mainSplitContainer.Panel2.SuspendLayout();
            this._mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._menuSplitContainer)).BeginInit();
            this._menuSplitContainer.Panel1.SuspendLayout();
            this._menuSplitContainer.Panel2.SuspendLayout();
            this._menuSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._viewerAndOptionsSplitContainer)).BeginInit();
            this._viewerAndOptionsSplitContainer.Panel1.SuspendLayout();
            this._viewerAndOptionsSplitContainer.Panel2.SuspendLayout();
            this._viewerAndOptionsSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // _mainSplitContainer
            // 
            this._mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this._mainSplitContainer.Name = "_mainSplitContainer";
            // 
            // _mainSplitContainer.Panel1
            // 
            this._mainSplitContainer.Panel1.Controls.Add(this._menuSplitContainer);
            this._mainSplitContainer.Panel1MinSize = 265;
            // 
            // _mainSplitContainer.Panel2
            // 
            this._mainSplitContainer.Panel2.Controls.Add(this._viewerAndOptionsSplitContainer);
            this._mainSplitContainer.Size = new System.Drawing.Size(1036, 581);
            this._mainSplitContainer.SplitterDistance = 265;
            this._mainSplitContainer.TabIndex = 1;
            // 
            // _menuSplitContainer
            // 
            this._menuSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._menuSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this._menuSplitContainer.Location = new System.Drawing.Point(0, 0);
            this._menuSplitContainer.Name = "_menuSplitContainer";
            this._menuSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _menuSplitContainer.Panel1
            // 
            this._menuSplitContainer.Panel1.Controls.Add(this._solutionListTreeViewUserControl);
            // 
            // _menuSplitContainer.Panel2
            // 
            this._menuSplitContainer.Panel2.Controls.Add(this._actionUserControl);
            this._menuSplitContainer.Panel2.Controls.Add(this._areaTagUserControl);
            this._menuSplitContainer.Size = new System.Drawing.Size(265, 581);
            this._menuSplitContainer.SplitterDistance = 210;
            this._menuSplitContainer.TabIndex = 0;
            // 
            // _viewerAndOptionsSplitContainer
            // 
            this._viewerAndOptionsSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewerAndOptionsSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this._viewerAndOptionsSplitContainer.Location = new System.Drawing.Point(0, 0);
            this._viewerAndOptionsSplitContainer.Name = "_viewerAndOptionsSplitContainer";
            // 
            // _viewerAndOptionsSplitContainer.Panel1
            // 
            this._viewerAndOptionsSplitContainer.Panel1.Controls.Add(this._viewerUserControl);
            // 
            // _viewerAndOptionsSplitContainer.Panel2
            // 
            this._viewerAndOptionsSplitContainer.Panel2.Controls.Add(this._renderOptionsTreeViewUserControl);
            this._viewerAndOptionsSplitContainer.Panel2Collapsed = true;
            this._viewerAndOptionsSplitContainer.Size = new System.Drawing.Size(767, 581);
            this._viewerAndOptionsSplitContainer.SplitterDistance = 517;
            this._viewerAndOptionsSplitContainer.TabIndex = 1;
            // 
            // _solutionListTreeViewUserControl
            // 
            this._solutionListTreeViewUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._solutionListTreeViewUserControl.Location = new System.Drawing.Point(0, 0);
            this._solutionListTreeViewUserControl.Name = "_solutionListTreeViewUserControl";
            this._solutionListTreeViewUserControl.Size = new System.Drawing.Size(265, 210);
            this._solutionListTreeViewUserControl.TabIndex = 0;
            // 
            // _actionUserControl
            // 
            this._actionUserControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._actionUserControl.Location = new System.Drawing.Point(6, 246);
            this._actionUserControl.Name = "_actionUserControl";
            this._actionUserControl.Size = new System.Drawing.Size(256, 118);
            this._actionUserControl.TabIndex = 1;
            // 
            // _areaTagUserControl
            // 
            this._areaTagUserControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._areaTagUserControl.Location = new System.Drawing.Point(3, 3);
            this._areaTagUserControl.Name = "_areaTagUserControl";
            this._areaTagUserControl.Size = new System.Drawing.Size(259, 244);
            this._areaTagUserControl.TabIndex = 0;
            // 
            // _viewerUserControl
            // 
            this._viewerUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewerUserControl.Location = new System.Drawing.Point(0, 0);
            this._viewerUserControl.Name = "_viewerUserControl";
            this._viewerUserControl.Size = new System.Drawing.Size(767, 581);
            this._viewerUserControl.TabIndex = 0;
            // 
            // _renderOptionsTreeViewUserControl
            // 
            this._renderOptionsTreeViewUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._renderOptionsTreeViewUserControl.Location = new System.Drawing.Point(0, 0);
            this._renderOptionsTreeViewUserControl.Name = "_renderOptionsTreeViewUserControl";
            this._renderOptionsTreeViewUserControl.Size = new System.Drawing.Size(236, 581);
            this._renderOptionsTreeViewUserControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 581);
            this.Controls.Add(this._mainSplitContainer);
            this.Name = "MainForm";
            this.Text = "Solution Third party package analyzer";
            this._mainSplitContainer.Panel1.ResumeLayout(false);
            this._mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._mainSplitContainer)).EndInit();
            this._mainSplitContainer.ResumeLayout(false);
            this._menuSplitContainer.Panel1.ResumeLayout(false);
            this._menuSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._menuSplitContainer)).EndInit();
            this._menuSplitContainer.ResumeLayout(false);
            this._viewerAndOptionsSplitContainer.Panel1.ResumeLayout(false);
            this._viewerAndOptionsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._viewerAndOptionsSplitContainer)).EndInit();
            this._viewerAndOptionsSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.SolutionListTreeViewUserControl _solutionListTreeViewUserControl;
        private System.Windows.Forms.SplitContainer _mainSplitContainer;
        private System.Windows.Forms.SplitContainer _menuSplitContainer;
        private UserControls.AreaTagUserControl _areaTagUserControl;
        private UserControls.ActionUserControl _actionUserControl;
        private UserControls.ViewerUserControl _viewerUserControl;
        private System.Windows.Forms.SplitContainer _viewerAndOptionsSplitContainer;
        private UserControls.RenderOptionsTreeViewUserControl _renderOptionsTreeViewUserControl;
    }
}

