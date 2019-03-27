using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageAnalyzer.Core.Services;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Tests
{
    [TestClass]
    public class TestLicenseMapper
    {
        [TestMethod]
        public async Task LicenseMapper_get_existing()
        {
            var licenseMapper = new LicenseMapper(new FileUtilities());
            var licenseType = await licenseMapper.GetLicenseType("https://opensource.org/licenses/mit-license.php");
            Assert.AreEqual("MIT", licenseType);
        }

        [TestMethod]
        public async Task LicenseMapper_get_non_existing_no_fallback()
        {
            var licenseMapper = new LicenseMapper(new FileUtilities());
            var licenseType = await licenseMapper.GetLicenseType("juba");
            Assert.AreEqual(string.Empty, licenseType);
        }

        [TestMethod]
        public async Task LicenseMapper_get_non_existing_with_fallback()
        {
            var licenseMapper = new LicenseMapper(new FileUtilities());
            var licenseType = await licenseMapper.GetLicenseType("juba", "fallback");
            Assert.AreEqual("fallback", licenseType);
        }
    }
}
