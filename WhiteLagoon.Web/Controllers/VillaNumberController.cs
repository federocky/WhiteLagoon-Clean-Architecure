using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;
using Microsoft.EntityFrameworkCore; 

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
            var villaNumbers = _db.VillaNumbers.Include(vn => vn.Villa).ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(l => new SelectListItem
                {
                    Text = l.Name,
                    Value = l.Id.ToString()
                })
            }; 

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM villaNumber)
        {
            bool roomNumberExists = _db.VillaNumbers.Any(vn => vn.Villa_Number == villaNumber.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                _db.VillaNumbers.Add(villaNumber.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "Villa Number created";
                return RedirectToAction("Index");
            }

            if (roomNumberExists)
            {
                TempData["error"] = "Villa number already exists";
            } else
            {
                TempData["error"] = "Villa cannot be created";
            }

            villaNumber = new()
            {
                VillaList = _db.Villas.ToList().Select(l => new SelectListItem
                {
                    Text = l.Name,
                    Value = l.Id.ToString()
                })
            };


            return View(villaNumber);
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
