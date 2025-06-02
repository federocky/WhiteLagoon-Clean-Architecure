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
                return RedirectToAction(nameof(Index));
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

        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(l => new SelectListItem
                {
                    Text = l.Name,
                    Value = l.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(vn => vn.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "Villa Number updated";
                return RedirectToAction(nameof(Index));
            }

            villaNumberVM.VillaList = _db.Villas.ToList().Select(l => new SelectListItem
            {
                Text = l.Name,
                Value = l.Id.ToString()
            });            

            return View(villaNumberVM);
        }

        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(l => new SelectListItem
                {
                    Text = l.Name,
                    Value = l.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(vn => vn.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? dbVillaNumber = _db.VillaNumbers.FirstOrDefault(v => v.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (dbVillaNumber is not null)
            {
                _db.VillaNumbers.Remove(dbVillaNumber);
                _db.SaveChanges();
                TempData["success"] = "Villa number deleted";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Villa number cannot be deleted";

            return View();
        }

    }
}
