using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Models.ViewModel
{
    public class CartVM
    {
        public IList<Service> ServicesList { get; set; }
        public OrderHeader OrderHeader { get; set; }

    }
}
