using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.ViewModels;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService service, DepartmentService departmentService)
        {
            _sellerService = service;
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel
            {
                Departments = departments
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var seller = await _sellerService.FindByIdAsync(id);

            if (seller == null) return RedirectToAction(nameof(Error), new { message = "Id not found" });

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            var seller = await _sellerService.FindByIdAsync(id);

            if (seller == null)
                return RedirectToAction(nameof(Error), new { message = "Id not found" });

            return View(seller);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            var seller = await _sellerService.FindByIdAsync(id);

            if (seller == null)
                return RedirectToAction(nameof(Error), new { message = "Id not found" });

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (id != seller.Id)
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });

            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }
            catch (DbConcurrentException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }

        }

        public IActionResult Error(string Message)
        {
            ErrorViewModel viewModel = new ErrorViewModel
            {
                Message = Message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }
    }
}
