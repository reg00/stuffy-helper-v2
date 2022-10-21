using Microsoft.AspNetCore.Http;

namespace StuffyHelper.Core.Features.Media
{
    public class AddMediaEntry
    {
        public IFormFile File { get; set; }
        public MediaType MediaType { get; set; }
        public string? Link { get; set; }
    }
}
