using RetailDesktop.Library.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailDesktop.Library.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
        void LogOffUser();
        HttpClient ApiClient { get; }
    }
}