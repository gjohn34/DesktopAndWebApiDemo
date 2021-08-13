using Dapper;
using RetailManager.Library.Helpers;
using RetailManager.Library.Internal.DataAccess;
using RetailManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.Library.DataAccess
{
    public class SaleData
    {
        public static void InsertSale(SaleModel sale, string userId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            foreach (SaleDetailModel saleDetail in sale.SaleDetails)
            {
                SaleDetailDBModel detail = new SaleDetailDBModel(saleDetail.ProductId, saleDetail.Quantity);

                var pi = ProductData.GetProductById(detail.ProductId);

                if (pi == null)
                {
                    throw new Exception("Does not exist");
                }
                detail.PurchasePrice = pi.RetailPrice * detail.Quantity;
                if (pi.IsTaxable)
                {
                    decimal tax = detail.PurchasePrice * (ConfigHelper.GetTaxRate() / 100);
                    detail.Tax = tax;
                }

                details.Add(detail);

            }

            SaleDBModel newSale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                UserId = userId,
            };
            newSale.Total = newSale.SubTotal + newSale.Tax;


            // write the sale first
            SqlDataAccess sql = new SqlDataAccess();

            sql.WriteData("dbo.spInsertSale", newSale, "RetailManager");

            int id = 2;

            foreach (SaleDetailDBModel detailItem in details)
            {
                detailItem.SaleId = id;
                sql.WriteData("dbo.spInsertSaleDetail", detailItem, "RetailManager");
            }
            // if successful, get id


            // write each sale detail

        }
    }
}
