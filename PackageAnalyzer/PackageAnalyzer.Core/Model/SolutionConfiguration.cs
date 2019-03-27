using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PackageAnalyzer.Core.Model
{
    public class SolutionConfiguration
    {
        [JsonConstructor]
        public SolutionConfiguration(List<string> solutionAreaTags)
        {
            if (solutionAreaTags != null)
            {
                foreach (var property in solutionAreaTags)
                {
                    SolutionAreaTag solutionAreaTag;
                    if (Enum.TryParse(property, out solutionAreaTag))
                    {
                        AreaTags |= solutionAreaTag;
                    }
                }
            }
        }

        public SolutionConfiguration() { }

        public string HeaderText { get; set;}
        public string PageTitle { get; set; }
        public string OutputPath { get; set; }
        public string IndexFileName { get; set; }
        public string RootFolder { get; set; }
        public List<SolutionInformation> Solutions { get; set; }
        public RenderProperties RenderProperties { get; set; }
        public string StorageIdentifier { get; set; }
        public SolutionAreaTag AreaTags { get; set; }
    }
}
