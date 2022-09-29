using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.Entities;

namespace praca_inzynierska_praca_inzynierska_backend.Data.Entities
{
    public class UploadProcess
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public User? Uploader { get; set; }
        public List<FileData>? FilesData { get; set; }
    }
}