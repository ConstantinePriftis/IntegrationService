using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegrationService.Data.DbContexts;
using IntegrationService.Models.Stores;
using IntegrationService.IServices.IQueries;
using IntegrationService.ConcreteServices.Queries;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IntegrationService.Controllers
{
    public class StoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStoreQuery _storeQuery;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoresController(ApplicationDbContext context, IStoreQuery storeQuery, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _storeQuery = storeQuery;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Stores
        public async Task<IActionResult> Index(int? page)
        {
            var filters = SessionHelper.GetBasket(_httpContextAccessor);
            var stores = await _storeQuery.GetPagedListAsync(page ?? 1, filters);
            return View(stores);
        }

        // GET: Stores/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Stores == null)
            {
                return NotFound();
            }

            var store = await _context.Stores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,Name,WarehouseID,ComesFromReflexes,Id,CreatedOn,ModifiedOn,Enabled")] Store store)
        {
            if (ModelState.IsValid)
            {
                store.Id = Guid.NewGuid();
                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        // GET: Stores/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Stores == null)
            {
                return NotFound();
            }

            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            return View(store);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,WarehouseID,Id")] Store store)
        {
            var storeUpdated = await _context.Stores.FindAsync(id);
            if (id != storeUpdated.Id)
            {
                return NotFound();
            }
            storeUpdated.Name = store.Name ?? string.Empty;
            _context.Update(storeUpdated);
            await _context.SaveChangesAsync();
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(store);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!StoreExists(store.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
                return RedirectToAction(nameof(Index));
            //}
            //return View(store);
        }

        // GET: Stores/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Stores == null)
            {
                return NotFound();
            }

            var store = await _context.Stores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Stores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Stores'  is null.");
            }
            var store = await _context.Stores.FindAsync(id);
            if (store != null)
            {
                _context.Stores.Remove(store);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(Guid id)
        {
          return _context.Stores.Any(e => e.Id == id);
        }
    }
}
