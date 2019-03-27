using System;

namespace PackageAnalyzer.WinForm.Events
{
    public delegate void ActionRefreshEventHandler(object sender, ActionRefreshEventArgs e);

    public class ActionRefreshEventArgs : EventArgs
    {    
    }
}
