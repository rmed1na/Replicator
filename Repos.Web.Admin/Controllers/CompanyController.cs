using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repos.Web.Admin.Data;
using Repos.Web.Admin.Models;

namespace Repos.Web.Admin.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ILogger<CompanyController> _logger;
        private IRepository _repo;
        private Session _session;

        public CompanyController(ILogger<CompanyController> logger, IRepository repo)
        {
            _logger = logger;
            _repo = repo;
            _session = new Session();
        }

        public IActionResult Index()
        {
            List<Company> companies = _repo.GetCompanies();
            ViewData["Companies"] = companies;

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Company company)
        {
            //TODO: Popup de creación satisfactoria
            _repo.CreateCompany(company);

            return RedirectToAction("Index", "Company");
        }

        public IActionResult Edit(Guid Id)
        {
            Company company = _repo.GetCompanyById(Id);
            return View(company);
        }

        [HttpPost]
        public IActionResult Edit(Guid Id, [Bind("Id, Code, Name, Status")] Company company)
        {
            //TODO: Successful action popup
            
            if (Id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _repo.EditCompany(company);
            }

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(Guid Id)
        {
            Company company = _repo.GetCompanyById(Id);
            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(Company company)
        {
            _repo.DeleteCompany(company);
            return RedirectToAction(nameof(Index));
        }
    }
}