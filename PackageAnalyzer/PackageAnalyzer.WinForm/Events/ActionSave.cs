using System;

namespace PackageAnalyzer.WinForm.Events
{
    public delegate void ActionSaveEventHandler(object sender, ActionSaveEventArgs e);

    public class ActionSaveEventArgs : EventArgs
    {    
    }
}
