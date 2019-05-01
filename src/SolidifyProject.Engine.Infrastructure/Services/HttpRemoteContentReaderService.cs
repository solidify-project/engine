using System;
using System.Net.Http;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models.RemoteContentModel;

namespace SolidifyProject.Engine.Infrastructure.Services
{
    public class HttpRemoteContentReaderService : IRemoteContentReaderService<HttpRemoteContentModel>
    {
        public async Task<string> LoadContentAsync(HttpRemoteContentModel parameters)
        {
            HttpResponseMessage response;
            
            using (var client = new HttpClient())
            switch (parameters.Method.ToLowerInvariant())
            {
                case "get":
                    response = await client.GetAsync(parameters.Url);
                    break;
                case "post":
                    response = await client.PostAsync(parameters.Url, null);
                    break;
                default:
                    throw new NotSupportedException($"Http method {parameters.Method} is not supported.");
            }
            
            return await response.Content.ReadAsStringAsync();
        }
    }
}