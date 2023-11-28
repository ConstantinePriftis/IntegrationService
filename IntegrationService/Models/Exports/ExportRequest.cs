using IntegrationService.Models.EntityBases;
using IntegrationService.Models.User;
using IntegrationService.ViewModels.FieldProductsViewModels;
using IntegrationService.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Protocol;
using OfficeOpenXml;
using Quartz.Impl.Triggers;
using System.Reflection.PortableExecutable;

namespace IntegrationService.Models.Exports
{
    public class ExportRequest : BaseEntity
    {
        public ExportRequest()
        {

        }
        private ExportRequest(ApplicationUser applicationUser)
        {
            var exportRequest = new ExportRequest();
            exportRequest.Id = Guid.NewGuid();
            exportRequest.CreatedBy = applicationUser;
            exportRequest.CreatedOn = DateTime.Now;
            exportRequest.ModifiedBy = applicationUser;
            exportRequest.ModifiedOn = DateTime.Now;

        }

        public static ExportRequest Create(ApplicationUser applicationUser)
        {
            var exportRequest = new ExportRequest();
            exportRequest.Id = Guid.NewGuid();
            exportRequest.CreatedBy = applicationUser;
            exportRequest.CreatedOn = DateTime.Now;
            exportRequest.ModifiedBy = applicationUser;
            exportRequest.ModifiedOn = DateTime.Now;
            exportRequest.Enabled = true;
            //var exportRequest =  new ExportRequest(applicationUser);
            return exportRequest;
        }
    }
}

