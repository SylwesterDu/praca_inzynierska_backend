using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data;
using praca_inzynierska_backend.Data.DTOs;
using Microsoft.EntityFrameworkCore;
using praca_inzynierska_praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_praca_inzynierska_backend.Misc;
using praca_inzynierska_backend.Data.Entities;

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

        public async Task SetArtworkIdToFiles(UploadProcess process, Guid id)
        {
            foreach (FileData fileData in process.FilesData!)
            {
                fileData.ArtworkId = id;
            }

            await _context.SaveChangesAsync();
        }
    }
}