#nullable enable
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json.Linq;
using OpenResume.API.Util;

namespace OpenResume.API.Tests.Unit
{
    public class ResumeUploadRequestValidatorTests
    {
        [Fact]
        public async Task IsValidFailsOnEmptyString() {
            var mockRequest = new Mock<HttpRequest>();

            Stream mockBodyStream = mockBodyStream = new MemoryStream();
            StreamWriter sw = new StreamWriter(mockBodyStream);
            sw.Write(string.Empty);
            sw.Flush();

            mockRequest.Setup(req => req.Body).Returns(mockBodyStream);

            ResumeUploadRequestValidator validator = new ResumeUploadRequestValidator();
            bool success = await validator.IsValid(mockRequest.Object);

            sw.Close();

            Assert.False(success);
        }

        [Fact]
        public async Task IsValidSucceedsOnSimpleJsonBody() {
            var mockRequest = new Mock<HttpRequest>();

            Stream mockBodyStream = mockBodyStream = new MemoryStream();
            StreamWriter sw = new StreamWriter(mockBodyStream);
            sw.Write("{\"key\":\"value\"}");
            sw.Flush();
            mockBodyStream.Position = 0;

            mockRequest.Setup(req => req.Body).Returns(mockBodyStream);

            ResumeUploadRequestValidator validator = new ResumeUploadRequestValidator();
            bool success = await validator.IsValid(mockRequest.Object);

            sw.Close();

            Assert.True(success);
        }

        [Fact]
        public async Task TryParseSucceedsOnSimpleJsonBody() {
            var mockRequest = new Mock<HttpRequest>();

            Stream mockBodyStream = mockBodyStream = new MemoryStream();
            StreamWriter sw = new StreamWriter(mockBodyStream);
            sw.Write("{\"key\":\"value\"}");
            sw.Flush();
            mockBodyStream.Position = 0;

            mockRequest.Setup(req => req.Body).Returns(mockBodyStream);

            ResumeUploadRequestValidator validator = new ResumeUploadRequestValidator();
            var (success, result, exception) = await validator.TryParse(mockRequest.Object);

            string resultValue = result["key"].ToString();
            
            sw.Close();

            Assert.True(resultValue == "value");
        }
    }
}
#nullable disable