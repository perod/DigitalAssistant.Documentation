using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using PackageAnalyzer.Core.Model;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Services
{
    public class StorageService : IStorageService
    {
        private const string _datePattern = "yyyy-MM-ddTHH:mm:ss-fff-zz";

        private CloudBlobContainer _blobContainer;
        private string _blobContainerName;
        private string _storageConnectionString;
        private string _buildId;

        public void Initialize(string buildId, string storageConnectionString, string blobContainerName)
        {
            _storageConnectionString = storageConnectionString;
            _blobContainerName = blobContainerName;
            _buildId = !string.IsNullOrWhiteSpace(buildId) ? $"_{buildId}" : string.Empty;
        }

        public async Task<bool> SetSolutionList(SolutionConfiguration solutionConfiguration, SolutionList solutionList)
        {
            var identifier = CreateIdentifier(solutionConfiguration);

            if (!string.IsNullOrWhiteSpace(identifier))
            {
                var blockBlob = await GetBlockBlob(identifier);

                if (blockBlob != null && !(await blockBlob.ExistsAsync()))
                {
                    var serializedSolutionList = JsonConvert.SerializeObject(solutionList, Formatting.None);
                    using (var ms = new MemoryStream())
                    {
                        using (var writer = new StreamWriter(ms))
                        {
                            writer.Write(serializedSolutionList);
                            writer.Flush();
                            ms.Seek(0, SeekOrigin.Begin);

                            await blockBlob.UploadFromStreamAsync(ms);

                            Console.WriteLine($"Stored solution results with identifier {identifier}.");
                            return true;
                        }
                    }
                }
                {
                    var reason = blockBlob != null ? "Block blob already exits." : "Could not get a block blob reference.";
                    Console.WriteLine($"Could not store solution information in backing storage. Reason: {reason}");
                }
            }
            else
            {
                Console.WriteLine("Could not create a valid storage identifier. Results will not be stored in the backing storage.");
            }

            return false;
        }

        public async Task<StorageIdentifiersSegment> GetSolutionListIdentifersSegmented(BlobContinuationToken token = null, int segmentSize = 100)
        {
            var container = await GetCloudBlobContainer();
            if (container != null)
            {
                var segment = await container.ListBlobsSegmentedAsync(string.Empty, true, BlobListingDetails.None, segmentSize, token, null, null);

                var storageIdentifiers = segment.Results.Select(r =>
                {
                    var blob = r as CloudBlockBlob;
                    return blob != null
                    ? ParseStorageIdentifer(blob.Name)
                    : null;
                }).OfType<StorageIdentifier>()
                .ToList();

                return new StorageIdentifiersSegment
                {
                    StorageIdentifiers = storageIdentifiers,
                    ContinuationToken = segment.ContinuationToken
                };
            }
            return null;
        }

        public async Task<SolutionList> GetSolutionList(string identifier)
        {
            var blockBlob = await GetBlockBlob(identifier);

            if (blockBlob != null && await blockBlob.ExistsAsync())
            {
                using (var memoryStream = new MemoryStream())
                {
                    await blockBlob.DownloadToStreamAsync(memoryStream);

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(memoryStream))
                    {
                        var str = reader.ReadToEnd();
                        return JsonConvert.DeserializeObject<SolutionList>(str);
                    }
                }
            }

            return null;
        }

        public async Task<bool> DeleteSolutionList(string identifier)
        {
            var blockBlob = await GetBlockBlob(identifier);
            return await blockBlob?.DeleteIfExistsAsync() ;
        }

        private async Task<CloudBlockBlob> GetBlockBlob(string identifier)
        {
            var container = await GetCloudBlobContainer();
            if (container != null)
            {
                return container.GetBlockBlobReference(identifier);
            }

            return null;
        }

        private async Task<CloudBlobContainer> GetCloudBlobContainer()
        {
            if (_blobContainer == null)
            {
                var storageAccount = GetStorageAccount();

                if (storageAccount != null)
                {
                    var blobClient = storageAccount.CreateCloudBlobClient();
                    var container = blobClient.GetContainerReference(_blobContainerName);
                    await container.CreateIfNotExistsAsync();

                    container.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Off
                        });

                    _blobContainer = container;
                }
            }
            return _blobContainer;
        }

        private CloudStorageAccount GetStorageAccount()
        {
            CloudStorageAccount storageAccount;
            return CloudStorageAccount.TryParse(_storageConnectionString, out storageAccount) ? storageAccount : null;
        }

        private string CreateIdentifier(SolutionConfiguration solutionConfiguration)
        {
            return !string.IsNullOrWhiteSpace(solutionConfiguration?.StorageIdentifier)
                ? $"{solutionConfiguration.StorageIdentifier}_{ CreateDateIdentifier()}_{(int)solutionConfiguration.AreaTags}{_buildId}"
                : null;
        }

        private StorageIdentifier ParseStorageIdentifer(string blobName)
        {
            var parts = blobName.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
            {
                var date = ParseDateFromIdentifer(parts[1]);
                int iAreaTag;

                if (date.HasValue && int.TryParse(parts[2], out iAreaTag))
                {
                    return new StorageIdentifier
                    {
                        BlobName = blobName,
                        Name = parts[0],
                        Date = date.Value,
                        AreaTags = (SolutionAreaTag)Enum.ToObject(typeof(SolutionAreaTag), iAreaTag),
                        BuildId = parts.Length > 3 ? parts[3] : null
                    };
                }
            }
            return null;
        }

        private DateTime? ParseDateFromIdentifer(string dateString)
        {
            DateTime parsedDate;
            if (DateTime.TryParseExact(dateString, _datePattern, null, System.Globalization.DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }
            return null;
        }

        private string CreateDateIdentifier()
        {
            return DateTime.Now.ToString(_datePattern);
        }
    }
}
