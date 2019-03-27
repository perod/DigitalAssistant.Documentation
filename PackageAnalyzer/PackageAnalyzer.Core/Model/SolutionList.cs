using System;
using System.Collections.Generic;
using System.Linq;

namespace PackageAnalyzer.Core.Model
{
    public class SolutionList : List<Solution>
    {
        public ProjectReferenceList GetPackageReferences(bool excludeDuplicates = true, bool excludeLibraryDependencies = true, string versionSeparator = "\r\n")
        {
            var list = new List<ProjectReference>();

            foreach (var solution in this)
            {
                list.AddRange(solution?.Projects?.GetPackageReferences());
            }
            
            if(excludeLibraryDependencies)
            {
                list = list
                    .Where(p => 
                        !string.IsNullOrWhiteSpace(p.NugetPackage?.Id) && 
                        !string.IsNullOrWhiteSpace(p.NugetPackage?.Version))
                    .ToList();
            }

            if (excludeDuplicates)
            {
                var distinctList = list
                    .GroupBy(p => new { p.NugetPackage.Id })
                    .Select(g =>
                    {
                        var versions = string.Join(versionSeparator, g.Select(p => p.NugetPackage.Version).OrderByDescending(v => v).Distinct());
                        var projectReferenceWithLicense = g.OrderByDescending(p => p.NugetPackage.Version).FirstOrDefault();
                        projectReferenceWithLicense.NugetPackage.Version = versions;
                        return projectReferenceWithLicense;
                    })
                    .OrderBy(l => l.NugetPackage.Id)
                    .ToList();

                list = distinctList;
            }

            var retVal = new ProjectReferenceList();
            retVal.AddRange(list);
            return retVal;
        }
    }
}
