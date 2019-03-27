using System;
using System.Collections.Generic;

namespace PackageAnalyzer.WinForm.Events
{
    public delegate void ActionPresentBlobsEventHandler(object sender, ActionPresentBlobsEventArgs e);

    public class ActionPresentBlobsEventArgs : EventArgs
    {
        public ActionPresentBlobsEventArgs(List<string> blobNames)
        {
            BlobNames = blobNames;
        }

        public List<string> BlobNames { get; private set; }
    }
}
