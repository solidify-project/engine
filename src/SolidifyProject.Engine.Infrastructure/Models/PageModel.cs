using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using SolidifyProject.Engine.Infrastructure.Enums;
using SolidifyProject.Engine.Infrastructure.Models.Base;

namespace SolidifyProject.Engine.Infrastructure.Models
{
    public sealed class PageModel : TextContentModel
    {
        public static readonly string SEPARATOR = "---";
        public static readonly string[] END_OF_LINE = {"\r", "\n", "\r\n"};
        public static readonly string[] ATTRIBUTE_SEPARATORS = {":"};

        private static readonly string[] TITLE_ATTRIBUTE = {"Title"};
        private static readonly string[] URL_ATTRIBUTE = {"Url"};
//        private static readonly string[] TEMPLATE_TYPE_ATTRIBUTE = {"TemplateType"};
        private static readonly string[] TEMPLATE_ID_ATTRIBUTE = {"TemplateId", "Template", "LayoutId", "Layout"};
        
        private static readonly string[] CUSTOM_ATTRIBUTE_PREFIX_SEPARATOR = {"."};
        private static readonly string[] CUSTOM_ATTRIBUTE_PREFIX = {"Custom"};

        private static readonly string[] MODEL_ATTRIBUTE_PREFIX = {"Model"};
        
        public string Title { get; set; }
        public string Url { get; set; }

        public dynamic Custom { get; set; }
        public dynamic Model { get; set; }

//        /// <summary>
//        /// </summary>
//        public TemplateType? TemplateType { get; set; }

        /// <summary>
        /// Template unique identifier
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// Raw content after separator
        /// </summary>
        public string Content { get; set; }
        

        public PageModel()
        {
            Custom = new ExpandoObject();
            Model = new ExpandoObject();
        }
        
        public override void Parse()
        {
            var lines = ContentRaw.Split(END_OF_LINE, StringSplitOptions.None);

            var attributeLines = lines
                .Select(x => x.Trim())
                .TakeWhile(x => !SEPARATOR.Equals(x))
                .Where(x => !string.IsNullOrEmpty(x));
            foreach (var attributeLine in attributeLines)
            {
                ParseAttributeLine(attributeLine);
            }
            
            var contentLines = lines.SkipWhile(x => !SEPARATOR.Equals(x)).Skip(1);
            ParseContent(contentLines);
        }

        public void MapDataToModel(ExpandoObject data)
        {
            if (Model is ExpandoObject)
            {
                MapDataToPageModel(Model, data);
            }
            else
            {
                Model = GetValueFromDataObject(Model, data);
            }
        }

        private void ParseAttributeLine(string line)
        {
            var attribute = line.Split(ATTRIBUTE_SEPARATORS, StringSplitOptions.None);

            var attributeName = attribute[0].Trim();
            var attributeValue = attribute[1].Trim();
            
            if (TITLE_ATTRIBUTE.Any(x => x.Equals(attributeName, StringComparison.OrdinalIgnoreCase)))
            {
                Title = attributeValue;
                return;
            }
            
            if (URL_ATTRIBUTE.Any(x => x.Equals(attributeName, StringComparison.OrdinalIgnoreCase)))
            {
                Url = attributeValue;
                return;
            }
            
//            if (TEMPLATE_TYPE_ATTRIBUTE.Any(x => x.Equals(attributeName, StringComparison.OrdinalIgnoreCase)))
//            {
//                TemplateType type;
//                if (Enum.TryParse(attributeValue, true, out type))
//                {
//                    TemplateType = type;
//                }
//                else
//                {
//                    throw new ArgumentOutOfRangeException($"Unknown value \"{attributeValue}\" for TemplateType enumerable at line \"{line}\"");
//                }
//
//                return;
//            }
            
            if (TEMPLATE_ID_ATTRIBUTE.Any(x => x.Equals(attributeName, StringComparison.OrdinalIgnoreCase)))
            {
                TemplateId = attributeValue;
                return;
            }

            if (MODEL_ATTRIBUTE_PREFIX.Any(x => x.Equals(attributeName, StringComparison.OrdinalIgnoreCase)))
            {
                Model = attributeValue;
                return;
            }

            if (CUSTOM_ATTRIBUTE_PREFIX_SEPARATOR.Any(x => attributeName.Contains(x)))
            {
                var customAttributeNames = attributeName.Split(CUSTOM_ATTRIBUTE_PREFIX_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                if (customAttributeNames.Length >= 2 && CUSTOM_ATTRIBUTE_PREFIX.Any(x => x.Equals(customAttributeNames[0], StringComparison.InvariantCultureIgnoreCase)))
                {
                    ParseCustomAttribute(Custom, customAttributeNames.Skip(1), attributeValue);
                }
                else
                {
                    if (customAttributeNames.Length >= 2 && MODEL_ATTRIBUTE_PREFIX.Any(x =>
                            x.Equals(customAttributeNames[0], StringComparison.InvariantCultureIgnoreCase)))
                    {
                        ParseCustomAttribute(Model, customAttributeNames.Skip(1), attributeValue);
                    }
                    else
                    {
                        throw new ArgumentException(
                            $"Unknown name format of custom attribute \"{attributeName}\" at line \"{line}\"");
                    }
                }

                return;
            }
            

            throw new ArgumentException($"Unknown attribute \"{attributeName}\" at line \"{line}\"");
        }

        private void ParseCustomAttribute(ExpandoObject obj, IEnumerable<string> attributeNames, string attributeValue)
        {
            IDictionary<string, object> node = obj;
            var currentSection = attributeNames.First();

            if (attributeNames.Count() > 1)
            {
                ExpandoObject currentNode;
                if (node.ContainsKey(currentSection))
                {
                    currentNode = node[currentSection] as ExpandoObject;
                }
                else
                {
                    currentNode = new ExpandoObject();
                    node.Add(new KeyValuePair<string, object>(currentSection, currentNode));
                }

                ParseCustomAttribute(currentNode, attributeNames.Skip(1), attributeValue);
            }
            else
            {
                node.Add(new KeyValuePair<string, object>(currentSection, attributeValue));
            }
        }

        private void ParseContent(IEnumerable<string> lines)
        {
            Content = string.Join("\r\n", lines);
        }
        
        private void MapDataToPageModel(ExpandoObject model, ExpandoObject data)
        {
            IDictionary<string, object> modelDict = model;
            Dictionary<string, object> objectToIterate = model.ToDictionary(k => k.Key, v => v.Value);
            foreach (var keyValuePair in objectToIterate)
            {
                if (keyValuePair.Value is ExpandoObject expObject)
                {
                    MapDataToPageModel(expObject, data);
                }
                else
                {
                    modelDict[keyValuePair.Key] = GetValueFromDataObject(keyValuePair.Value as string, data);
                }
            }
        }

        private object GetValueFromDataObject(string path, ExpandoObject data)
        {
            var attributeNames = path.Split('.');
            if (attributeNames.Length == 0)
            {
                return null;
            }

            if (attributeNames.First() == "Data")
            {
                if (attributeNames.Length == 1)
                {
                    return data;
                }

                attributeNames = attributeNames.Skip(1).ToArray();
            }

            object value = data;
            foreach (var attribute in attributeNames)
            {
                IDictionary<string, object> dict = value as IDictionary<string, object>;

                if (dict == null)
                {
                    dict = value.GetType().GetProperties()
                        .ToDictionary(x => x.Name, x => x.GetValue(value, null));
                }

                value = dict[attribute];
            }

            return value;
        }
    }
}