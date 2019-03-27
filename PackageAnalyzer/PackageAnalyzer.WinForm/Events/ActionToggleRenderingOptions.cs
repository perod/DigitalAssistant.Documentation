using System;

namespace PackageAnalyzer.WinForm.Events
{
    public delegate void ActionToggleRenderingOptionsEventHandler(object sender, ActionToggleRenderingOptionsEventArgs e);

    public class ActionToggleRenderingOptionsEventArgs : EventArgs
    {
    }
}
