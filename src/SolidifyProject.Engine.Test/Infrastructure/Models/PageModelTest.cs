using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Models;

namespace SolidifyProject.Engine.Test.Infrastructure.Models
{
    [TestFixture]
    public class PageModelTest
    {
        [Test]
        public void ParseTest()
        {
            var expectedModel = new PageModel
            {
                Title = "test title",
                Url = "some url",
                TemplateId = "my template"
            };
            
            var actualModel = new PageModel { ContentRaw = @"
                title:        test title
                url:          some url
                template:     my template
                ---
            "};
            
            actualModel.Parse();
            
            Assert.AreEqual(expectedModel.Title, actualModel.Title);
            Assert.AreEqual(expectedModel.Url, actualModel.Url);
            Assert.AreEqual(expectedModel.TemplateId, actualModel.TemplateId);
        }
        
        
        [Test]
        public void ParseCustomAttributesTest()
        {
            var expectedModel = new PageModel
            {
                Custom = new
                {
                    image = "logo.png",
                    description = "this is description",
                    root = new
                    {
                        SomeThing = "bla-bla-bla"
                    },
                    l1 = new { l2 = new { l3 = new { l4 = "hello" } } }
                }
            };
            
            var actualModel = new PageModel { ContentRaw = @"
                custom.image:            logo.png
                custom.description:      this is description
                custom.root.SomeThing:   bla-bla-bla
                custom.l1.l2.l3.l4:      hello
                ---
            "};
            
            actualModel.Parse();
            
            Assert.AreEqual(expectedModel.Custom.image, actualModel.Custom.image);
            Assert.AreEqual(expectedModel.Custom.description, actualModel.Custom.description);
            Assert.AreEqual(expectedModel.Custom.root.SomeThing, actualModel.Custom.root.SomeThing);
            Assert.AreEqual(expectedModel.Custom.l1.l2.l3.l4, actualModel.Custom.l1.l2.l3.l4);
        }
    }
}