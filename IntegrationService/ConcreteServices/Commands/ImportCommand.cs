using IntegrationService.Services.Repository;
using IntegrationService.Services;
using IntegrationService.Models.Imports;
using IntegrationService.ViewModels;
using AutoMapper;
using IntegrationService.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using IntegrationService.IServices.ICommand;
using Quartz;

namespace IntegrationService.ConcreteServices.Commands
{
    public class ImportCommand : IImportCommand
    {
        private readonly IImportRepository _importRepository;
        public ImportCommand(IImportRepository importRepository)
        {
            _importRepository = importRepository;
        }

        public void Insert(List<Import> imports)
        {
            var db = _importRepository.GetDb();
            var context = _importRepository.GetContext();


            var ImportsCount = _importRepository.GetAll().Count();
            if (ImportsCount > 0)
            {
                var Init = db.ExecuteSqlRaw("DECLARE @RC int EXEC @RC = [dbo].[InitializeImports]");
                var delete = db.ExecuteSqlRaw("DELETE FROM [dbo].[Imports]");
                _importRepository.AddRange(imports);
                _importRepository.Save();
            }
            else
            {
                _importRepository.AddRange(imports);
                _importRepository.Save();
            }
        }

        public void Insert(Import item)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(Import item)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(List<Import> collection)
        {
            throw new NotImplementedException();
        }
        public int SaveChanges()
        {
            return _importRepository.Save();
        }
    }
}
