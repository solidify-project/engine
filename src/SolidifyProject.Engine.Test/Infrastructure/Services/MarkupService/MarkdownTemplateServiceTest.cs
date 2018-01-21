using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Services.MarkupService;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.MarkupService
{
    public class MarkdownTemplateServiceTest : _MarkupServiceTestBase
    {
        private const string MarkdigNewLine = "\n"; 
        
        protected override IMarkupService MarkupService => new MarkdownMarkupService();

        private static object[] _renderTemplateTestCases =
        {
            new object[]
            {
                "# Hello **world**!" + Environment.NewLine + "## test",
                "<h1>Hello <strong>world</strong>!</h1>" + MarkdigNewLine  + "<h2>test</h2>" + MarkdigNewLine
            },
            new object[]
            {
                "| a | b |  " + Environment.NewLine + "|---|---|  " + Environment.NewLine + "| 0 | 1 |  " + Environment.NewLine,
                "<table>" + MarkdigNewLine + "<thead>" + MarkdigNewLine + 
                "<tr>" + MarkdigNewLine + "<th>a</th>" + MarkdigNewLine + "<th>b</th>" + MarkdigNewLine + "</tr>" + MarkdigNewLine +
                "</thead>" + MarkdigNewLine + "<tbody>" + MarkdigNewLine +
                "<tr>" + MarkdigNewLine + "<td>0</td>" + MarkdigNewLine + "<td>1</td>" + MarkdigNewLine + "</tr>" + MarkdigNewLine +
                "</tbody>" + MarkdigNewLine + "</table>" + MarkdigNewLine
            }
        };
        
        [TestCaseSource(nameof(_renderTemplateTestCases))]
        public override async Task RenderMarkupTest(string template, string html)
        {
            await base.RenderMarkupTest(template, html);
        }
    }
}