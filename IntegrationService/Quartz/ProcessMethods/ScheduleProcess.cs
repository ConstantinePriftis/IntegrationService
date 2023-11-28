using AutoMapper;
using IntegrationService.IServices.Ftp;
using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IQueries;
using IntegrationService.Models.Imports;
using IntegrationService.Services;
using IntegrationService.ViewModels.ImportViewModels;
using System.IO;
using System.Text;

namespace IntegrationService.Quartz.ProcessMethods
{
    public class ScheduleProcess
    {
        private readonly ICSVService _csvService;
        private readonly IFtpIntegrator _ftpIntegrator;
        private readonly IFieldProductStoreQuery _fieldProductStoreQuery;
        private readonly ICollectionCommand _collectionCommand;
        private readonly IMapper _mapper;
        private readonly IImportCommand _importCommand;
        private readonly IStoreCommand _storeCommand;
        private readonly IProductCharacteristicCommand _productCharacteristicCommand;
        private readonly IProductCommand _productCommand;
        public ScheduleProcess(
           ICSVService cSVService,
           IFtpIntegrator ftpIntegrator,
           //IFieldProductStoreQuery fieldProductStoreQuery,
           ICollectionCommand collectionCommand,
           IMapper mapper,
           IImportCommand importCommand,
           IStoreCommand storeCommand,
           IProductCharacteristicCommand productCharacteristicCommand,
           IProductCommand productCommand)
        {
            _csvService = cSVService;
            _ftpIntegrator = ftpIntegrator;
            // _fieldProductStoreQuery = fieldProductStoreQuery;
            _collectionCommand = collectionCommand;
            _mapper = mapper;
            _importCommand = importCommand;
            _storeCommand = storeCommand;
            _productCharacteristicCommand = productCharacteristicCommand;
            _productCommand = productCommand;
        }

        public void Execute()
        {
            try
            {
                var fileStream = _ftpIntegrator.GetFileStream();
                var imports = _csvService.ReadCSV<ImportViewModel>(fileStream, Encoding.GetEncoding(1253));
                var MappedImports = imports.Select(x => _mapper.Map<ImportViewModel, Import>(x)).ToList();
                _collectionCommand.InsertFromImport(MappedImports);
                _importCommand.Insert(MappedImports);
                _storeCommand.InsertFromImport(MappedImports);
                var differences = _productCharacteristicCommand.EvaluateDifferences();
                _productCommand.InsertFromImport(MappedImports);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}

