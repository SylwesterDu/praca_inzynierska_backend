using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_praca_inzynierska_backend.Data.Entities;

namespace praca_inzynierska_backend.Repositories.FilesRepository
{
    public interface IFilesRepository
    {
        Task addUploadProcess(UploadProcess process);
        Task<UploadProcess> getUploadProcessById(Guid id);
        Task addFile(FileData fileData);
        Task AddArtwork(Artwork artwork);
        Task setArtworkIdToFiles(UploadProcess process, Guid id);
    }
}