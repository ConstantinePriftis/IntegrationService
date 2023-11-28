using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Errors;
using IntegrationService.Models.Nutritions;
using IntegrationService.Models.Product;
using IntegrationService.Models.User;
using IntegrationService.ViewModels.ProductViewModels;

namespace IntegrationService.ConcreteServices.Commands
{
	public class NutritionCommand : INutritionCommand
	{
		private readonly INutritionRepository _nutritionRepository;
		private readonly IErrorRepository _errorRepository;
		private readonly IProductsRepository _productsRepository;

		public NutritionCommand(
			INutritionRepository nutritionRepository,
			IErrorRepository errorRepository,
			IProductsRepository productsRepository)
		{
			_nutritionRepository = nutritionRepository;
			_errorRepository = errorRepository;
			_productsRepository = productsRepository;
		}
		public Task<int> Insert(Nutrition item, ApplicationUser user, List<Guid>? items)
		{
			if (item.Order >= 0)
			{
				try
				{
					if (item == null)
					{
						throw new ArgumentNullException("item");
					}
					if (user == null)
					{
						throw new ArgumentNullException("user");
					}
					var nutrition = Nutrition.Create(
						item.Sku,
						item.Title,
						item.FirstCell,
						item.SecondCell,
						item.ThirdCell,
						item.FourthCell,
						item.IsHighlight,
						item.Order,
						item.IsBold,
						item.Note,
						item.ProductId);
					_nutritionRepository.Add(nutrition);

				}
				catch (Exception ex)
				{
					_errorRepository.Add(Error.Create(ex.Message));
					_errorRepository.Save();
				}
			}
			return Task.FromResult(_nutritionRepository.Save());
		}

		public void InsertFromImport(List<Nutrition> collection)
		{
			try
			{
				foreach (var item in collection)
				{
					var productFound = _productsRepository.FindBy(x => x.Sku == item.Sku).FirstOrDefault();
					if (productFound != null)
					{
						item.Update(item, productFound.Id);
						_nutritionRepository.Add(item);
					}
				}
				_nutritionRepository.Save();
			}
			catch (Exception ex)
			{
				_errorRepository.Add(Error.Create(ex.Message));
				_errorRepository.Save();
			}
		}



		public void InsertRange(string sku, List<Nutrition> nutritions, out string validationMessage)
		{
			validationMessage = string.Empty;
			nutritions = nutritions.Where(x => x.Order > 0).ToList();
			try
			{

				var relatedProductTask = _productsRepository.FindBy(x => x.Sku == sku).FirstOrDefault();
				// var relatedProduct = relatedProductTask.FirstOrDefault(x => x.Sku == Sku);
				if (relatedProductTask == null)
				{
					validationMessage = "Product cannot be null";
					return;
				}
				foreach (var item in nutritions.Where(x => x.Order > 0).ToList())
				{
					item.Sku = sku;
					//item.Product = relatedProductTask;
					item.ProductId = relatedProductTask.Id;
					item.CreatedOn = DateTime.Now;
					item.ModifiedOn = DateTime.Now;
				}

				foreach (var item in nutritions)
				{

					var isValid = item.IsValid(out validationMessage);
					if (!isValid && !string.IsNullOrWhiteSpace(validationMessage))
						return;

					_nutritionRepository.Add(item);
					_nutritionRepository.Save();
				}
			}
			catch (Exception ex)
			{
				validationMessage = ex.Message;
				_errorRepository.Add(Error.Create(ex.Message));
				_errorRepository.Save();
			}

		}

		public Task<int> InsertRange(IEnumerable<Nutrition> items, ApplicationUser user)
		{
			throw new NotImplementedException();
		}

		public Task<int> UpdateRange(List<Nutrition> nutritions, ApplicationUser user, List<Guid>? items)
		{
			var test = nutritions.Where(x => x.Order > 0).ToList();

            try
			{
				foreach (var item in nutritions.Where(x => x.Order > 0).ToList())
				{
					var storedNutrition = _nutritionRepository.FindBy(x => x.Id == item.Id).FirstOrDefault();
					if (storedNutrition == null)
					{
						_nutritionRepository.Add(item);
					}
					else
					{
						var storedRelatedProduct = _productsRepository.FindBy(x => x.Sku == storedNutrition.Sku).FirstOrDefault();
						if (storedRelatedProduct == null)
							throw new ArgumentNullException(nameof(storedRelatedProduct));
						storedNutrition.Update(item, storedRelatedProduct.Id);
						_nutritionRepository.Update(storedNutrition);
					}
				}

				return Task.FromResult(_nutritionRepository.Save());
			}
			catch (Exception ex)
			{
				_errorRepository.Add(Error.Create(ex.Message));
				_errorRepository.Save();
				return Task.FromResult(0);
			}
		}

		public Task<int> UpdateNutrition(Guid nutritionId, Nutrition nutrition, Products product)
		{
			try
			{
				var storedNutrition = _nutritionRepository.FindBy(x => x.Id == nutritionId).FirstOrDefault();
				if (storedNutrition != null)
				{
					storedNutrition.Update(storedNutrition, product.Id);
				}

				var savedItems = _nutritionRepository.Save();
				return Task.FromResult(savedItems);

			}
			catch (Exception ex)
			{
				_errorRepository.Add(Error.Create(ex.Message));
				_errorRepository.Save();
				return Task.FromResult(0);
			}
		}

		void INutritionCommand.Insert(Nutrition item, ApplicationUser user, List<Guid>? items)
		{
			throw new NotImplementedException();
		}

		public Task<int> Update(Guid id, Nutrition item, ApplicationUser user, List<Guid>? items)
		{
			throw new NotImplementedException();
		}

		public void InsertRange(string sku, List<Nutrition> nutritions)
		{
			throw new NotImplementedException();
		}
	}
}
