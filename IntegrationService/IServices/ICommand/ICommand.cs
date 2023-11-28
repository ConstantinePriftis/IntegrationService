using IntegrationService.Models.EntityBases;
using IntegrationService.Models.Imports;
using IntegrationService.ViewModels;

namespace IntegrationService.IServices.ICommand
{
    public interface ICommand<T1> where T1 : class, new() 
    {
        void Insert(T1 item);
        void Insert(List<T1> collection);
        void InsertOrUpdate(T1 item);
        void InsertOrUpdate(List<T1> collection);
        int SaveChanges();
    }
}
