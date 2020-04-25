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
    public class ItemController : Controller
    {
        private readonly ILogger<ItemController> _logger;
        private IRepository _repo;

        public ItemController(ILogger<ItemController> logger, IRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View(_repo.GetItems());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Item item)
        {
            _repo.CreateItem(item);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid Id)
        {
            Item item = _repo.GetItemById(Id);
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Guid Id, [Bind("Id, Code, Description, Price, TaxPercentaje, Status")] Item item)
        {
            if (Id != item.Id)
                return NotFound();

            if (ModelState.IsValid)
                _repo.EditItem(item);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid Id)
        {
            Item item = _repo.GetItemById(Id);
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(Item item)
        {
            _repo.DeleteItem(item);
            return RedirectToAction(nameof(Index));
        }
    }
}