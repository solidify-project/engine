using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Services.HtmlMinificationService;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.HtmlCompressorService
{
    public class NoMinificationServiceTest : _HtmlCompressorServiceTestBase
    {
        protected override IHtmlMinificationService HtmlMinificationService => new NoMinificationService();
        
        private static object[] _compressHtmlTestCases =
        {
            new object[]
            {
                "<h1>Hello " + Environment.NewLine + "world!</h1>" + Environment.NewLine + "<!-- awesome -->",
                "<h1>Hello " + Environment.NewLine + "world!</h1>" + Environment.NewLine + "<!-- awesome -->"
            }
        };
        
        [TestCaseSource(nameof(_compressHtmlTestCases))]
        public override Task CompressHtmlTest(string uncompressedHtml, string expectedHtml)
        {
            return base.CompressHtmlTest(uncompressedHtml, expectedHtml);
        }
    }
}