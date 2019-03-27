using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Services
{
    public interface ILicenseMapper
    {
        Task<string> GetLicenseType(string licenseUrl, string fallbackText = null);
    }
}
