using Repos.Web.Admin.Models;
using Repos.Web.Admin.ViewModels;
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
        List<Company> GetCompanies(bool justActive = false);
        bool CreateCompany(Company company);
        Company GetCompanyById(Guid Id);
        bool EditCompany(Company company);
        bool DeleteCompany(Company company);
        StoreViewModel GetStores(bool justActive = false);
    }
}
