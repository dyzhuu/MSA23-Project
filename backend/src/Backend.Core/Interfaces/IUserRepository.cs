﻿using Backend.Domain;

namespace Backend.Core.Interfaces
{
    public interface IUserRepository
	{
		// scuffed
        bool CreateUser(User user);

		bool UpdateUser(User user);

		ICollection<User> GetUsers();

		User GetUser(int id);

        User GetUser(string username);

		bool DeleteUser(User user);

        bool UserExists(int userId);

        bool UserExists(string username);

    }
}

