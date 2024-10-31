using HairSalon_BusinessObject.Models;
using HairSalon_Repository.REPOSITORY;
using HairSalon_Services.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.SERVICE
{
    public class PaymentService : IPaymentService
    {
        private PaymentRepo paymentRepo;
        public PaymentService()
        {
            paymentRepo = new PaymentRepo();
        }
        public bool AddPayment(Payment payment)
            => paymentRepo.AddPayment(payment);
    }
}
