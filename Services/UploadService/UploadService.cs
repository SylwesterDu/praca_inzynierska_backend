using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.DTOs;
using backend.Data.Entities;
using backend.Repositories.FilesRepository;
using backend.Services.AccountService;

namespace backend.Services.UploadService
{
    public class UploadService : IUploadService
    {
        private readonly IAccountService _accountService;
        private readonly IFilesRepository _filesRepository;

        public UploadService(IAccountService accountService, IFilesRepository filesRepository)
        {
            _accountService = accountService;
            _filesRepository = filesRepository;
        }


        public async Task<bool> UploadSong(string token, UploadSongRequestDTO uploadSongRequestDTO)
        {
            User user = await _accountService.getUserByToken(token);
            bool success = await _filesRepository.UploadSong(user, uploadSongRequestDTO);

            return success;
        }
    }
}