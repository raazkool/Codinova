using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodinovaTask.Model
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        public int OrderBy { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string ProductImage { get; set; }
    }
}
