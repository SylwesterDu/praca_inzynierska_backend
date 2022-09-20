using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Data.DTOs;
using backend.Data.Entities;
using backend.Misc;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.FilesRepository
{
    public class FilesRepository : IFilesRepository
    {
        private AppDbContext _context;

        public FilesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UploadSong(User uploader, UploadSongRequestDTO uploadSongRequestDTO)
        {
            Artwork? artWork = await _context!.Artworks!
                .FirstOrDefaultAsync(artWork => artWork.Title == uploadSongRequestDTO.Title);

            if (artWork is not null)
            {
                return false;
            }


            User? user = await _context.Users!.FirstOrDefaultAsync(user => user.Id == uploader.Id);
            if (user is null)
            {
                return false;
            }


            user.Artworks!.Add(new Artwork()
            {
                ArtType = ArtType.MUSIC,
                Owner = user,
                Id = new Guid(),
                Tags = uploadSongRequestDTO!.Tags!.Select(tag => new Tag(tag)).ToList(),
                Title = uploadSongRequestDTO.Title,
                Views = 0,
            });

            await _context.SaveChangesAsync();

            return true;
        }
    }
}