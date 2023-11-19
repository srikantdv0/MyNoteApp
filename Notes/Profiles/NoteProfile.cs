using System;
using AutoMapper;
using Notes.Entities;
using NotesShared.Models;

namespace Notes.Profiles
{
	public class NoteProfile : Profile
	{
		public NoteProfile()
		{
			CreateMap<NotesForCreationDto, Note>();
			CreateMap<Note, NoteContentDto>();
            CreateMap<Note, NoteMetadata>();
            CreateMap<NoteContentDto, Note>();

            CreateMap<Note, SharedNoteMetadata>()
                .ForMember(dest => dest.PermissionId,
                   opts => opts.MapFrom(src =>
                       src.SharedNote.Select(ci => ci.PermissionId).FirstOrDefault()));

            CreateMap<Note, SharedNoteContentDto>()
                .ForMember(dest => dest.PermissionId,
                   opts => opts.MapFrom(src =>
                       src.SharedNote.Select(ci => ci.PermissionId).FirstOrDefault()));
 
            CreateMap<Note, NoteforUpdateDto>();
            CreateMap<NoteforUpdateDto, Note>();

            CreateMap<SharedNote, SharedNoteUsersDto>()
                .ForMember(dest => dest.UserId,
                   opts => opts.MapFrom(src => src.User!.Id))
                .ForMember(dest => dest.UserName,
                   opts => opts.MapFrom(src => src.User!.Name))
                .ForMember(dest => dest.Permission,
                   opts => opts.MapFrom(src => src.Permission!.Description));

            CreateMap<UserForCreationDto,User>();
            CreateMap<User,UserProfileDto>()
                .ForMember(dest => dest.UserId,
                opts => opts.MapFrom(src => src.Id)
                );
        }
	}
}

