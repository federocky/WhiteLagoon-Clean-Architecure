using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa villa)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Villa.Add(villa);
                _unitOfWork.SaveChanges();
                TempData["success"] = "Villa created";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Villa cannot be created";
            return View();
        }

        public IActionResult Update(int villaId)
        {
            Villa? villa = _unitOfWork.Villa.Get(x => x.Id == villaId);

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
                _unitOfWork.Villa.Update(villa);
                _unitOfWork.SaveChanges();
                TempData["success"] = "Villa has been updated";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Villa cannot be updated";
            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? villa = _unitOfWork.Villa.Get(x => x.Id == villaId);

            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? dbVilla = _unitOfWork.Villa.Get(v => v.Id == villa.Id);

            if (dbVilla is not null)
            {
                _unitOfWork.Villa.Remove(dbVilla);
                _unitOfWork.SaveChanges();
                TempData["success"] = "Villa deleted";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Villa cannot be deleted";

            return View();
        }

    }
}
