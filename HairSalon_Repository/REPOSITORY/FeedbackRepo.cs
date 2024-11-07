﻿using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DAO;
using HairSalon_Repository.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.REPOSITORY
{
    public class FeedbackRepo : IFeedbackRepo
    {
        public bool addFeedback(Feedback feedback) => FeedbackDAO.Instance.AddFeedback(feedback);
       
    }
}