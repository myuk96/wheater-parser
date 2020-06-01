using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CrawlingSystem.Crawler.Infrastructure.Downloading.Models;

namespace CrawlingSystem.Crawler.Infrastructure.Downloading
{
    public class HttpDownloadService : IDownloadService
    {
        public async Task<Response> DownloadAsync(Request request)
        {
            var response = await new HttpClient().GetAsync(request.Uri).ConfigureAwait(false);
            return new Response(content: await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false),
                Encoding.UTF8,
                request.Uri); 
        }
    }
}
