using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Enums;

namespace SolidifyProject.Engine.Test.Unit.Infrastructure.Models.CustomDataModel.Parse
{
   [TestFixture]
   public class XmlCustomDataModelTest
   {
       private static IEnumerable<(string, Func<dynamic, int, string>, Func<dynamic, int, string>)> _xmlSources
       {
           get
           {
               yield return (
                   @"<root>
                   <links>
                      <name>facebook</name>
                      <url>https://facebook.com</url>
                   </links>
                   <links>
                       <name>twitter</name>
                       <url>https://twitter.com</url>
                   </links>
               </root>",
                   (data, indx) => data.root.links[indx].name,
                   (data, indx) => data.root.links[indx].url
               );
               yield return (
                   @"<root>
                    <links name=""facebook"" url=""https://facebook.com"" />
                    <links name=""twitter"" url=""https://twitter.com"" />
               </root>",
                    (data, indx) => data.root.links[indx]["@name"],
                    (data, indx) => data.root.links[indx]["@url"]
                   );
               yield return (
                   @"<root>
                   <links name=""facebook"">
                       <url>https://facebook.com</url>
                   </links>
                   <links name=""twitter"">
                       <url>https://twitter.com</url>
                   </links>
               </root>",
                   (data, indx) => data.root.links[indx]["@name"],
                   (data, indx) => data.root.links[indx].url
               );
               yield return (
                   @"<root>
                   <links name=""facebook"">https://facebook.com</links>
                   <links name=""twitter"">https://twitter.com</links>
               </root>",
                    (data, indx) => data.root.links[indx]["@name"],
                    (data, indx) => data.root.links[indx]["#text"]
               );

           }

       }
      
      
      
      [Test]
      [TestCaseSource(nameof(_xmlSources))]
      public void ParseXml((string xml, Func<dynamic, int, string> nameGetter, Func<dynamic, int, string> urlGetter) tuple)
      {
         var model = new Engine.Infrastructure.Models.CustomDataModel();
         model.Id = "file.xml";
         model.ContentRaw = tuple.xml;
            
         model.Parse();
            
         Assert.NotNull(model.DataType);
         Assert.AreEqual(CustomDataType.Xml.ToString(), model.DataType.ToString());
            
         var data = (dynamic)model.CustomData;
         Assert.IsNotNull(data);
         Assert.AreEqual(2, data.root.links.Count);
            
         Assert.AreEqual("facebook", tuple.nameGetter(data, 0));
         Assert.AreEqual("https://facebook.com", tuple.urlGetter(data, 0));
            
         Assert.AreEqual("twitter", tuple.nameGetter(data, 1));
         Assert.AreEqual("https://twitter.com", tuple.urlGetter(data, 1));
      }
      
   }
}