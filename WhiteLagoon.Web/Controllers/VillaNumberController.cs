using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly AppDbContext _db;

        public VillaNumberController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(VillaNumber villaNumber)
        {

            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Add(villaNumber);
                _db.SaveChanges();
                TempData["success"] = "Villa Number created";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Villa cannot be created";
            return View();
        }

        public IActionResult Update(int villaId)
        {
            Villa? villa = _db.Villas.FirstOrDefault(x => x.Id == villaId);

            if(villa is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Update(Villa villa)
        {
            if (ModelState.IsValid && villa.Id != 0)
            {
                _db.Villas.Update(villa);
                _db.SaveChanges();
                TempData["success"] = "Villa has been updated";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Villa cannot be updated";
            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? villa = _db.Villas.FirstOrDefault(x => x.Id == villaId);

            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? dbVilla = _db.Villas.FirstOrDefault(v => v.Id == villa.Id);

            if (dbVilla is not null)
            {
                _db.Villas.Remove(dbVilla);
                _db.SaveChanges();
                TempData["success"] = "Villa deleted";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Villa cannot be deleted";

            return View();
        }

    }
}
