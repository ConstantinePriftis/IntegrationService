namespace IntegrationService.ViewModels.CategoryViewModels
{
    public interface ICategoryViewModel
    {
        string Code { get; set; }
        string Description { get; set; }
        Guid Id { get; set; }
        int Level { get; set; }
    }
}