using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace GaraioLogParser.Tests.Controllers
{
    [TestClass]
    public class IISLogParserTest
    {
        [TestMethod]
        public void ParserTest()
        {
            // Arrange
            var file = new FileInfo("Logs/IISLog.log");
            IISLogParser parser = new IISLogParser(file.FullName);

            // Act
            var results = parser.ParseW3CLog();

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);
        }
    }
}
