using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class PaymentDAO
    {
        private HairSalonServiceContext _context;
        private static PaymentDAO instance = null;
        public static PaymentDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PaymentDAO();
                }
                return instance;
            }
        }

        public PaymentDAO()
        {
            _context = new HairSalonServiceContext();
        }

        public bool AddPayment(Payment payment)
        {
            bool isSuccess = false;
            try
            {
                _context.Payment.Add(payment);
                _context.SaveChanges();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

    }
}
