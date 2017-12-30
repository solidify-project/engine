using System;
using System.Collections.Generic;
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
        private static readonly string[] TEMPLATE_TYPE = {"TemplateType"};
        private static readonly string[] TEMPLATE_ID_ATTRIBUTE = {"TemplateId", "Template", "LayoutÏd", "Layout"};
        
        public string Title { get; set; }
        public string Url { get; set; }

        /// <summary>
        /// </summary>
        public TemplateType? TemplateType { get; set; }

        /// <summary>
        /// Template unique identifier
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// Raw content after separator
        /// </summary>
        public string Content { get; set; }

        public override void Parse()
        {
            var lines = ContentRaw.Split(END_OF_LINE, StringSplitOptions.None);

            var attributeLines = lines
                .TakeWhile(x => !SEPARATOR.Equals(x))
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x));
            foreach (var attributeLine in attributeLines)
            {
                ParseAttributeLine(attributeLine);
            }
            
            var contentLines = lines.SkipWhile(x => !SEPARATOR.Equals(x)).Skip(1);
            ParseContent(contentLines);
            
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
            
            if (TEMPLATE_TYPE.Any(x => x.Equals(attributeName, StringComparison.OrdinalIgnoreCase)))
            {
                TemplateType type;
                if (Enum.TryParse(attributeValue, true, out type))
                {
                    TemplateType = type;
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Unknown value \"{attributeValue}\" for TemplateType enumerable at line \"{line}\"");
                }

                return;
            }
            
            if (TEMPLATE_ID_ATTRIBUTE.Any(x => x.Equals(attributeName, StringComparison.OrdinalIgnoreCase)))
            {
                TemplateId = attributeValue;
                return;
            }
            
            throw new ArgumentException($"Unknown attribute \"{attributeName}\" at line \"{line}\"");
        }

        private void ParseContent(IEnumerable<string> lines)
        {
            Content = string.Join("\r\n", lines);
        }
    }
}