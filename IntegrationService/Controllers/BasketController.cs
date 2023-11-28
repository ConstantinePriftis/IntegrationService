using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IQueries;
using IntegrationService.IServices.IQuery;
using IntegrationService.Models.User;
using IntegrationService.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.Controllers
{
    public class BasketController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFieldsQuery _fieldsQuery;
        public BasketController(
            IFieldsQuery fieldsQuery,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _fieldsQuery = fieldsQuery;
        }
        public IActionResult Index(int dash)
        {
            return View();
        }

        public IActionResult GetBasketItems()
        {
            var items = SessionHelper.GetBasket(_httpContextAccessor);
            return Json(items);
        }
        [HttpPost]
        public IActionResult AddToBasket(FilterItem item , int dash)
        {

            SessionHelper.AddToBasket(item, _httpContextAccessor);
            var basketItems = SessionHelper.GetBasket(_httpContextAccessor);
            var url = dash == 20 ? Url.Action("Index", "FieldProductStores"): dash == 30 ? Url.Action("Index", "Nutritions") : dash == 10 ? Url.Action("Index", "FieldProducts"): dash == 40 ? Url.Action("Index", "Categories"): Url.Action("Index", "Stores");
            return Content(url);
        }
        [HttpPost]
        public IActionResult RemoveFromBasket(FilterItem item, int dash)
        {
            SessionHelper.RemoveFromBasket(item, _httpContextAccessor);
            var url = dash == 20 ? Url.Action("Index", "FieldProductStores") : dash == 30 ? Url.Action("Index", "Nutritions") : dash == 10 ? Url.Action("Index", "FieldProducts") : dash == 40 ? Url.Action("Index", "Categories") : Url.Action("Index", "Stores");
            return Content(url);
        }
        [HttpGet]
        public async Task<IActionResult> GetFieldValues(string id)
        {
            var predifinedValues = await _fieldsQuery.GetListAsync(x => x.Id == new Guid(id));
            var values = predifinedValues.SelectMany(x => x.PredefinedValues).Select(p => new { Value = p, Text = p }).OrderBy(x=>x.Value);
            return Json(values);

        }

        [HttpPost]
        public ActionResult RemoveSession(int dash)
        {
            var url = dash == 20 ? Url.Action("Index", "FieldProductStores") : dash == 30 ? Url.Action("Index", "Nutritions") : dash == 10 ? Url.Action("Index", "FieldProducts") : dash == 40 ? Url.Action("Index", "Categories") : Url.Action("Index", "Stores");
            _httpContextAccessor.HttpContext.Session.Clear();
            return Content(url);
        }

        public ActionResult ClearSession()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            return Ok();
        }
    }
}
