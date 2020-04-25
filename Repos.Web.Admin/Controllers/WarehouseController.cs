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
    public class WarehouseController : Controller
    {
        private readonly ILogger<WarehouseController> _logger;
        private IRepository _repo;

        public WarehouseController(ILogger<WarehouseController> logger, IRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }
        public IActionResult Index()
        {
            return View(_repo.GetWarehouses());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Warehouse warehouse)
        {
            _repo.CreateWarehouse(warehouse);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid Id)
        {
            Warehouse warehouse = _repo.GetWarehouseById(Id);
            return View(warehouse);
        }

        [HttpPost]
        public IActionResult Edit(Guid Id, [Bind("Id, Code, Name, Address, Status, Store")] Warehouse warehouse)
        {
            if (Id != warehouse.Id)
                return NotFound();

            if (ModelState.IsValid)
                _repo.EditWarehouse(warehouse);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid Id)
        {
            Warehouse warehouse = _repo.GetWarehouseById(Id);
            return View(warehouse);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(Warehouse warehouse)
        {
            _repo.DeleteWarehouse(warehouse);
            return RedirectToAction(nameof(Index));
        }
    }
}