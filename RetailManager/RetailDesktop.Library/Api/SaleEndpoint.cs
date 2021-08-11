using RetailDesktop.Library.Helpers;
using RetailDesktop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RetailDesktop.Library.Api
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private readonly IAPIHelper _apiHelper;

        public SaleEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async void PostSale(SaleModel sale)
        {
            //HttpContent content;
            //using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync("Sale", ""))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        throw new NotImplementedException();
            //    }
            //}
            throw new NotImplementedException();
        }
    }
}
