using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Services.CloudflareFileService
{
    public interface ICloudflareFileService
    {
        public string GetFileUrl(string key);
        Task<string> UploadAvatar(Guid id, IFormFile file);
        Task<string> UploadFile(Guid artworkId, IFormFile file);
    }
}
