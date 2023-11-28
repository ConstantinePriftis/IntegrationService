using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegrationService.Data.DbContexts;
using IntegrationService.Models.Fields;
using IntegrationService.IServices.IQueries;
using System.Text.Json.Nodes;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.FieldProductsViewModels;
using System.Text.Json;
using Newtonsoft.Json;
using AutoMapper;
using IntegrationService.IServices.ICommand;
using Microsoft.AspNetCore.Identity;
using IntegrationService.Models.User;
using IntegrationService.IServices.IQuery;
using IntegrationService.ConcreteServices.Queries;
using IntegrationService.Models;
using IntegrationService.ConcreteServices.Commands;
using IntegrationService.Models.Errors;
using Microsoft.AspNetCore.Authorization;
using IntegrationService.ViewModels.ProductViewModels;
using IntegrationService.ViewModels.FieldViewModels;
using IntegrationService.Models.Product;

namespace IntegrationService.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class FieldProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFieldProductQuery _fieldProductQuery;
        private readonly IFieldProductCommand _fieldProductCommand;
        private UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFieldsQuery _fieldsQuery;
        private readonly IProductsQuery _productsQuery;
        private readonly IErrorCommand _errorCommand;

        public FieldProductsController(ApplicationDbContext context,
            IMapper mapper,
            IFieldProductQuery fieldProductQuery,
            IFieldProductCommand fieldProductCommand,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IFieldsQuery fieldsQuery,
            IProductsQuery productsQuery,
            IErrorCommand errorCommand)
        {
            _context = context;
            _fieldProductQuery = fieldProductQuery;
            _mapper = mapper;
            _fieldProductCommand = fieldProductCommand;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _fieldsQuery = fieldsQuery;
            _productsQuery = productsQuery;
            _errorCommand = errorCommand;
        }

        // GET: FieldProducts
        public async Task<IActionResult> Index(int? page)
        {

            try
            {
                var filters = SessionHelper.GetBasket(_httpContextAccessor);
                var fieldsPerProduct = await _fieldProductQuery.GetPagedListAsync(page ?? 1, filters);
                return View(fieldsPerProduct);
            }
            catch (Exception ex)
            {
                await _errorCommand.Insert(Error.Create(ex.Message), null, null);
                //string errorMessage = "An error occurred while applying migrations. Please contact the administrator.";

                return View(ex.Message);
                //return View("Error", new ErrorViewModel { Message = errorMessage });
            }
        }
        public async Task<IActionResult> GetProducts(string term)
        {
			var basketItems = SessionHelper.GetBasket(_httpContextAccessor);
			var items = _productsQuery.GetSelectListItemsQueryable(term, basketItems);

            return Ok(items);
        }
        public async Task<IActionResult> GetProductById(string  sku)
        {
            try
            {

                var items = await _fieldProductQuery.GetBySku(sku);
                var field = items.Fields.Where(x => x.FieldName == "Title").FirstOrDefault();
                return Ok(field.Value);
            }
            catch (Exception ex)
            {
                _errorCommand.Insert(Error.Create(ex.Message),null,null);
                throw ex;
            }
        }
        // GET: FieldProducts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.FieldProducts == null)
            {
                return NotFound();
            }

            var fieldProducts = await _context.FieldProducts
                .Include(f => f.Field)
                .Include(f => f.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fieldProducts == null)
            {
                return NotFound();
            }

            return View(fieldProducts);
        }


        public async Task<IActionResult> Edit(Guid? id)
        {
            var fieldProduct = await _fieldProductQuery.GetByProductIdAsync(id);

            if (fieldProduct == null)
            {
                return NotFound();
            }
            return View(fieldProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductsViewModel product,
            //Guid id,string descriptionCategoryName, bool isCompleted,
            List<string> fields)
        {
            var user = await _userManager.GetUserAsync(User);
            Products prod = _mapper.Map<ProductsViewModel, Products>(product);
            var list = new List<FieldsPerProductEditViewModel>();
            fields.ForEach(x => list.Add(JsonConvert.DeserializeObject<FieldsPerProductEditViewModel>(x) ?? new FieldsPerProductEditViewModel { }));
            var fieldProducts = list.Select(x => _mapper.Map<FieldsPerProductEditViewModel, FieldProducts>(x));
            await _fieldProductCommand.InsertRange(fieldProducts, prod, user);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.FieldProducts == null)
            {
                return NotFound();
            }

            var fieldProducts = await _context.FieldProducts
                .Include(f => f.Field)
                .Include(f => f.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fieldProducts == null)
            {
                return NotFound();
            }

            return View(fieldProducts);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.FieldProducts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.FieldProducts'  is null.");
            }
            var fieldProducts = await _context.FieldProducts.FindAsync(id);
            if (fieldProducts != null)
            {
                _context.FieldProducts.Remove(fieldProducts);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FieldProductsExists(Guid id)
        {
            return _context.FieldProducts.Any(e => e.Id == id);
        }
    }
}
