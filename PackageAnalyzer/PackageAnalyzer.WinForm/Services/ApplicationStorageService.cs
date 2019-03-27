using PackageAnalyzer.Core.Model;
using PackageAnalyzer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageAnalyzer.WinForm.Services
{
    public class ApplicationStorageService : IApplicationStorageService
    {
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IStorageService _storageService;

        public ApplicationStorageService(IStorageService storageService, IApplicationConfiguration applicationConfiguration)
        {
            _applicationConfiguration = applicationConfiguration;
            _storageService = storageService;
            _storageService.Initialize(string.Empty, _applicationConfiguration.StorageConnectionString, _applicationConfiguration.StorageContainerName);
        }

        public async Task<List<StorageIdentifier>> GetSolutionListStorageIdentifiers()
        {
            var storageIdentifiers = new List<StorageIdentifier>();

            var segment = await _storageService.GetSolutionListIdentifersSegmented(segmentSize: 100);

            if (segment?.StorageIdentifiers.Any() == true)
            {
                storageIdentifiers.AddRange(segment.StorageIdentifiers);

                while (segment?.ContinuationToken != null)
                {
                    segment = await _storageService.GetSolutionListIdentifersSegmented(
                        token: segment.ContinuationToken,
                        segmentSize: 100);

                    if (segment?.StorageIdentifiers.Any() == true)
                    {
                        storageIdentifiers.AddRange(segment.StorageIdentifiers);
                    }
                }
            }

            return storageIdentifiers;
        }

        public async Task<SolutionList> GetSolutionList(string blobName)
        {
            return await _storageService.GetSolutionList(blobName);
        }
    }
}
