using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_153504_Klimkovich.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public Electronics? Electronic { get; set; }
        public int Count { get; set; }
    }
}
