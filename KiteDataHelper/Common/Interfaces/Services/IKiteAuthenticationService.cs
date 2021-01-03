using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KiteDataHelper.Common.Interfaces.Services
{
    public interface IKiteAuthenticationService
    {
        Task<bool> IsAuthenticated();
        Task Login(string request_token);
        Task Logout();
        Task RefreshKiteAccessToken();
    }
}
