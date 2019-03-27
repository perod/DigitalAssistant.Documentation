using System.Collections.Generic;
using System.Linq;

namespace PackageAnalyzer.Core.Model
{
    public class PackageList : List<Package>
    {
        public Package GetPackageByName(string name)
        {
            return this.Where(p => p.Id.IndexOf(name) == 0).FirstOrDefault();
        }
    }
}
