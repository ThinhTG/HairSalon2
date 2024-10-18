using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{

    public class UserDAO
    {
        private HairSalonServiceContext _context;
        private static UserDAO instance = null;

        public static UserDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserDAO();
                }
                return instance;
            }
        }

        public UserDAO()
        {
            _context = new HairSalonServiceContext();
        }

        public User GetUser(string email, string password)
        {
            return _context.User.Where(u => u.Email == email && u.Password == password).FirstOrDefault();
        }

        public User GetUserByName(string username)
        {
            return _context.User.Where(u => u.UserName == username).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return _context.User.Where(u => u.Email == email).FirstOrDefault();
        }

        public List<User> GetUsers()
        {
            return new List<User>(_context.User.ToList());
        }

        public bool AddUser(User user)
        {
            User user1 = GetUserByEmail(user.Email);
            try
            {
                if (user1 != null)
                {
                    return false;
                }
                else
                {
                    _context.User.Add(user);
                    _context.SaveChanges();
                    return true;
                }
            }

            catch (Exception)
            {
                return false;
            }
        }

    }
}
