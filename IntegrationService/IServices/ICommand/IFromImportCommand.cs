using IntegrationService.Models.Imports;

namespace IntegrationService.IServices.ICommand
{
    public interface IFromImportCommand
    {
        void InsertFromImport(List<Import> collection);
    }
}
