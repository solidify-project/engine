using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Xml.Linq;
using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolidifyProject.Engine.Infrastructure.Enums;
using SolidifyProject.Engine.Infrastructure.Models.Base;
using YamlDotNet.Serialization;

namespace SolidifyProject.Engine.Infrastructure.Models
{
    public class CustomDataModel : TextContentModel
    {
        private string _id;
        public override string Id
        {
            get => _id;
            set
            {
                _id = value;
                _sanitezedId = null;
            }
        }

        private string _sanitezedId;
        public string SanitezedId
        {
            get
            {
                if (string.IsNullOrEmpty(_sanitezedId))
                {
                    
                    _sanitezedId = Id
                        .Replace('/', '.')
                        .Replace('\\', '.');

                    if (Path.HasExtension(Id))
                    {
                        var extension = Path.GetExtension(Id);
                        _sanitezedId = _sanitezedId
                            .Replace(extension, string.Empty);
                    }
                }
                return _sanitezedId;
            }
        }

        public CustomDataType? DataType { get; protected set; }

        public object CustomData { get; set; }

        public override void Parse()
        {
            var extension = Path.GetExtension(Id).Trim('.');
            
            if (extension.Equals(CustomDataType.Json.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                DataType = CustomDataType.Json;
                ParseJson();
                return;
            }
            
            if (extension.Equals(CustomDataType.Xml.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                DataType = CustomDataType.Xml;
                ParseXml();
                return;
            }
            
            if (extension.Equals(CustomDataType.Yaml.ToString(), StringComparison.OrdinalIgnoreCase) ||
                extension.Equals(CustomDataType.Yml.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                DataType = CustomDataType.Yaml;
                ParseYaml();
                return;
            }
            
            if (extension.Equals(CustomDataType.Csv.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                DataType = CustomDataType.Csv;
                ParseCsv();
                return;
            }
            
            if (extension.Equals(CustomDataType.Txt.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                DataType = CustomDataType.Txt;
                ParseTxt();
                return;
            }
            
            throw new NotSupportedException($"Unknown custom data type \"{extension}\"");
        }

        private void ParseJson()
        {
            CustomData = JObject.Parse(ContentRaw);
        }
        
        private void ParseXml()
        {
            XDocument doc = XDocument.Parse(ContentRaw);
            string jsonText = JsonConvert.SerializeXNode(doc);
            CustomData = JObject.Parse(jsonText);
        }
        
        private void ParseYaml()
        {
            var deserializer = new DeserializerBuilder()
                .Build();

            object obj;
            using (var reader = new StringReader(ContentRaw))
            {
                obj = deserializer.Deserialize(reader);
            }

            CustomData = ParseYaml(obj);
        }

        private object ParseYaml(object node)
        {
            if (node is ICollection<KeyValuePair<object, object>> nestedObjects)
            {
                var result = new ExpandoObject();
                
                foreach (var property in nestedObjects)
                {
                    ((ICollection<KeyValuePair<string, object>>)result)
                        .Add(new KeyValuePair<string, object>(property.Key.ToString(), ParseYaml(property.Value)));
                }

                return result;
            }

            if (node is ICollection<object> properties)
            {
                var result = new List<object>();

                foreach (var property in properties)
                {
                    result.Add(ParseYaml(property));
                }

                return result;
            }

            return node;
        }

        private void ParseCsv()
        {
            CustomData = new List<dynamic>();
            
            using (var reader = new StringReader(ContentRaw))
            using (var csv = new CsvReader(reader))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    ICollection<KeyValuePair<string, object>> row = csv.GetRecord<dynamic>();
                    ICollection<KeyValuePair<string, object>> data = new ExpandoObject();

                    foreach (var cell in row)
                    {
                        var property = new KeyValuePair<string, object>(cell.Key.Trim(), cell.Value.ToString().Trim());
                        data.Add(property);
                    }
                
                    ((List<dynamic>)CustomData).Add(data);
                }
            }
        }
        
        private void ParseTxt()
        {
            CustomData = ContentRaw?.Trim();
        }
    }
}