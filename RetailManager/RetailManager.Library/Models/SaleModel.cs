using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetailManager.Library.Models
{
    public class SaleModel
    {
        //public int Id { get; set; }
        public List<SaleDetailModel> SaleDetails { get; set; }
        //public string UserId { get; set; }
        //public DateTime SaleDate { get; set; } = DateTime.UtcNow;
        //public decimal SubTotal { get; set; }
        //public decimal Tax { get; set; }
        //public decimal Total { get; set; }
    }
}