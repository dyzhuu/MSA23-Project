﻿using System;
using Backend.Models;
using Backend.Dto;

namespace Backend.Services
{
	public interface IUserService
	{
        //scuffed
        bool CreateUser(User user);

        bool UpdateUser(UserDto userDto);

        ICollection<User> GetUsers();

        User GetUser(int userId);

        User GetUser(string username);

        ICollection<BookmarkDto> GetBookmarks(int userId);

        Bookmark GetBookmark(int userId, int animeId);

        bool UserExists(int userId);

        bool BookmarkExists(int userId, int animeId);

        bool CreateBookmark(BookmarkDto bookmarkDto);

        bool UpdateBookmark(BookmarkDto bookmarkDto);

    }
}

