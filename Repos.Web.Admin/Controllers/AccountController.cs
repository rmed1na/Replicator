using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repos.Web.Admin.Data;
using Repos.Web.Admin.Models;

namespace Repos.Web.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private IRepository _repo;
        private Session _session;

        public AccountController(ILogger<AccountController> logger, IRepository repo)
        {
            _logger = logger;
            _repo = repo;
            _session = new Session();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var ok = _repo.AuthenticateUser(ref user);

            if (ok)
            {
                HttpContext.Session.SetString(_session.User, user.Username);
                return RedirectToAction("Index", "Home");
            }
            else
                return View();
        }
    }
}