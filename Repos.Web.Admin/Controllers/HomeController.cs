using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repos.Web.Admin.Models;
using Microsoft.AspNetCore.Session;
using Repos.Web.Admin.Data;
using Microsoft.AspNetCore.Http;

namespace Repos.Web.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IRepository _repo;
        private Session _session;
        private User _user;

        public HomeController(ILogger<HomeController> logger, IRepository repo)
        {
            _logger = logger;
            _repo = repo;
            _user = new User();
            _session = new Session();
        }

        public IActionResult Index()
        {
            _user.Username = HttpContext.Session.GetString(_session.User);

            if (!string.IsNullOrWhiteSpace(_user.Username))
                _repo.GetUserDataByUsername(ref _user);

            if (_user.Id == default || _user == null)
                return RedirectToAction("Login", "Account");
            else
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Company()
        {
            return View();
        }
    }
}
