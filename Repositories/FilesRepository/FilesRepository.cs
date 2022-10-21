using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using praca_inzynierska_backend.Data;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_backend.Misc;

namespace praca_inzynierska_backend.Repositories.FilesRepository
{
    public class FilesRepository : IFilesRepository
    {
        private AppDbContext _context;

        public FilesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddArtwork(Artwork artwork)
        {
            await _context.Artworks!.AddAsync(artwork);
            await _context.SaveChangesAsync();
        }

        public async Task AddFile(FileData fileData)
        {
            await _context!.FilesData!.AddAsync(fileData);
            await _context.SaveChangesAsync();
        }

        public async Task AddUploadProcess(UploadProcess process)
        {
            await _context.UploadProcesses!.AddAsync(process);
            await _context.SaveChangesAsync();
        }

        public async Task<UploadProcess> GetUploadProcessById(Guid id)
        {
            UploadProcess? process = await _context!
                .UploadProcesses!
                .Include(process => process.FilesData)
                .FirstOrDefaultAsync(process => process.Id == id);
            return process!;
        }

        public async Task SetArtworkToFiles(UploadProcess process, Guid id)
        {
            Artwork? artwork = await _context.Artworks!
                .FirstOrDefaultAsync(artwork => artwork.Id == id);
            foreach (FileData fileData in process.FilesData!)
            {
                fileData.Artwork = artwork;
            }

            await _context.SaveChangesAsync();
        }
    }
}