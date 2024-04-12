using BTLWEB.Models;
using BTLWEB.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BTLWEB.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRespository _categoryRepo;

        public CategoryController(ICategoryRespository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepo.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category model) 
        {
            try
            {
                await _categoryRepo.Add(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepo.GetById(id);
            return View(category);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            try
            {
                await _categoryRepo.Update(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _categoryRepo.GetById(id);
            if(category != null)
            {
                return View(category);
            }
            TempData["errorMessage"] = $"Category details not found with Id: {id}";
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryRepo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
