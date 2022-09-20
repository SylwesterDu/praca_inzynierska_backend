using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Data.DTOs
{
    public class UploadSongRequestDTO
    {
        public string? Title { get; init; }
        public bool WithClip { get; init; }
        public List<string>? Tags { get; init; }
    }
}