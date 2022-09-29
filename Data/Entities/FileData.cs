using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_praca_inzynierska_backend.Data.Entities;

namespace praca_inzynierska_backend.Data.Entities
{
    public class FileData
    {
        public Guid Id { get; set; }
        public UploadProcess? UploadProcess { get; set; }
        public string? FileName { get; set; }
        public string? Path { get; set; }
        public Guid? ArtworkId { get; set; }
    }
}