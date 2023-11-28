using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegrationService.Data.DbContexts;
using IntegrationService.Models.Fields;
using IntegrationService.Models.Product;
using IntegrationService.IServices.IQueries;
using IntegrationService.ConcreteServices.Queries;
using IntegrationService.ViewModels.FieldProductsViewModels;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using IntegrationService.Models.User;
using IntegrationService.IServices.ICommand;
using AutoMapper;
using IntegrationService.ViewModels.FieldProductStoresViewModels;
using IntegrationService.IServices.IRepository;
using Microsoft.AspNetCore.Authorization;

namespace IntegrationService.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class FieldProductStoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFieldProductStoreQuery _fieldProductStoreQuery;
        private UserManager<ApplicationUser> _userManager;
        private readonly IFieldProductStoreCommand _fieldProductStoreCommand;
        private readonly IStoreQuery _storeQuery;
        public FieldProductStoresController(ApplicationDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IFieldProductStoreQuery fieldProductStoreQuery,
            UserManager<ApplicationUser> userManager,
            IFieldProductStoreCommand fieldProductStoreCommand,
            IStoreQuery storeQuery)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _fieldProductStoreQuery = fieldProductStoreQuery;
            _userManager = userManager;
            _fieldProductStoreCommand = fieldProductStoreCommand;
            _storeQuery = storeQuery;
        }

        // GET: FieldProductStores
        public async Task<IActionResult> Index(int? page, string sortOrder, string sortDirection)
        {
            var filters = SessionHelper.GetBasket(_httpContextAccessor);
            var fieldsPerProduct = await _fieldProductStoreQuery.GetPagedListAsync(page ?? 1, filters);

            return View(fieldsPerProduct);
        }
        public async Task<IActionResult> GetStores(string term)
        {
			var filters = SessionHelper.GetBasket(_httpContextAccessor);
			var items = _storeQuery.GetSelectListItemsQueryable(term, filters);

            return Ok(items);
        }
        // GET: FieldProductStores/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.FieldProductStores == null)
            {
                return NotFound();
            }

            var fieldProductStore = await _context.FieldProductStores
                .Include(f => f.Field)
                .Include(f => f.ProductStores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fieldProductStore == null)
            {
                return NotFound();
            }

            return View(fieldProductStore);
        }

        // GET: FieldProductStores/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var fieldProduct = await _fieldProductStoreQuery.GetByProductStoreIdAsync(id);

            if (fieldProduct == null)
            {
                return NotFound();
            }
            return View(fieldProduct);
        }

        // POST: FieldProductStores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,bool IsPublished, List<string> fields)
        {
            var user = await _userManager.GetUserAsync(User);
            var list = new List<FieldsPerProductStoreEditViewModel>();
            fields.Where(x=> x!=null).ToList().ForEach(x => list.Add(JsonConvert.DeserializeObject<FieldsPerProductStoreEditViewModel>(x) ?? new FieldsPerProductStoreEditViewModel { }));
            var fieldProductStores = list.Select(x => _mapper.Map<FieldsPerProductStoreEditViewModel, FieldProductStore>(x));
            await _fieldProductStoreCommand.InsertRange(fieldProductStores, IsPublished, user);

            return RedirectToAction(nameof(Index));
        }

        // GET: FieldProductStores/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.FieldProductStores == null)
            {
                return NotFound();
            }

            var fieldProductStore = await _context.FieldProductStores
                .Include(f => f.Field)
                .Include(f => f.ProductStores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fieldProductStore == null)
            {
                return NotFound();
            }

            return View(fieldProductStore);
        }

        // POST: FieldProductStores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.FieldProductStores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.FieldProductStores'  is null.");
            }
            var fieldProductStore = await _context.FieldProductStores.FindAsync(id);
            if (fieldProductStore != null)
            {
                _context.FieldProductStores.Remove(fieldProductStore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FieldProductStoreExists(Guid id)
        {
          return _context.FieldProductStores.Any(e => e.Id == id);
        }
    }
}
