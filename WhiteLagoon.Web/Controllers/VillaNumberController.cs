using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa").ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(l => new SelectListItem
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
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(vn => vn.Villa_Number == villaNumber.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                _unitOfWork.VillaNumber.Add(villaNumber.VillaNumber);
                _unitOfWork.SaveChanges();
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
                VillaList = _unitOfWork.Villa.GetAll().Select(l => new SelectListItem
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
                VillaList = _unitOfWork.Villa.GetAll().Select(l => new SelectListItem
                {
                    Text = l.Name,
                    Value = l.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(vn => vn.Villa_Number == villaNumberId)
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
                _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfWork.SaveChanges();
                TempData["success"] = "Villa Number updated";
                return RedirectToAction(nameof(Index));
            }

            villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(l => new SelectListItem
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
                VillaList = _unitOfWork.Villa.GetAll().Select(l => new SelectListItem
                {
                    Text = l.Name,
                    Value = l.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(vn => vn.Villa_Number == villaNumberId)
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
            VillaNumber? dbVillaNumber = _unitOfWork.VillaNumber.Get(v => v.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (dbVillaNumber is not null)
            {
                _unitOfWork.VillaNumber.Remove(dbVillaNumber);
                _unitOfWork.SaveChanges();
                TempData["success"] = "Villa number deleted";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Villa number cannot be deleted";

            return View();
        }

    }
}
