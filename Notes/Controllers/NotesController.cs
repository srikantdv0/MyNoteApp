using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Notes.Entities;
using NotesShared.Models;
using Notes.Services;

namespace Notes.Controllers
{
    [ApiController]
	[Route("api/[Controller]")]
	[Authorize]
    public class NotesController : ControllerBase
	{
		
        private readonly INoteRepository _noteRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
		
		private readonly int userContextId;

        private readonly string userContextName;

        public NotesController(INoteRepository noteRepository, IMapper mapper, ILogger<NotesController> logger
			,IHttpContextAccessor httpContext)
		{
           
            _noteRepository = noteRepository;
			_mapper = mapper;
			_logger = logger;

            userContextId = Int32.Parse(httpContext.HttpContext!.User.Claims.Where(d => d.Type == "UserId")
                    .Select(d => d.Value).FirstOrDefault() ?? throw new ArgumentNullException(nameof(httpContext)));

            userContextName = httpContext.HttpContext!.User.Claims.Where(d => d.Type == "UserName")
                    .Select(d => d.Value).FirstOrDefault() ?? throw new ArgumentNullException(nameof(httpContext));

        }

                

        [HttpPost("Createnote")]
		public async Task<ActionResult> CreateNote(NotesForCreationDto notesForCreationDto)
		{
			var note = _mapper.Map<Note>(notesForCreationDto);
			note.CreatedDTS = DateTime.UtcNow;
            note.CreatorId = userContextId;
            note.CreatorName = userContextName;
            var id = await _noteRepository.CreateNoteAsync(note);
			return Ok(id);
		}

