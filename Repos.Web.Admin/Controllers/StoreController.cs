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
    public class StoreController : Controller
    {
        private readonly ILogger<StoreController> _logger;
        private IRepository _repo;

        public StoreController(ILogger<StoreController> logger, IRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View(_repo.GetStores());
        }

        public IActionResult Edit(Guid Id)
        {
            Store store = _repo.GetStoreById(Id);
            return View(store);
        }

        [HttpPost]
        public IActionResult Edit(Guid Id, [Bind("Id", "Code", "Name", "Status", "Address")] Store store)
        {
            if (Id != store.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _repo.EditStore(store);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Store store)
        {
            _repo.CreateStore(store);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid Id)
        {
            Store store = _repo.GetStoreById(Id);
            return View(store);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(Store store)
        {
            _repo.DeleteStore(store);
            return RedirectToAction(nameof(Index));
        }
    }
}