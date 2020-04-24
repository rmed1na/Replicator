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
    public class PosController : Controller
    {
        private readonly ILogger<PosController> _logger;
        private IRepository _repo;

        public PosController(ILogger<PosController> logger, IRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }
        public IActionResult Index()
        {
            return View(_repo.GetPos());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Pos pos)
        {
            _repo.CreatePos(pos);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid Id)
        {
            Pos pos = _repo.GetPosById(Id);
            return View(pos);
        }
        
        [HttpPost]
        public IActionResult Edit(Guid Id, [Bind("Id, Code, Name, Status, Warehouse")] Pos pos)
        {
            // TODO: Successful action popup

            if (Id != pos.Id)
                return NotFound();

            if (ModelState.IsValid)
                _repo.EditPos(pos);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid Id)
        {
            Pos pos = _repo.GetPosById(Id);
            return View(pos);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(Pos pos)
        {
            _repo.DeletePos(pos);
            return RedirectToAction(nameof(Index));
        }
    }
}