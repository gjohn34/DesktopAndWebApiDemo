using Microsoft.AspNet.Identity;
using RetailManager.Library.DataAccess;
using RetailManager.Library.Models;
using RetailManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RetailManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        // POST: api/Sale
        [HttpPost]
        public void Post(SaleModel sale)
        {
            SaleData.InsertSale(sale, RequestContext.Principal.Identity.GetUserId());
        }

        [HttpGet]
        // TODO - Authorized by role
        public List<SaleReportModel> GetSalesReport()
        {
            return SaleData.GetSaleReport();
        }
    }
}
