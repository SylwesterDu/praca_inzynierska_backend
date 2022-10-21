using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Data.Entities;

namespace praca_inzynierska_backend.Repositories.FilesRepository
{
    public interface IFilesRepository
    {
        Task AddUploadProcess(UploadProcess process);
        Task<UploadProcess> GetUploadProcessById(Guid id);
        Task AddFile(FileData fileData);
        Task AddArtwork(Artwork artwork);
        Task SetArtworkToFiles(UploadProcess process, Guid id);
    }
}