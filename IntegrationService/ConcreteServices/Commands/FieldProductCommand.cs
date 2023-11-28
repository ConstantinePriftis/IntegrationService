using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Fields;
using IntegrationService.Models.Product;
using IntegrationService.Models.User;
using Microsoft.EntityFrameworkCore;

namespace IntegrationService.ConcreteServices.Commands
{
    public class FieldProductCommand : IFieldProductCommand
    {
        private readonly IFieldProductRepository _fieldProductRepository;
        private readonly IProductsRepository _productsRepository;
        public FieldProductCommand(IFieldProductRepository fieldProductRepository, IProductsRepository productsRepository)
        {
            _fieldProductRepository = fieldProductRepository;
            _productsRepository = productsRepository;

        }

        public async Task<int> InsertRange(IEnumerable<FieldProducts> items, Products product, ApplicationUser user)
        {
            var fieldProducts = new List<FieldProducts>();
            var productToModify = await _productsRepository.FindBy(x => x.Id == product.Id).FirstOrDefaultAsync();
            foreach (var item in items)
            {
                var fieldProduct = await _fieldProductRepository.FindBy(x => x.ProductsId == item.ProductsId && x.FieldId == item.FieldId).FirstOrDefaultAsync();
                
                if (fieldProduct != null)
                {
                    fieldProduct.UpdateValue(item.Value);
                }
                else
                {
                    fieldProducts.Add(FieldProducts.Create(item.FieldId, item.ProductsId, item.Value));
                }

               
            }
            if (productToModify != null)
            {
                productToModify.UpdateModified();
                if (product.Enabled)
                {
                    productToModify.Completed();
                }
                else
                {
                    productToModify.UpdateEnabled();
                }

                productToModify.Description_Category = product.Description_Category;
            }
            _fieldProductRepository.AddRange(fieldProducts);
            return _fieldProductRepository.Save();
        }

        public Task<int> Insert(FieldProducts item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertRange(IEnumerable<FieldProducts> items, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(Guid id, FieldProducts item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }
    }
}
