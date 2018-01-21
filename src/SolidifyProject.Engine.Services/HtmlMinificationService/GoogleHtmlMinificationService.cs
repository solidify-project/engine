using System;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using ZetaProducerHtmlCompressor.Internal;

namespace SolidifyProject.Engine.Services.HtmlMinificationService
{
    public class GoogleHtmlMinificationService : IHtmlMinificationService
    {
        private readonly HtmlCompressor _compressor;

        public GoogleHtmlMinificationService()
        {
            _compressor = new HtmlCompressor();
            
            _compressor.setEnabled(true);
            _compressor.setRemoveComments(true);
            _compressor.setRemoveMultiSpaces(true);
            _compressor.setRemoveIntertagSpaces(true);
        }
        
        public Task<string> CompressHtmlAsync(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            var result = _compressor.compress(html);

            return Task.FromResult(result);
        }
    }
}