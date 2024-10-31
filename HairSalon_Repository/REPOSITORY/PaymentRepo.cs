using HairSalon_DAO.DAO;
using HairSalon_Repository.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.REPOSITORY
{
    public class PaymentRepo : IPaymentRepo
    {
        public bool AddPayment(HairSalon_BusinessObject.Models.Payment payment)
            => PaymentDAO.Instance.AddPayment(payment);
    }
}
