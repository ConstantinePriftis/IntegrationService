using IntegrationService.Models.Fields;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.FieldProductsViewModels;
using Newtonsoft.Json;

public static class SessionHelper
{
    private const string BasketKey = "Basket";

    public static void AddToBasket(FilterItem item, IHttpContextAccessor httpContextAccessor)
     {
        var basket = GetBasket(httpContextAccessor);
        var existingItem = basket.FirstOrDefault(x => x.Field == item.Field && x.Type == item.Type);
        if (item.Type == FilterItemEnum.DynamicSort)
        {
            foreach (var dynamicItem in basket.Where(x=>x.Type == FilterItemEnum.DynamicSort).ToList())
            {
                basket.Remove(dynamicItem);
            }
        }
        if (existingItem != null)
        {
            if(existingItem.Type != FilterItemEnum.StaticSort && existingItem.Type != FilterItemEnum.DynamicSort 
                && existingItem.Group != "FreeText" && existingItem.Group != "TrueOrFalse" && existingItem.Group != "Channel" && existingItem.Group != "Bool")
            {
				var newValues = existingItem.Value.Union(item.Value);
				item.Value = newValues.ToArray<string>();
			}
            
                basket.Remove(existingItem);
        }
        basket.Add(item);
        SaveBasket(basket, httpContextAccessor);

    }
    public static void RemoveFromBasket(FilterItem item, IHttpContextAccessor httpContextAccessor)
    {
        List<FilterItem> basketItems = GetBasket(httpContextAccessor);
        FilterItem itemToRemove = basketItems.FirstOrDefault(x => x.Field == item.Field && x.Type == item.Type);
        if (itemToRemove != null)
        {
            if(itemToRemove.Value.Count() == item.Value.Count())
            {
				basketItems.Remove(itemToRemove);
			}
            else
            {
				basketItems.Remove(itemToRemove);
				itemToRemove.Value = itemToRemove.Value.Except(item.Value).ToArray<string>();
				basketItems.Add(itemToRemove);
			}
            
            SaveBasket(basketItems, httpContextAccessor);
        }
    }

	public static void RemoveItem(FilterItem item, IHttpContextAccessor httpContextAccessor)
	{
		List<FilterItem> basketItems = GetBasket(httpContextAccessor);
		FilterItem itemToRemove = basketItems.FirstOrDefault(x => x.Field == item.Field && x.Type == item.Type);
		if (itemToRemove != null)
		{
			basketItems.Remove(itemToRemove);
			SaveBasket(basketItems, httpContextAccessor);
		}
	}
	public static List<FilterItem> GetBasket(IHttpContextAccessor httpContextAccessor)
    {
        var basketJson = httpContextAccessor.HttpContext.Session.GetString(BasketKey);
        if (basketJson == null)
        {
            return new List<FilterItem>();
        }
        return JsonConvert.DeserializeObject<List<FilterItem>>(basketJson);
    }

    public static int BasketCount(IHttpContextAccessor httpContextAccessor)
    {
        var count = GetBasket(httpContextAccessor).Count;
        return GetBasket(httpContextAccessor).Count;
    }

    private static void SaveBasket(List<FilterItem> basket, IHttpContextAccessor httpContextAccessor)
    {
        var basketJson = JsonConvert.SerializeObject(basket);
        httpContextAccessor.HttpContext.Session.SetString(BasketKey, basketJson);
    }
}
