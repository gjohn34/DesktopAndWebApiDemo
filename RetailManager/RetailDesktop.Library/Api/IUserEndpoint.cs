using RetailDesktop.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailDesktop.Library.Api
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAll();
    }
}