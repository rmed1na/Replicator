using Repos.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repos.Web.Admin.Data
{
    public interface IRepository
    {
        bool AuthenticateUser(ref User user);
        User GetUserDataByUsername(ref User user);
    }
}
