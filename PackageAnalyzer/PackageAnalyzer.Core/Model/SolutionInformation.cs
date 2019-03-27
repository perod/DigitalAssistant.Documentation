using System.Collections.Generic;

namespace PackageAnalyzer.Core.Model
{
    public class SolutionInformation
    {
        public string SolutionFile { get; set; }
        public List<string> ProjectReferencesToIgnore { get; set; }
        public List<string> ProjectsToIgnore { get; set; }
    }
}
