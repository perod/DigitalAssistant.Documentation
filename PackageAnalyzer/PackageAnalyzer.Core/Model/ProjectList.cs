using System.Linq;
using System.Collections.Generic;

namespace PackageAnalyzer.Core.Model
{
    public class ProjectList : List<Project>
    {
        public ProjectReferenceList GetPackageReferences()
        {
            var retVal = new ProjectReferenceList();
            foreach(var project in this)
            {
                retVal.AddRange(project.ProjectReferences.GetPackageReferences());
            }

            return retVal;
        }
    }
}
