using System.Collections.Generic;
using System.Linq;

namespace PackageAnalyzer.Core.Model
{
    public class ProjectReferenceList : List<ProjectReference>
    {
        public ProjectReferenceList GetProjectReferencesByReferenceType(ReferenceType referenceType)
        {
            var retVal = new ProjectReferenceList();
            retVal.AddRange(this.Where(p => p.ReferenceType == referenceType));
            return retVal;
        }

        public ProjectReferenceList GetPackageReferences()
        {
            return GetProjectReferencesByReferenceType(ReferenceType.PackageReference);
        }

        public ProjectReferenceList GetPureReferences()
        {
            return GetProjectReferencesByReferenceType(ReferenceType.Reference);
        }

        public ProjectReferenceList GetProjectReferences()
        {
            return GetProjectReferencesByReferenceType(ReferenceType.ProjectReference);
        }
    }
}
