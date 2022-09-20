using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.DTOs;

namespace backend.Services.UploadService
{
    public interface IUploadService
    {
        public Task<bool> UploadSong(string token, UploadSongRequestDTO uploadSongRequestDTO);
    }
}