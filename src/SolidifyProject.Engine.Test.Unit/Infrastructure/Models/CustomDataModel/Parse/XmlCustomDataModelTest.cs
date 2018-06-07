using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Enums;

namespace SolidifyProject.Engine.Test.Unit.Infrastructure.Models.CustomDataModel.Parse
{
   [TestFixture]
   public class XmlCustomDataModelTest
   {
      private string _xml = @"<root>
                              <links>
                                 <name>facebook</name>
                                 <url>https://facebook.com</url>
                              </links>
                              <links>
                                  <name>twitter</name>
                                  <url>https://twitter.com</url>
                              </links>
                           </root>";
      
      [Test]
      public void ParseXml()
      {
         var model = new Engine.Infrastructure.Models.CustomDataModel();
         model.Id = "file.xml";
         model.ContentRaw = _xml;
            
         model.Parse();
            
         Assert.NotNull(model.DataType);
         Assert.AreEqual(CustomDataType.Xml.ToString(), model.DataType.ToString());
            
         var data = (dynamic)model.CustomData;
         Assert.IsNotNull(data);
         Assert.AreEqual(2, data.root.links.Count);
            
         Assert.AreEqual("facebook", (string)data.root.links[0].name);
         Assert.AreEqual("https://facebook.com", (string)data.root.links[0].url);
            
         Assert.AreEqual("twitter", (string)data.root.links[1].name);
         Assert.AreEqual("https://twitter.com", (string)data.root.links[1].url);
      }
   }
}