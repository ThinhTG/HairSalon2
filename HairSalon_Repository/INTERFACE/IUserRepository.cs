﻿using HairSalon_BusinessObject.Models;
using HairSalon_DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.INTERFACE
{
    public interface IUserRepository
    {
        public User GetUserByEmail(string email);

        public User GetUserByName(string username);

        public List<User> GetUsers();

        public User GetUserByUserId(int userId);
        public bool AddUser(User user);

        public User GetUserById(int id);
    }
}
