﻿using System;
using Backend.Models;
using Backend.Dtos;
using AutoMapper;

namespace Backend.Helper
{
	public class AutoMapperProfile : Profile
	{
        private readonly IMapper _mapper;

        public AutoMapperProfile()
		{
			CreateMap<Anime, AnimeDto>();
            CreateMap<AnimeDto, Anime>();
            CreateMap<Bookmark, BookmarkDto>()
				.ForMember(d => d.Title, b => b.MapFrom(s => s.Anime.Title))
				.ForMember(d => d.ImageURL, b => b.MapFrom(s => s.Anime.ImageURL))
				.ForMember(d => d.Description, b => b.MapFrom(s => s.Anime.Description));
			CreateMap<BookmarkDto, Bookmark>();

			//id provided in userService
			CreateMap<BookmarkDto, Anime>()
				.ForMember(d => d.Id, b => b.MapFrom(s => s.AnimeId));

			CreateMap<UserDto, User>();
			//CreateMap<User, UserDto>()
			//	.ForMember(d => d.BookmarkDtos, b => b.MapFrom(s => _mapper.Map<List<BookmarkDto>>(s.Bookmarks)));
        }
    }
}

