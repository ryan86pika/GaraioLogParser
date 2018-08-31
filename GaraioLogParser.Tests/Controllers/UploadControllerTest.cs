using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebGaraioLogParser.Controllers;
using WebGaraioLogParser.Models;
using WebGaraioLogParser.Utils;

namespace GaraioLogParser.Tests.Controllers
{
    [TestClass]
    public class UploadControllerTest
    {
        [TestMethod]
        public void GetDataIntoJSONTest()
        {
            // Arrange
            FileInfo file = new FileInfo("Logs/IISLog.log");
            UploadController controller = new UploadController();

            // Act
            var result = controller.GetDataIntoJSON(file.FullName) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOfType(result.Data, typeof(ExtractDataResult));
            Assert.IsFalse(string.IsNullOrEmpty((result.Data as ExtractDataResult).Message));
        }

        [TestMethod]
        public void MergeFilesUploadedTest()
        {
            // Arrange
            var fackedContext = new Mock<HttpContextBase>();

            var fackedRequest = new Mock<HttpRequestBase>();
            var fackedFiles = new Mock<HttpFileCollectionBase>();
            var fakedFile = new Mock<HttpPostedFileBase>();

            var fackedServer = new Mock<HttpServerUtilityBase>();

            var controller = new UploadController();

            var file = new FileInfo("Logs/IISLog.log");
            var fileName = file.Name + FileUtils.PART_TOKEN + "1.1";
            var fakeFileKeys = new List<string> { fileName };

            fackedServer.Setup(s => s.MapPath(It.IsAny<string>())).Returns(@"c:\TmpLogUploaderFolder");

            fakedFile.Setup(x => x.InputStream).Returns(file.OpenRead);
            fakedFile.Setup(f => f.ContentLength).Returns(8192);
            fakedFile.Setup(f => f.FileName).Returns(fileName);

            fackedFiles.Setup(x => x[fileName]).Returns(fakedFile.Object);
            fackedFiles.Setup(keys => keys.GetEnumerator()).Returns(fakeFileKeys.GetEnumerator());
            fackedFiles.Setup(x => x.Count).Returns(1);

            fackedRequest.Setup(x => x.Files).Returns(fackedFiles.Object);

            fackedContext.Setup(x => x.Request).Returns(fackedRequest.Object);
            fackedContext.Setup(x => x.Server).Returns(fackedServer.Object);
            controller.ControllerContext = new ControllerContext(fackedContext.Object, new RouteData(), controller);

            // Act
            JsonResult result = controller.MergeFilesUploadedIntoSingleFile() as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOfType(result.Data, typeof(MergeFileResult));
            Assert.IsFalse(string.IsNullOrEmpty((result.Data as MergeFileResult).BaseFileName));
        }
    }
}
