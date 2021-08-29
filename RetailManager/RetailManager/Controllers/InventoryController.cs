using RetailManager.Library.DataAccess;
using RetailManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RetailManager.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class InventoryController : ApiController
    {
        [HttpGet]
        public List<InventoryModel> GetInventoryData()
        {
            InventoryData data = new InventoryData();
            return data.GetInventory();
        }

        [HttpPost]
        public void Post(InventoryModel item)
        {
            InventoryData data = new InventoryData();
            data.SaveInventoryRecord(item);

        }
    }
}
