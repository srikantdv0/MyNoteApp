using System;
namespace NotesShared.Models
{
    public class ShareToUserDTO
    {
        public int UserId { get; set; }
        public int NoteId { get; set; }
        public int PermissionId { get; set; }
    }
}

