using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repos.Web.Admin.Data;
using Repos.Web.Admin.Models;
using Repos.Web.Admin.ViewModels;

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

        public IActionResult GetInventory()
        {
            return View(_repo.GetInventory());
        }

        public IActionResult EditInventory(Guid itemId, Guid warehouseId, Guid inventoryId)
        {
            Inventory inventory = new Inventory();

            if (inventoryId != default)
                inventory = _repo.GetInventoryById(inventoryId);
            else
            {
                inventory.Id = inventoryId;
                inventory.Item = _repo.GetItemById(itemId);
                inventory.Warehouse = _repo.GetWarehouseById(warehouseId);
            }

            return View(inventory);
        }

        [HttpPost]
        public IActionResult EditInventory(Inventory inventory)
        {
            inventory.Warehouse = _repo.GetWarehouseByCode(inventory.Warehouse.Code);
            inventory.Item = _repo.GetItemByCode(inventory.Item.Code);
            _repo.EditInventory(inventory);
            return RedirectToAction(nameof(GetInventory));
        }
    }
}