using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.INTERFACE
{
    public interface IPaymentRepo
    {
        public bool AddPayment(HairSalon_BusinessObject.Models.Payment payment);
    }
}
