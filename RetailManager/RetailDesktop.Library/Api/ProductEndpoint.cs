﻿using RetailDesktop.Library.Helpers;
using RetailDesktop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RetailDesktop.Library.Api
{
    public class ProductEndpoint : IProductEndpoint
    {
        private readonly IAPIHelper _apiHelper;
        public ProductEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        public async Task<List<ProductModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("Product"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<ProductModel>>();
                    // TODO - Mapper
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

    }
}
