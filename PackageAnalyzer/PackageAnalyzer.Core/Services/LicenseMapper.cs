using Newtonsoft.Json;
using PackageAnalyzer.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Services
{
    public class LicenseMapper : ILicenseMapper
    {
        private readonly IFileUtilities _fileUtilities;
        private List<LicenseMap> _licenseMaps;
        
        public LicenseMapper(IFileUtilities fileUtilities)
        {
            _fileUtilities = fileUtilities;
        }

        public async Task<string> GetLicenseType(string licenseUrl, string fallbackText = null)
        {
            var licenseMaps = await GetLicenseMaps();
            var identifier = string.Empty;

            if (!string.IsNullOrWhiteSpace(licenseUrl))
            {
                identifier = licenseMaps
                    .Where(l => l.LicenseUrls.Contains(licenseUrl.Trim()))
                    .FirstOrDefault()?.LicenseType;
            }

            return !string.IsNullOrWhiteSpace(identifier) ? identifier : !string.IsNullOrWhiteSpace(fallbackText) ? fallbackText : string.Empty;
        }

        private async Task<List<LicenseMap>> GetLicenseMaps()
        {
            if(_licenseMaps == null)
            {
                var assemblyLocation = Assembly.GetExecutingAssembly().Location;
                var assemblyFileInfo = new FileInfo(assemblyLocation);
                var fileName = Path.Combine(assemblyFileInfo.DirectoryName, @"json\licenseMap.json");
                
                _licenseMaps = new List<LicenseMap>();
                var licenseMapFileInfo = new FileInfo(fileName);
                if (licenseMapFileInfo.Exists)
                {
                    var txt = await _fileUtilities.ReadFile(licenseMapFileInfo);
                    var licenseMaps = JsonConvert.DeserializeObject<List<LicenseMap>>(txt);
                    if (licenseMaps != null)
                    {
                        _licenseMaps = licenseMaps;
                    }
                    else
                    {
                        Console.WriteLine($"Unable to read license maps from file {assemblyFileInfo.Name}");
                    }
                }
                else
                {
                    Console.WriteLine($"Unable to locate license map file {assemblyFileInfo.Name}");
                }
            }

            return _licenseMaps;
        }
    }
}
