using PackageAnalyzer.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Services
{
    public interface IHtmlRenderer
    {
        Task<string> RenderToString(RenderProperties renderProperties, SolutionList solutionList, string titleText, string headerText);
        Task<string> Render(SolutionConfiguration solutionConfiguration, SolutionList solutionList);

    }
}
