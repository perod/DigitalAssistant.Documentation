using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageAnalyzer.Cleanup.Function.Services
{
    internal class CleanerEngine : ICleanerEngine
    {
        private readonly IApplicationLogger _logger;
        private readonly IApplicationStorageService _storageService;
        private const string _logSource = "CleanerEngine";

        public CleanerEngine(IApplicationStorageService storageService, IApplicationLogger logger)
        {
            _storageService = storageService;
            _logger = logger;
        }
        
        public async Task Cleanup(int numItemsToKeep = 5)
        {
            var storageIdentifiers = await _storageService.GetSolutionListStorageIdentifiers();

            if (storageIdentifiers.Any() == true)
            {
                var entriesToDelete = storageIdentifiers
                    .GroupBy(s => s.Name)
                    .Where(g => g.Count() > numItemsToKeep)
                    .SelectMany(g => g
                        .OrderByDescending(s => s.Date)
                        .Skip(numItemsToKeep))
                    .Select(s => s.BlobName);
                
                if (entriesToDelete?.Any() == true)
                {
                    var tasks = new List<Task<bool>>();
                    var msg = $"These blobs will be removed:{Environment.NewLine}";

                    foreach (var blobName in entriesToDelete)
                    {
                        msg += $"{blobName}{Environment.NewLine}";
                        tasks.Add(_storageService.Delete(blobName));
                    }

                    _logger.Information(msg, _logSource);
                    await Task.WhenAll(tasks);
                }
                else
                {
                    _logger.Information("There are no old entries to remove.", _logSource);
                }
            }
            else
            {
                _logger.Information("There are no entries to remove.", _logSource);
            }
        }
    }
}
