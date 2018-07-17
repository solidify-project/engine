using System.Collections.Generic;
using System.Dynamic;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Models;

namespace SolidifyProject.Engine.Test.Unit.Infrastructure.Models
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
                Custom.image:            logo.png
                Custom.description:      this is description
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
        
        [Test]
        public void ParseModelSimpleTest()
        {
            ICollection<KeyValuePair<string, object>> data = new ExpandoObject();
            data.Add(new KeyValuePair<string, object>("goods", new List<string>
            {
                "apple",
                "peach",
                "mango"
            }));
            data.Add(new KeyValuePair<string, object>("banner", new
            {
                url = "http://mydomain.com/banner.png"
            }));
            data.Add(new KeyValuePair<string, object>("other", new { }));
            
            var actualModel = new PageModel { ContentRaw = @"
                Model:            Data
                ---
            "};

            actualModel.Parse();
            actualModel.MapDataToModel((ExpandoObject)data);
            
            Assert.AreEqual(((dynamic)data).goods.Count, actualModel.Model.goods.Count);
            Assert.AreEqual(((dynamic)data).goods[0], actualModel.Model.goods[0]);
            Assert.AreEqual(((dynamic)data).goods[1], actualModel.Model.goods[1]);
            Assert.AreEqual(((dynamic)data).goods[2], actualModel.Model.goods[2]);
            
            Assert.AreEqual(((dynamic)data).banner.url, actualModel.Model.banner.url);
        }
        
        [Test]
        public void ParseModelSingleItemTest()
        {
            ICollection<KeyValuePair<string, object>> data = new ExpandoObject();
            data.Add(new KeyValuePair<string, object>("goods", new List<string>
            {
                "apple",
                "peach",
                "mango"
            }));
            data.Add(new KeyValuePair<string, object>("banner", new
            {
                url = "http://mydomain.com/banner.png"
            }));
            data.Add(new KeyValuePair<string, object>("other", new { }));
            
            var actualModel = new PageModel { ContentRaw = @"
                Model.goods:            Data.goods
                ---
            "};

            actualModel.Parse();
            actualModel.MapDataToModel((ExpandoObject)data);
            
            Assert.AreEqual(((dynamic)data).goods.Count, actualModel.Model.goods.Count);
            Assert.AreEqual(((dynamic)data).goods[0], actualModel.Model.goods[0]);
            Assert.AreEqual(((dynamic)data).goods[1], actualModel.Model.goods[1]);
            Assert.AreEqual(((dynamic)data).goods[2], actualModel.Model.goods[2]);
        }
        
        [Test]
        public void ParseModelMultipleItemsTest()
        {
            ICollection<KeyValuePair<string, object>> data = new ExpandoObject();
            data.Add(new KeyValuePair<string, object>("goods", new List<string>
            {
                "apple",
                "peach",
                "mango"
            }));
            data.Add(new KeyValuePair<string, object>("banner", new
            {
                url = "http://mydomain.com/banner.png"
            }));
            data.Add(new KeyValuePair<string, object>("other", new { }));
            
            var actualModel = new PageModel { ContentRaw = @"
                Model.goods:            Data.goods
                Model.bannerUrl:        Data.banner.url
                ---
            "};

            actualModel.Parse();
            actualModel.MapDataToModel((ExpandoObject)data);
            
            Assert.AreEqual(((dynamic)data).goods.Count, actualModel.Model.goods.Count);
            Assert.AreEqual(((dynamic)data).goods[0], actualModel.Model.goods[0]);
            Assert.AreEqual(((dynamic)data).goods[1], actualModel.Model.goods[1]);
            Assert.AreEqual(((dynamic)data).goods[2], actualModel.Model.goods[2]);
            Assert.AreEqual(((dynamic)data).banner.url, actualModel.Model.bannerUrl);
        }
        
        [Test]
        public void ParseModelComplexMultipleItemsTest()
        {
            ICollection<KeyValuePair<string, object>> data = new ExpandoObject();
            data.Add(new KeyValuePair<string, object>("goods", new List<string>
            {
                "apple",
                "peach",
                "mango"
            }));
            data.Add(new KeyValuePair<string, object>("banner", new
            {
                url = "http://mydomain.com/banner.png"
            }));
            data.Add(new KeyValuePair<string, object>("other", new { }));
            
            var actualModel = new PageModel { ContentRaw = @"
                Model.category.goods:            Data.goods
                Model.category.bannerUrl:        Data.banner.url
                ---
            "};

            actualModel.Parse();
            actualModel.MapDataToModel((ExpandoObject)data);
            
            Assert.AreEqual(((dynamic)data).goods.Count, actualModel.Model.category.goods.Count);
            Assert.AreEqual(((dynamic)data).goods[0], actualModel.Model.category.goods[0]);
            Assert.AreEqual(((dynamic)data).goods[1], actualModel.Model.category.goods[1]);
            Assert.AreEqual(((dynamic)data).goods[2], actualModel.Model.category.goods[2]);
            Assert.AreEqual(((dynamic)data).banner.url, actualModel.Model.bannerUrl);
        }
    }
}