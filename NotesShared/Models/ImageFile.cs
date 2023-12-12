using System;
namespace NotesShared.Models
{
    public class ImageFile
    {
        public string Base64data { get; set; } = String.Empty;
        public string ContentType { get; set; } = String.Empty;
        public string FileName { get; set; } = String.Empty;
    }
}

