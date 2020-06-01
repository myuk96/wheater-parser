using System.Threading.Tasks;
using CrawlingSystem.Crawler.Infrastructure.Downloading.Models;

namespace CrawlingSystem.Crawler.Infrastructure.Downloading
{
    public interface IDownloadService
    {
        Task<Response> DownloadAsync(Request request);
    }
}
