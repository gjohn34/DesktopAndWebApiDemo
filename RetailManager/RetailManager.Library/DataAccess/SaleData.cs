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
            // sale is from the client
            // details are what is being written to db
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            // converting each sale detail into a detaildb
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

            // creating new sale that has current date set automatically
            SaleDBModel newSale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                UserId = userId
            };

            newSale.Total = newSale.SubTotal + newSale.Tax;


            using(SqlDataAccess sql = new SqlDataAccess())
            {   
                try
                {
                    sql.StartTransaction("RetailManager");
                
                    // write the sale first
                    sql.WriteDataInTransaction("dbo.spInsertSale", newSale);

                    // find the sale 
                    // TODO - find a way to get it on the write
                    newSale.Id = sql.LoadDataInTransaction<int, dynamic>("spSaleLookup", new { newSale.UserId, newSale.SaleDate }).FirstOrDefault();

                    // Save each saleItem
                    foreach (SaleDetailDBModel detailItem in details)
                    {
                        detailItem.SaleId = newSale.Id;
                        sql.WriteDataInTransaction("dbo.spInsertSaleDetail", detailItem);
                        var p = new { Id = detailItem.ProductId, Quantity = detailItem.Quantity };
                        sql.WriteDataInTransaction("dbo.spUpdateProductQuantity", p);
                    }
                } catch
                {
                    sql.RollbackTransaction();
                    throw;
                }
            }


            // if successful, get id


            // write each sale detail

        }
        public static List<SaleReportModel> GetSaleReport()
        {
            SqlDataAccess sql = new SqlDataAccess();
            return sql.LoadData<SaleReportModel, dynamic>("spGetSalesReport", new { }, "RetailManager");
        }
    }
}
