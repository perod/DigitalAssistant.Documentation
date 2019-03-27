using System;

namespace PackageAnalyzer.Core.Model
{
    public class StorageIdentifier
    {
        public string BlobName { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string BuildId { get; set; }
        public SolutionAreaTag AreaTags { get; set; }
    }
}
