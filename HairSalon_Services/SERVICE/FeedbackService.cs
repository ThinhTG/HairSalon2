using HairSalon_BusinessObject.Models;
using HairSalon_Repository.INTERFACE;
using HairSalon_Repository.REPOSITORY;
using HairSalon_Services.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.SERVICE
{
    public class FeedbackService : IFeedbackService
    {

        IFeedbackRepo _feedbackRepo;
        public FeedbackService() {
            _feedbackRepo = new FeedbackRepo();
        }
        public bool addFeedback(Feedback feedback)
        {
            return _feedbackRepo.addFeedback(feedback);
        }
    }
}
