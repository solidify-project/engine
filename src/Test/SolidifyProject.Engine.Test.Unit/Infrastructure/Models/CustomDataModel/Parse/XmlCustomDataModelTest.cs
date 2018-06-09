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
        [Test]
        public void ParseXmlOnlyWithAttributes()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.xml";
            model.ContentRaw = @"<root>
                    <links name=""facebook"" url=""https://facebook.com"" />
                    <links name=""twitter"" url=""https://twitter.com"" />
               </root>";

            model.Parse();

            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Xml.ToString(), model.DataType.ToString());

            var data = (dynamic) model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.root.links.Count);

            Assert.AreEqual("facebook", (string)data.root.links[0]["@name"]);
            Assert.AreEqual("https://facebook.com", (string)data.root.links[0]["@url"]);

            Assert.AreEqual("twitter", (string)data.root.links[1]["@name"]);
            Assert.AreEqual("https://twitter.com", (string)data.root.links[1]["@url"]);
        }

        [Test]
        public void ParseXmlOnlyWithFields()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.xml";
            model.ContentRaw = @"<root>
                   <links>
                      <name>facebook</name>
                      <url>https://facebook.com</url>
                   </links>
                   <links>
                       <name>twitter</name>
                       <url>https://twitter.com</url>
                   </links>
               </root>";

            model.Parse();

            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Xml.ToString(), model.DataType.ToString());

            var data = (dynamic) model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.root.links.Count);

            Assert.AreEqual("facebook", (string)data.root.links[0].name);
            Assert.AreEqual("https://facebook.com", (string)data.root.links[0].url);

            Assert.AreEqual("twitter", (string)data.root.links[1].name);
            Assert.AreEqual("https://twitter.com", (string)data.root.links[1].url);
        }

        [Test]
        public void ParseXmlWithFieldAndAttribute()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.xml";
            model.ContentRaw = @"<root>
                   <links name=""facebook"">
                       <url>https://facebook.com</url>
                   </links>
                   <links name=""twitter"">
                       <url>https://twitter.com</url>
                   </links>
               </root>";

            model.Parse();

            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Xml.ToString(), model.DataType.ToString());

            var data = (dynamic) model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.root.links.Count);

            Assert.AreEqual("facebook", (string)data.root.links[0]["@name"]);
            Assert.AreEqual("https://facebook.com", (string)data.root.links[0].url);

            Assert.AreEqual("twitter", (string)data.root.links[1]["@name"]);
            Assert.AreEqual("https://twitter.com", (string)data.root.links[1].url);
        }

        [Test]
        public void ParseXmlWithAttributeAndValue()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.xml";
            model.ContentRaw = @"<root>
                   <links name=""facebook"">https://facebook.com</links>
                   <links name=""twitter"">https://twitter.com</links>
               </root>";

            model.Parse();

            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Xml.ToString(), model.DataType.ToString());

            var data = (dynamic) model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.root.links.Count);

            Assert.AreEqual("facebook", (string)data.root.links[0]["@name"]);
            Assert.AreEqual("https://facebook.com", (string)data.root.links[0]["#text"]);

            Assert.AreEqual("twitter", (string)data.root.links[1]["@name"]);
            Assert.AreEqual("https://twitter.com", (string)data.root.links[1]["#text"]);
        }

    }
}