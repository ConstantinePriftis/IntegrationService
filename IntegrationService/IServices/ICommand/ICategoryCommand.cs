using IntegrationService.Models.Categories;

namespace IntegrationService.IServices.ICommand
{
    public interface ICategoryCommand : ICommandAsync<Category>
    {
        void InsertRange(List<Category> categories);
        void InsertFromImport(List<Category> collection);
    }
}
