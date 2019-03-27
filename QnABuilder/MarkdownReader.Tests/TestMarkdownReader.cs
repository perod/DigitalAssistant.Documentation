using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MarkdownReader.Tests
{
    [TestClass]
    public class TestMarkdownReader
    {
        [TestMethod]
        public async Task MarkdownReader_readmarkdownfiles()
        {
            var reader = new MarkdownReader();
            await reader.ReadMarkdownFiles(@"C:\VS2015\pp-git\DigitalAssistant.Documentation\Helpcenter\docs");
        }


        
    }
}
