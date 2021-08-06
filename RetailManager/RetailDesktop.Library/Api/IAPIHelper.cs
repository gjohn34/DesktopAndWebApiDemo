using RetailDesktop.Library.Models;
using System.Threading.Tasks;

namespace RetailDesktop.Library.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}