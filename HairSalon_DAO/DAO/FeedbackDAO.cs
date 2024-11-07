using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class FeedbackDAO
    {
        private HairSalonServiceContext dbContext;

        private static FeedbackDAO instance = null;

        public static FeedbackDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FeedbackDAO();
                }
                return instance;
            }
        }

        public FeedbackDAO()
        {
            dbContext = new HairSalonServiceContext();
        }


        public List<Feedback> GetAll()
        {
            return dbContext.Feedback.ToList();
        }

        public Feedback GetFeedbackById(int feedbackId)
        {
            return dbContext.Feedback.SingleOrDefault(b => b.FeedbackId == feedbackId);
        }

        public bool AddFeedback(Feedback feedback)
        {
            bool isSuccess = false;
            try
            {
                if (feedback != null)
                {
                    dbContext.Feedback.Add(feedback);
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while adding Feedback: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                }
                throw;
            }
            return isSuccess;
        }


      

        public bool SaveChanges()
        {
            try
            {
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving changes: " + ex.Message);
            }
        }
    }
}

