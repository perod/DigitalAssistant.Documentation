using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace PackageAnalyzer.Core.Services
{
    public class FileUtilities : IFileUtilities
    {
        public DirectoryInfo GetDirectory(string path, bool cleanDirectory)
        {
            var directory = new DirectoryInfo(path);

            if (!directory.Exists)
            {
                directory.Create();
            }

            if (cleanDirectory)
            {
                CleanDirectory(directory);
            }

            return directory;
        }

        public void DeleteDirectory(DirectoryInfo directory)
        {
            CleanDirectory(directory);
            directory.Delete();
        }

        public void CleanDirectory(DirectoryInfo directory)
        {
            foreach (var file in directory.EnumerateFiles())
            {
                file.Delete();
            }

            foreach (var childDirectory in directory.EnumerateDirectories())
            {
                CleanDirectory(childDirectory);
                childDirectory.Delete();
            }
        }

        public async Task CreateFile(string fileName, string content)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (var streamWriter = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write)))
            {
                await streamWriter.WriteAsync(content);
            }
        }

        public async Task<string> ReadFile(FileInfo fileInfo)
        {
            var retVal = String.Empty;
            using (var streamReader = fileInfo.OpenText())
            {
                retVal = await streamReader.ReadToEndAsync();
            }
            return retVal;
        }

        public async Task<XmlDocument> ReadXmlDocument(FileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                try
                {
                    var doc = new XmlDocument();

                    byte[] result;

                    using (var fs = fileInfo.OpenRead())
                    {
                        result = new byte[fs.Length];
                        await fs.ReadAsync(result, 0, (int)fs.Length);

                        using (var ms = new MemoryStream(result))
                        {
                            await ms.FlushAsync();
                            ms.Position = 0;
                            doc.Load(ms);
                        }
                    }
                    return doc;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to load contents of file {fileInfo.FullName}. Error: {ex.Message}");
                    return null;
                }
            }

            Console.WriteLine($"Unable to load {fileInfo.FullName}. Error: File does not exist.");
            return null;
        }

    }
}
