﻿using Backend.Core.Interfaces;
using Backend.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Backend.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
        private readonly IUserService _userService;
        private readonly IBookmarkService _bookmarkService;

        public UserController(IUserService userService, IBookmarkService bookmarkService)
		{
            _userService = userService;
            _bookmarkService = bookmarkService;
        }

        private async Task<int> GetLoggedInUserId()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            int internalUserId = await _userService.GetInternalId(userId);

            return internalUserId;
        }

        [Authorize]
        [HttpPut("profile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
        {
            int userId = await GetLoggedInUserId();

            if (userId != userDto.Id)
                return Unauthorized();

            if (userDto is null)
                return BadRequest();

            if (!await _userService.UpdateUser(userDto))
                return StatusCode(500, "Something went wrong"); ;

            return Ok();

        }

  //      [HttpGet]
  //      [ProducesResponseType(200)]
  //      public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
		//{
  //          IEnumerable<UserDto> userDtos = await _userService.GetUsers();

  //          return Ok(userDtos);
		//}

		//[HttpGet("{userId}")]
		//[ProducesResponseType(200)]
  //      [ProducesResponseType(404)]
  //      public async Task<IActionResult> GetUser(int userId)
		//{
		//	UserDto user = await _userService.GetUser(userId);

  //          if (user is null)
  //              return NotFound();

  //          return Ok(user);
  //      }

        [Authorize]
        [HttpDelete("profile")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser()
        {
            int userId = await GetLoggedInUserId();

            if (!await _userService.UserExists(userId))
                return NotFound();

            if (!await _userService.DeleteUser(userId))
                return StatusCode(500, "Something went wrong"); ;

            return NoContent();
        }

        //Bookmarks

        [HttpGet("profile/bookmarks")]
		[ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBookmarks(int userId)
		{
			if (!await _userService.UserExists(userId))
				return NotFound();

            IEnumerable<BookmarkDto> bookmarkDtos = await _bookmarkService.GetBookmarks(userId);

			return Ok(bookmarkDtos);
		}

        [Authorize]
        [HttpPost("profile/bookmarks")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateBookmark([FromQuery] int animeId, [FromBody] BookmarkDto bookmarkDto)
        {
            int userId = await GetLoggedInUserId();

            if (userId != bookmarkDto.UserId)
                return Unauthorized();

            if (bookmarkDto is null)
                return BadRequest();

            if (await _bookmarkService.BookmarkExists(userId, animeId))
                return BadRequest("Bookmark already exists");

            if (!await _bookmarkService.CreateBookmark(bookmarkDto))
                return StatusCode(500, "Something went wrong"); ;

            return CreatedAtAction(nameof(CreateBookmark), bookmarkDto);
        }

        [Authorize]
        [HttpPut("profile/bookmarks/{animeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateBookmark(int animeId, [FromBody] BookmarkDto bookmarkDto)
        {
            int userId = await GetLoggedInUserId();

            if (userId != bookmarkDto.UserId)
                return Unauthorized();

            if (bookmarkDto is null)
                return BadRequest();

            if (!await _bookmarkService.BookmarkExists(userId, animeId))
                return NotFound();

            if (!await _bookmarkService.UpdateBookmark(bookmarkDto))
                return StatusCode(500, "Something went wrong"); ;

            return Ok();
        }

        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("profile/bookmarks/{animeId}")]
        public async Task<IActionResult> DeleteBookmark(int animeId)
        {
            int userId = await GetLoggedInUserId();

            if (!await _bookmarkService.BookmarkExists(userId, animeId))
                return NotFound();

            if (!await _bookmarkService.DeleteBookmark(userId, animeId))
                return StatusCode(500, "Something went wrong"); ;

            return NoContent();
        }
    }
}
