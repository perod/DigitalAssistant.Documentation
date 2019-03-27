using System.Collections.Generic;

namespace PackageAnalyzer.Core.Model
{
    public class LicenseMap
    {
        public string LicenseType { get; set; }
        public List<string> LicenseUrls { get; set; }
    }
}
