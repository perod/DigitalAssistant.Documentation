using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PackageAnalyzer.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Services
{
    public class SolutionConfigurationReader : ISolutionConfigurationReader
    {
        private readonly IFileUtilities _fileUtilities;

        public SolutionConfigurationReader(IFileUtilities fileUtilities)
        {
            _fileUtilities = fileUtilities;
        }
        public async Task<List<SolutionConfiguration>> GetSolutionConfigurations(string solutionConfigurationFile)
        {
            var fileInfo = new FileInfo(solutionConfigurationFile);
            if (fileInfo.Exists)
            {
                var txt = await _fileUtilities.ReadFile(fileInfo);
                var solutionConfigurations = JsonConvert.DeserializeObject<List<SolutionConfiguration>>(txt);
                if(solutionConfigurations != null)
                {
                    foreach(var solutionConfiguration in solutionConfigurations)
                    {
                        if(solutionConfiguration.RenderProperties == null)
                        {
                            solutionConfiguration.RenderProperties = RenderProperties.Default;
                        }
                    }
                    return solutionConfigurations;
                }
                throw new Exception($"Unable to deserialize {solutionConfigurationFile} into a valid RenderInformation instance.");
            }
            else
            {
                throw new Exception($"File {solutionConfigurationFile} does not exist.");
            }
        }
    }
}
