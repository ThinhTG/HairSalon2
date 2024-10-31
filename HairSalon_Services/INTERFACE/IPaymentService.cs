using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.INTERFACE
{
    public interface IPaymentService
    {
        public bool AddPayment(HairSalon_BusinessObject.Models.Payment payment);
    }
}
