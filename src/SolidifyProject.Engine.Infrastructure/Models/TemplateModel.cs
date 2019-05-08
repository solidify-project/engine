using System;
using System.IO;
using SolidifyProject.Engine.Infrastructure.Enums;
using SolidifyProject.Engine.Infrastructure.Models.Base;

namespace SolidifyProject.Engine.Infrastructure.Models
{
    public sealed class TemplateModel : TextContentModel
    {
        public string Template { get; private set; }

//        public TemplateType TemplateType { get; set; }
        
        public override void Parse()
        {
//            var extension = Path.GetExtension(Id).Trim('.');
//
//            if (extension.Equals("hjs", StringComparison.OrdinalIgnoreCase))
//            {
//                TemplateType = TemplateType.Mustache;
//            }

            Template = ContentRaw;
        }
    }
}