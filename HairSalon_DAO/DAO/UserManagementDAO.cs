using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class UserManagementDAO
    {
        private HairSalonServiceContext dbContext;
        private static UserManagementDAO instance;

        public UserManagementDAO()
        {
            dbContext = new HairSalonServiceContext();
        }
        public static UserManagementDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserManagementDAO();
                }
                return instance;
            }
        }

        public User GetUserById(int id)
        {
            return dbContext.User.SingleOrDefault(u => u.UserId == id);
        }

        public List<User> GetUserList()
        {
            return dbContext.User.ToList();
        }

        public bool AddUser(User user)
        {
            bool isSuccess = false;
            try
            {
                User users = GetUserById(user.UserId);
                if (users == null)
                {
                    dbContext.User.Add(user);
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
                else
                {
                    throw new Exception("Not Found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the user: " + ex.InnerException?.Message, ex);
            }
            return isSuccess;
        }

        public bool UpdateUser(User user)
        {
            bool isSuccess = false;
            try
            {
                User users = GetUserById(user.UserId);
                if (users != null)
                {
                    var existingEntity = dbContext.User.Local
                              .FirstOrDefault(e => e.UserId == user.UserId);

                    if (existingEntity != null)
                    {
                        dbContext.Entry(existingEntity).State = EntityState.Detached;
                    }
                    dbContext.Entry(user).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
                else
                {
                    throw new Exception("Not Found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user: " + ex.InnerException?.Message, ex);
            }
            return isSuccess;
        }

        public bool DeleteUser(int userId)
        {
            bool isSuccess = false;
            try
            {
                User users = GetUserById(userId);
                if (users != null)
                {
                    dbContext.User.Remove(users);
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
                else
                {
                    throw new Exception("Not Found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the user: " + ex.InnerException?.Message, ex);
            }
            return isSuccess;
        }
    }
}
