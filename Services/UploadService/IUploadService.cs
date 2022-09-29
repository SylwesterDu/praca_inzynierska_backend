using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_praca_inzynierska_backend.Data.Entities;

namespace praca_inzynierska_backend.Services.UploadService
{
    public interface IUploadService
    {
        Task<UploadProcessDTO> CreateUploadProcess(string token);
        Task<bool> UploadFile(string token, IFormFile formFile, Guid id);
        Task<bool> PublishArtWork(string token, Guid id, PublishArtworkRequestDTO publishArtworkRequestDTO);
    }
}