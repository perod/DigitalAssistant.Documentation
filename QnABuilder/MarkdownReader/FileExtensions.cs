using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownReader
{
    public static class FileExtensions
    {
        public static async Task<string> ReadAllTextAsync(string path)
        {
            var retVal = string.Empty;
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    retVal = await reader.ReadToEndAsync();
                }
            }
            return retVal;
        }

        public static async Task WriteAllTextAsync(string path, string contents)
        {
            var encoding = new UTF8Encoding();
            byte[] byteContents = encoding.GetBytes(contents);

            using (var fs = File.Open(path, FileMode.Create, FileAccess.Write))
            {
                fs.Seek(0, SeekOrigin.Begin);
                await fs.WriteAsync(byteContents, 0, byteContents.Length);
            }
        }

        public static async Task AppendAllTextAsync(string path, string contents)
        {
            var encoding = new UTF8Encoding();
            byte[] byteContents = encoding.GetBytes(contents);

            using (var fs = File.Open(path, FileMode.Append, FileAccess.Write))
            {
                await fs.WriteAsync(byteContents, 0, byteContents.Length);
            }
        }
    }
}
