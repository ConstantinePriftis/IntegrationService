using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegrationService.Data.DbContexts;
using IntegrationService.Models.Categories;
using IntegrationService.IServices.ICommand;
using IntegrationService.Models.User;
using Microsoft.AspNetCore.Identity;
using IntegrationService.IServices.IQueries;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IntegrationService.IServices.IHttpFactoryService;
using IntegrationService.ConcreteServices.Queries;

namespace IntegrationService.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImportCommand _cmd;
        private UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientFactoryService _httpFactory;
        private readonly ICategoriesQuery _categoriesQuery;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoriesController(
            ApplicationDbContext context,
            IImportCommand cmd,
            UserManager<ApplicationUser> userManager,
            IHttpClientFactoryService httpFactory,
            ICategoriesQuery categoriesQuery,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _cmd = cmd;
            _userManager = userManager;
            _httpFactory = httpFactory;
            _categoriesQuery = categoriesQuery;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var filters = SessionHelper.GetBasket(_httpContextAccessor);
            var categories = await _categoriesQuery.GetPagedListAsync(page ?? 1, filters);
           // var categories = await _categoriesQuery.GetListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        //[HttpGet]
        //[Route("codes")]
        public async Task<IActionResult> GetCategories(string term)
        {
            var items = _categoriesQuery.GetSelectListItemsQueryable(term);

            return Ok(items);
        }

        public IActionResult Create()
        {
            //if (ModelState.IsValid)
            //{

            //}

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,Level,Description,Id,CreatedOn,ModifiedOn,Enabled")] Category category)
        {
            var user = await _userManager.GetUserAsync(User);
            Category cat = Category.Create(category, user);
            _context.Add(cat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Code,Level,Description,Id,CreatedOn,ModifiedOn,Enabled")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            var cat = await _context.Categories.FindAsync(id);

            if (cat != null)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    cat.Update(category, user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                }
            }
            else
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
