using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;

namespace PackageAnalyzer.Core.Model
{
    public class StorageIdentifiersSegment
    {
        public List<StorageIdentifier> StorageIdentifiers { get; set; }
        public BlobContinuationToken ContinuationToken { get; set; }
    }
}
