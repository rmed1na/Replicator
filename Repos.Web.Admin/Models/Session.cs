using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Web.Admin.Models
{
    public class Session
    {
        private const string _user = "_user";
        public string User
        {
            get { return _user; }
        }
    }
}
