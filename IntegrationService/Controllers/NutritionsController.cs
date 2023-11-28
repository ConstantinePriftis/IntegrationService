using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegrationService.Data.DbContexts;
using IntegrationService.Models.Nutritions;
using IntegrationService.IServices.IQueries;
using IntegrationService.ConcreteServices.Queries;
using IntegrationService.Models;
using IntegrationService.ViewModels.Nutrition;
using NonFactors.Mvc.Grid;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AutoMapper;
using IntegrationService.ViewModels.ProductViewModels;
using IntegrationService.Models.Product;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IntegrationService.IServices.ICommand;
using IntegrationService.Models.User;
using Microsoft.AspNetCore.Identity;
using IntegrationService.Models.Errors;

namespace IntegrationService.Controllers
{
    public class NutritionsController : Controller
    {
        private readonly IProductsQuery _productsQuery;
        private readonly IFieldProductQuery _fieldProductQuery;
        private readonly INutritionQuery _nutritionQuery;
        private readonly INutritionCommand _nutritionCommand;
        private readonly IErrorCommand _errorCommand;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NutritionsController(ApplicationDbContext context,
            IProductsQuery productsQuery,
            IFieldProductQuery fieldProductQuery,
            INutritionQuery nutritionQuery,
            INutritionCommand nutritionCommand,
            UserManager<ApplicationUser> userManager,
            IErrorCommand errorCommand,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _productsQuery = productsQuery;
            _fieldProductQuery = fieldProductQuery;
            _nutritionQuery = nutritionQuery;
            _nutritionCommand = nutritionCommand;
            _errorCommand = errorCommand;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Nutritions
        public async Task<IActionResult> Index(int? page)
        {
            var filters = SessionHelper.GetBasket(_httpContextAccessor);
            var nutritions = await _nutritionQuery.GetPagedListAsync(page ?? 1, filters);
            return View(nutritions);
        }

        // GET: Nutritions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var nutritionsBySku = _context.Nutritions.Where(x => x.ProductId == id).OrderBy(x => x.Order).ToList();
            return nutritionsBySku.Count() > 0 ? View(nutritionsBySku) : NotFound();

            //var nutrition = await _context.Nutritions.FirstOrDefaultAsync(m => m.Id == id);
            //if (nutrition == null)
            //{
            //    return NotFound();
            //}

            //return View(nutrition);
        }

        // GET: Nutritions/Create
        public async Task<IActionResult> Create(string? sku)
        {
            var nutQuery = await _nutritionQuery.GetQueryableNutritions();
            var nutritions = nutQuery.Where(x => x.Sku == sku).ToList();
            return View(nutritions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Sku, List<Nutrition> nutritions)
        {
            try
            {
                string validationMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(Sku))
                {
                    _errorCommand.Insert(Error.Create("Sku Cannot be null"), await _userManager.GetUserAsync(User), nutritions.Select(x => x.Id).ToList());
                    return BadRequest("Sku cannot be null");
                }
                var firstUnValidated = nutritions.FirstOrDefault(x => x.Order <= 0);
                if (firstUnValidated != null) { return BadRequest("Validation Failed for title : " + firstUnValidated.Title); }



                _nutritionCommand.InsertRange(Sku, nutritions, out validationMessage);
                if (!string.IsNullOrWhiteSpace(validationMessage)) return BadRequest("Validation Failed for title : " + validationMessage);
            }
            catch (Exception ex)
            {
                _errorCommand.Insert(Error.Create(ex.Message), await _userManager.GetUserAsync(User), nutritions.Select(x => x.Id).ToList());
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            //var prod = await _productsQuery.GetListAsync(x=>x.Id == id);
            //var sku = prod.FirstOrDefault().Sku;
            var nutritionsBySku = _context.Nutritions.Where(x => x.ProductId == id).OrderBy(x=>x.Order).ToList();
            return nutritionsBySku.Count() > 0 ? View(nutritionsBySku) : NotFound();
        }
        public async Task<IActionResult> GetNutritions(string sku)
        {
            var items = await _nutritionQuery.GetQueryableNutritions();
            return Ok(items.Where(x => x.Sku == sku));
        }
        public async Task<IActionResult> PostNutritions(string serializedJson)
        {
            var items = await _nutritionQuery.GetQueryableNutritions();
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, string Sku, List<Nutrition> nutritions)
        {
            var user = await _userManager.GetUserAsync(User);
            if (nutritions.Count == 0)
                throw new ArgumentException("");
            if (string.IsNullOrWhiteSpace(id.ToString()))
                throw new ArgumentException("");
            if (string.IsNullOrWhiteSpace(Sku))
                throw new ArgumentException("");
            string validationMessage = string.Empty;
            try
            {
                //if (validationMessage != string.Empty)
                //    return BadRequest("Validation failed for nutrition with title : " + validationMessage);
                var result = await _nutritionCommand.UpdateRange(nutritions, user, null);
                if (result == 0)
                    return BadRequest("Items were not updated");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _errorCommand.Insert(Error.Create(ex.Message), user, null);
                throw ex;
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> GetProductsWithoutNutritions(string term)
        {
            var basketItems = SessionHelper.GetBasket(_httpContextAccessor);
            var items = _nutritionQuery.GetSelectListProductsWithoutNutritionQueryable(term, basketItems);

            return Ok(items);
        }
        // GET: Nutritions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Nutritions == null)
            {
                return NotFound();
            }

            var nutrition = await _context.Nutritions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nutrition == null)
            {
                return NotFound();
            }

            return View(nutrition);
        }

        // POST: Nutritions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Nutritions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Nutritions'  is null.");
            }
            var nutrition = await _context.Nutritions.FindAsync(id);
            if (nutrition != null)
            {
                _context.Nutritions.Remove(nutrition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NutritionExists(Guid id)
        {
            return (_context.Nutritions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
