using SolidifyProject.Engine.Infrastructure.Models.Base;

namespace SolidifyProject.Engine.Infrastructure.Models
{
    public sealed class FeedModel : TextContentModel
    {
        public string Name { get; set; }
        public int PageSize { get; set; }
        public string Source { get; set; }

        public override void Parse()
        {
            base.Parse();
        }
    }
}