        [HttpPost("Updatenote/{noteId}")]
        public async Task<ActionResult> UpdateNote(int noteId, NoteContentDto patchDocument)
        {

            var note = await _noteRepository.GetNoteAsync(noteId, null);
            if (note == null)
            {
                _logger.LogWarning($"Action:Updatenote - A note with Id {noteId} not found");
                return NotFound();
            }

            if (!await CheckPermission(note.Id, note.CreatorId, new HashSet<string>() { "RW" }))
            {
                return Unauthorized("You don't have permission to modify the note");
            }

            note.ModifiedById = userContextId;
            note.ModifiedByName = userContextName;
            note.ModifiedDTS = DateTime.UtcNow;
            var noteToPatch = _mapper.Map<NoteContentDto>(patchDocument);

            if (!TryValidateModel(noteToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(noteToPatch, note);

            await _noteRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("Updatenote/{noteId}")]
		public async Task<ActionResult> UpdateNote(int noteId, JsonPatchDocument<NoteforUpdateDto> patchDocument)
		{
			
			var note = await _noteRepository.GetNoteAsync(noteId,null);
			if (note == null)
			{
				_logger.LogWarning($"Action:Updatenote - A note with Id {noteId} not found");
				return NotFound();
			}

			if (! await CheckPermission(note.Id, note.CreatorId, new HashSet<string>() { "RW"}))
			{
				return Unauthorized("You don't have permission to modify the note");
			}

            note.ModifiedById = userContextId;
            note.ModifiedByName = userContextName;
            note.ModifiedDTS = DateTime.UtcNow;
            var noteToPatch = _mapper.Map<NoteforUpdateDto>(note);
            patchDocument.ApplyTo(noteToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!TryValidateModel(noteToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(noteToPatch, note);

            await _noteRepository.SaveChangesAsync();

            return NoContent();
		}

		[HttpDelete("DeleteNote/{noteId}")]
		public async Task<ActionResult> DeleteNote(int noteId)
		{
           
            var note = await _noteRepository.GetNoteAsync(noteId,null);
            if (note == null)
            {
                return NotFound();
            }

            if (note.CreatorId != userContextId)
            {
                return Unauthorized("You don't have permission to delete the note");
            }

            _noteRepository.DeleteNote(note);
            await _noteRepository.SaveChangesAsync();
            return NoContent();
		}

		[HttpGet("Getnote/{noteId}")]
		public async Task<ActionResult<NoteContentDto?>> GetNote(int noteId)
		{
            var res = await _noteRepository.GetNoteAsync(noteId, userContextId);

            if (res == null)
			{
				return NotFound();
			}
			var noteContentDto = _mapper.Map<NoteContentDto>(res);
			return Ok(noteContentDto);
		}

		[HttpGet("GetNotes")]
		public async Task<ActionResult<IEnumerable<NoteMetadata>>> GetNotes(int? Id)
		{
            var res = await _noteRepository.GetNotesAsync(userContextId);
            if (res == null)
			{
				return NotFound();
			}
			var noteMetadata = _mapper.Map<IEnumerable<NoteMetadata>>(res);

            if (Id == null)
            {
                return Ok(noteMetadata);
            }
            else
            {
                var note = noteMetadata.Where(a => a.Id == Id).FirstOrDefault();
                if (note != null)
                {
                    return Ok(note);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpGet("GetSharednote/{noteId}")]
		public async Task<ActionResult<SharedNoteContentDto>> GetSharedNote(int noteId)
		{
			var res = await _noteRepository.GetSharedNoteAsync(noteId,userContextId);
			if (res == null)
			{
				return NotFound();
			}
			var sharedNoteContent = _mapper.Map<SharedNoteContentDto>(res);
			return Ok(sharedNoteContent);
		}

		[HttpGet("Getsharednotes")]
        public async Task<ActionResult<IEnumerable<SharedNoteMetadata>>> GetSharedNotes(int? Id)
        {
            var res = await _noteRepository.GetSharedNotesAsync(userContextId);
            if (res == null)
            {
                return NotFound();
            }
            var noteSharedMetadata = _mapper.Map<IEnumerable<SharedNoteMetadata>>(res);
            if (Id == null)
            {
                return Ok(noteSharedMetadata);
            }
            else
            {
                var note = noteSharedMetadata.Where(a => a.Id == Id).FirstOrDefault();
                if (note != null)
                {
                    return Ok(note);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost("Sharenote")]
        public async Task<ActionResult> ShareNote(ShareToUserDTO shareToUserDTO)
        {
            if (shareToUserDTO.UserId == userContextId)
            {
                return BadRequest("Owner can't share to self");
            }

            var note = await _noteRepository.GetNoteAsync(shareToUserDTO.NoteId, null);

            if (note == null)
            {
                return NotFound();
            }
            if (note.CreatorId != userContextId)
            {
                return Unauthorized("You don't have permission to share the note");
            }

            await _noteRepository.ShareNoteAsync(shareToUserDTO.UserId,note.Id,shareToUserDTO.PermissionId);
            await _noteRepository.SaveChangesAsync();
            return NoContent();
        }

		[HttpDelete("UnSharenote/{noteId}")]
		public async Task<ActionResult> UnShareNote(int userId, int noteId)
		{
            if (userId == userContextId)
            {
                return BadRequest("Owner can't unshare to self");
            }
            var note = await _noteRepository.GetNoteAsync(noteId, null);

            if (note == null)
            {
                return NotFound();
            }

            if (note.CreatorId != userContextId)
            {
                return Unauthorized("You don't have permission to unshare the note");
            }

            var sharedNote = await _noteRepository.GetSharedNoteToAsync(userId,noteId);
			if (sharedNote == null)
			{
				return NotFound();
			}

            _noteRepository.UnShareNote(sharedNote);
			await _noteRepository.SaveChangesAsync();
			return NoContent();
		}


        [HttpGet("GetSharedNoteUsers/{noteId}")]
        public async Task<ActionResult<IEnumerable<SharedNoteUsersDto>>> GetSharedNoteUsers(int noteId)
        {
            var note = await _noteRepository.GetNoteAsync(noteId, null);

            if (note == null)
            {
                return NotFound();
            }

            if (note.CreatorId != userContextId)
            {
                return Unauthorized("You don't have permission");
            }

            var shared = await _noteRepository.GetSharedNoteDetails(noteId);

            var sharedNote = _mapper.Map<IEnumerable<SharedNoteUsersDto>>(shared);
            return Ok(sharedNote);
        }

        [HttpDelete("UnSuscribetonote/{noteId}")]
        public async Task<ActionResult> UnSubscribeToNote(int noteId)
        {
            var note = await _noteRepository.GetNoteAsync(noteId, null);

            if (note == null)
            {
                return NotFound();
            }

            if (note.CreatorId == userContextId)
            {
                return Unauthorized("Can't unsuscribe a creator of the note");
            }
            var sharedNote = await _noteRepository.GetSharedNoteToAsync(userContextId, noteId);

            if (sharedNote == null)
            {
                return NotFound();
            }

            _noteRepository.UnShareNote(sharedNote);
            await _noteRepository.SaveChangesAsync();
            return NoContent();

        }


        private async Task<bool> CheckPermission(int noteId, int ownerId, HashSet<string> requiredPermissions)
		{
            if (!(ownerId == userContextId))
            {
                var perm = await _noteRepository.GetSharedPermission(noteId, userContextId);
                if (perm == null || !requiredPermissions.Contains(perm))
                {
					return false;
                }
            }
			return true;
        }
        
    }

    
}

