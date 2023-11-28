using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegrationService.Data.DbContexts;
using IntegrationService.Models.Fields;
using IntegrationService.IServices.ICommand;
using IntegrationService.Models.Imports;
using IntegrationService.Models.Channels;
using Microsoft.AspNetCore.Identity;
using IntegrationService.Models.User;
using IntegrationService.IServices.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using IntegrationService.IServices.IQuery;
using IntegrationService.IServices.IQueries;
using Microsoft.AspNetCore.Mvc.Filters;
using IntegrationService.ViewModels.FieldViewModels;
using IntegrationService.IServices.IHttpFactoryService;
using IntegrationService.IServices.Excel;
using System.Linq.Expressions;

namespace IntegrationService.Controllers
{
    //[Route("api/fields/{userId:guid}/[controller]", Order = 0)]
    //[ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class FieldsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFieldCommand _fieldCommand;
        private UserManager<ApplicationUser> _userManager;
        private readonly IFieldsQuery _fieldsQuery;
        private readonly IChannelsQuery _channelsQuery;
        private readonly IProductsQuery _productsQuery;
        private readonly IDOMGroupsQuery _dOMGroupsQuery;
        private readonly IFieldProductQuery _fieldProductQuery;
        private readonly IExcelService _excelService;

        public FieldsController(ApplicationDbContext context,IMapper mapper,
            IFieldCommand fieldCommand,
            UserManager<ApplicationUser> userManager,
            IFieldsQuery fieldsQuery,
            IChannelsQuery channelsQuery,
            IProductsQuery productsQuery,
            IDOMGroupsQuery dOMGroupsQuery,
            IFieldProductQuery fieldProductQuery,
            IExcelService excelService)
        {
            _context = context;
            _mapper = mapper;   
            _fieldCommand = fieldCommand;
            _userManager = userManager;
            _fieldsQuery = fieldsQuery;
            _channelsQuery = channelsQuery;
            _productsQuery = productsQuery;
            _dOMGroupsQuery = dOMGroupsQuery;
            _fieldProductQuery = fieldProductQuery;
            _excelService = excelService;
        }

        // GET: Fields
        //[HttpGet("fields")]

        public async Task<IActionResult> Index(Dictionary<string,string> filters)
        {
            var fieldsPerProduct = (await _fieldProductQuery.GetListAsync()).ToList().SelectMany(x => x.Fields).Select(x =>
            new
            {
                //Sku = x.

            }

            );
            //try
            //{
            //    var data = new Dictionary<string, List<DOMGroupsViewModel>>();

            //    //var products = await _productsQuery.GetListAsync();
            //    var groups = await _dOMGroupsQuery.GetListAsync();
            //    byte[] bytes = new byte[0];
            //    data.Add(nameof(DOMGroupsViewModel), groups);
            //    //var keys = filters.Keys.Select(x => x.Split("-")).ToList();

            var filters1 = filters;
            var model = await _fieldsQuery.GetListAsync();
            //    foreach (var item in data)
            //    {
            //        bytes = _excelService.WriteToExcel(data);
            //    }
            //    return File(bytes, "application/ms-excel", "ReportFile.xlsx");
            //}
            //catch (Exception ex)
            //{
            //    var test = ex;
            //    throw ex;
            //}

            return View(model);
            // return Ok(model);
        }

        // GET: Fields/Create
        public async Task<IActionResult> Create()
        {
            //var channels = await _productsQuery.GetSelectListItems();
            //var vm = new FieldCreateViewModel() { Channels = channels};
            //return View(vm);
            return View();
        }

        // POST: Fields/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
        //[Bind("Name,Type,Id,CreatedOn,ModifiedOn,Enabled")]
        FieldCreateViewModel vm)
        {
            List<Guid> ids = vm.SelectedChannels.ToList();
            var user = await _userManager.GetUserAsync(User);
            var field = _mapper.Map<FieldCreateViewModel, Field>(vm);
            await _fieldCommand.Insert(field, user, ids);
            return RedirectToAction(nameof(Index));
        }

        // GET: Fields/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Fields == null)
            {
                return NotFound();
            }

            var @field = await _context.Fields.FindAsync(id);
            if (@field == null)
            {
                return NotFound();
            }
            return View(@field);
        }

        // POST: Fields/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Type,Id,CreatedOn,ModifiedOn,Enabled")] Field @field)
        {
            var user = await _userManager.GetUserAsync(User);
            await _fieldCommand.Update(id,field, user, new List<Guid> { });
            
            return RedirectToAction(nameof(Index));
            //return View(@field);
        }

        // GET: Fields/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Fields == null)
            {
                return NotFound();
            }

            var @field = await _context.Fields
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@field == null)
            {
                return NotFound();
            }

            return View(@field);
        }
        // GET: Fields/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Fields == null)
            {
                return NotFound();
            }

            var @field = await _context.Fields
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@field == null)
            {
                return NotFound();
            }

            return View(@field);
        }

        // POST: Fields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Fields == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Fields'  is null.");
            }
            var @field = await _context.Fields.FindAsync(id);
            if (@field != null)
            {
                _context.Fields.Remove(@field);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FieldExists(Guid id)
        {
          return _context.Fields.Any(e => e.Id == id);
        }
    }
}
