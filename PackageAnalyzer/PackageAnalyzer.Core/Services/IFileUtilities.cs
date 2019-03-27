using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace PackageAnalyzer.Core.Services
{
    public interface IFileUtilities
    {
        DirectoryInfo GetDirectory(string path, bool cleanDirectory);
        void DeleteDirectory(DirectoryInfo directory);
        void CleanDirectory(DirectoryInfo directory);
        Task CreateFile(string fileName, string content);
        Task<XmlDocument> ReadXmlDocument(FileInfo fileInfo);
        Task<string> ReadFile(FileInfo fileInfo);
    }
}
