using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.DTOs;
using backend.Data.Entities;

namespace backend.Repositories.FilesRepository
{
    public interface IFilesRepository
    {
        public Task<bool> UploadSong(User user, UploadSongRequestDTO uploadSongRequestDTO);
    }
}