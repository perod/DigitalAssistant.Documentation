using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Model
{
    public class NugetPackage
    {
        public NugetPackage()
        {
            Authors = string.Empty;
            Description = string.Empty;
            Id = string.Empty;
            Owners = string.Empty;
            Version = string.Empty;
            LicenseUrl = string.Empty;
            ProjectUrl = string.Empty;
        }

        public string Authors { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public string LicenseUrl { get; set; }
        public string Owners { get; set; }
        public string ProjectUrl { get; set; }
        public string Version { get; set; }
    }
}
