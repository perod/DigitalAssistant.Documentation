using System;

namespace PackageAnalyzer.WinForm.Events
{
    public delegate void ActionRunEventHandler(object sender, ActionRunEventArgs e);

    public class ActionRunEventArgs : EventArgs
    {        
    }
}
