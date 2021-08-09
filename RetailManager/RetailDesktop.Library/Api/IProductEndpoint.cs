using RetailDesktop.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailDesktop.Library.Api
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}