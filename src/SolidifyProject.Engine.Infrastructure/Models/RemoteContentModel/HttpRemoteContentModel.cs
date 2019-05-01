namespace SolidifyProject.Engine.Infrastructure.Models.RemoteContentModel
{
    public sealed class HttpRemoteContentModel
    {
        public string Url { get; set; }
        public string Method { get; set; }

        public string CustomDataType { get; set; }
    }
}