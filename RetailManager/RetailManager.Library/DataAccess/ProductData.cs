using RetailManager.Library.Internal.DataAccess;
using RetailManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetAllProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            // TODO - add overflow to loaddata to accept no p
            return sql.LoadData<ProductModel, dynamic>
                ("dbo.spGetAllProducts", new { }, "RetailManager");
        }
    }
}
