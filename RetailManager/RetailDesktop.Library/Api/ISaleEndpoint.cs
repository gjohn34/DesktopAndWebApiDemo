using RetailDesktop.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailDesktop.Library.Api
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}