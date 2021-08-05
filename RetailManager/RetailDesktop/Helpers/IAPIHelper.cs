using RetailDesktop.Models;
using System.Threading.Tasks;

namespace RetailDesktop.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}