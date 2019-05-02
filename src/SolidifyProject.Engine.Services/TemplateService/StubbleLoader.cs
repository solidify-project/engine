using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models.Base;
using Stubble.Core.Interfaces;

namespace SolidifyProject.Engine.Services.TemplateService
{
    public class StubbleLoader : IStubbleLoader
    {
        private IContentReaderService<TextContentModel> _loader;

        public StubbleLoader(IContentReaderService<TextContentModel> loader)
        {
            _loader = loader;
        }
        
        public string Load(string name)
        {
            return LoadAsync(name).Result;
        }

        public async ValueTask<string> LoadAsync(string name)
        {
            var result = (await _loader.LoadContentByIdAsync(name));
            return result.ContentRaw;
        }

        public IStubbleLoader Clone()
        {
            return this.Clone();
        }
    }
}