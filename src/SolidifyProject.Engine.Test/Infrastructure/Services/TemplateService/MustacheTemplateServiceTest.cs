using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Infrastructure.Models.Base;
using SolidifyProject.Engine.Services.TemplateService;
using SolidifyProject.Engine.Test._Fake.Infrastructure.Services;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.TemplateService
{
    public class MustacheTemplateServiceTest : _TemplateServiceTestBase
    {
        private readonly IContentReaderService<TextContentModel> _partialsLocator = new ContentReaderServiceFake<TextContentModel, string>();
        protected override ITemplateService TemplateService => new MustacheTemplateService(_partialsLocator);

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            ((ContentReaderServiceFake<TextContentModel, string>)_partialsLocator)
                .Storage.Add(new CustomDataModel {Id = "pre", ContentRaw = "Hello"});
        }

        private static object[] _renderTemplateTestCases =
        {
            new object[]
            {
                "<h1>Hello {{ Page.Title }}!</h1>",
                new PageModel { Title = "home" },
                "<h1>Hello home!</h1>"
            }
        };
        
        [TestCaseSource(nameof(_renderTemplateTestCases))]
        public override async Task RenderTemplateTest(string template, PageModel pageModel, string html)
        {
            await base.RenderTemplateTest(template, pageModel, html);
        }

        private static ExpandoObject _nestedData
        {
            get
            {
                var objSubLevel = new ExpandoObject();
                ((ICollection<KeyValuePair<string, object>>) objSubLevel)
                    .Add(new KeyValuePair<string, object>("SubLevel", new { Title = "home" }));

                
                var objLevel = new ExpandoObject();
                ((ICollection<KeyValuePair<string, object>>) objLevel)
                    .Add(new KeyValuePair<string, object>("Level", objSubLevel));
                
                return objLevel;
            }
        }

        private static object[] _renderTemplateWithNestetDataTestCases =
        {
            new object[]
            {
                "<h1>Hello {{ Data.Level.SubLevel.Title }}!</h1>",
                new PageModel(),
                _nestedData, 
                "<h1>Hello home!</h1>"
            }
        };
        
        [TestCaseSource(nameof(_renderTemplateWithNestetDataTestCases))]
        public override Task RenderTemplateWithNestetDataTest(string template, PageModel pageModel, ExpandoObject dataModel, string html)
        {
            return base.RenderTemplateWithNestetDataTest(template, pageModel, dataModel, html);
        }


        private static object[] _renderTemplateWithLayoutestCases =
        {
            new object[]
            {
                "{{> pre}} {{ Page.Title }}!",
                new PageModel { Title = "world" },
                "Hello world!"
            } 
        };
        
        [TestCaseSource(nameof(_renderTemplateWithLayoutestCases))]
        public override Task RenderTemplateWithLayoutTest(string template, PageModel pageModel, string html)
        {
            return base.RenderTemplateWithLayoutTest(template, pageModel, html);
        }
    }
}