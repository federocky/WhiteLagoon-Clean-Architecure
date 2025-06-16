using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll(includeProperties: "Villa").ToList();
            return View(amenities);
        }

        public IActionResult Create()
        {
            AmenityVM amenityVM = new()     
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(l => new SelectListItem
                {
                    Text = l.Name,
                    Value = l.Id.ToString()
                })
            }; 

            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(amenityVM.Amenity);
                _unitOfWork.SaveChanges();
                TempData["success"] = "Amenity created";
                return RedirectToAction(nameof(Index));
            }

            amenityVM.VillaList = _unitOfWork.Villa.GetAll().Select(l => new SelectListItem
            {
                Text = l.Name,
                Value = l.Id.ToString()
            });

            return View(amenityVM);
        }

        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(l => new SelectListItem
                {
                    Text = l.Name,
                    Value = l.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(vn => vn.Id == amenityId)
            };
            if (amenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(amenityVM.Amenity);
                _unitOfWork.SaveChanges();
                TempData["success"] = "Amenity updated";
                return RedirectToAction(nameof(Index));
            }

            amenityVM.VillaList = _unitOfWork.Villa.GetAll().Select(l => new SelectListItem
            {
                Text = l.Name,
                Value = l.Id.ToString()
            });            

            return View(amenityVM);
        }

        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(l => new SelectListItem
                {
                    Text = l.Name,
                    Value = l.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(vn => vn.Id == amenityId)
            };
            if (amenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? dbAmenity = _unitOfWork.Amenity.Get(v => v.Id == amenityVM.Amenity.Id);

            if (dbAmenity is not null)
            {
                _unitOfWork.Amenity.Remove(dbAmenity);
                _unitOfWork.SaveChanges();
                TempData["success"] = "Amenity deleted";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Amenity cannot be deleted";

            return View();
        }

    }
}
