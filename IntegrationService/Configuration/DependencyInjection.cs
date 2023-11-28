using AutoMapper;
using IntegrationService.ConcreteRepository;
using IntegrationService.ConcreteServices;
using IntegrationService.ConcreteServices.Commands;
using IntegrationService.ConcreteServices.ConcreteRepository;
using IntegrationService.ConcreteServices.Ftp;
using IntegrationService.ConcreteServices.Query;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.Ftp;
using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IQuery;
using IntegrationService.IServices.IRepository;
using IntegrationService.Profiles;
using IntegrationService.Quartz.Jobs;
using IntegrationService.Quartz;
using IntegrationService.Services;
using IntegrationService.Services.Repository;
using Microsoft.EntityFrameworkCore;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using System.Collections.Specialized;
using IntegrationService.IServices.IQueries;
using IntegrationService.ConcreteServices.Queries;
using IntegrationService.IServices.Excel;
using IntegrationService.Models.Imports;
using IntegrationService.IServices.IHttpFactoryService;
using IntegrationService.ConcreteServices.HttpFactory;
using IntegrationService.ViewModels.FieldViewModels;
using Microsoft.Extensions.DependencyInjection.Extensions;
using IntegrationService.Quartz.ProcessMethods;

namespace IntegrationService.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCustomConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<Configurations>();

            return services;
        }
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("IntegrationService");
            //builder.Services.AddDbContext<IntegrationServiceDbContext>(options =>
            //    options.UseSqlServer(connectionString));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(60000)));

            return services;
        }

        public static IServiceCollection AddCustomControllerServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            //services.AddSingleton(typeof(IDatatableConvertorService<>), typeof(DatatableConvertorService<>));
            services.AddScoped<IHttpClientFactoryService, HttpFactoryService>();
            services.AddTransient<ICSVService, CSVService>();
            services.AddTransient<IExcelService, ExcelService>();
            services.AddTransient<IFtpIntegrator, FtpIntegrator>();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromSeconds(3600); });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(mapperConfig.CreateMapper());

            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductStoreRepository, ProductStoreRepository>();
            services.AddScoped<IImportRepository, ImportRepository>();
            services.AddScoped<IProductCharacteristicRepository, ProductCharacteristicRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IFieldRepository, FieldRepository>();
            services.AddScoped<IChannelRepository, ChannelRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IDOMGroupsRepository, DOMGroupsRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IFieldProductRepository, FieldProductRepository>();
            services.AddScoped<IFieldProductStoreRepository, FieldProductStoreRepository>();
            services.AddScoped<IChannelProductStoreRepository, ChannelProductStoreRepository>();
            services.AddScoped<ICollectionRepository, CollectionRepository>();
            services.AddScoped<IErrorRepository, ErrorRepository>();
            services.AddScoped<INutritionRepository, NutritionRepository>();
            services.AddScoped<IFieldPerProductViewModelRepository, FieldPerProductViewModelRepository>();
            services.AddScoped<IExportRequestRepository, ExportRequestRepository>();


            return services;
        }

        public static IServiceCollection AddCqrs(this IServiceCollection services)
        {
            services.AddScoped<IImportCommand, ImportCommand>();
            services.AddScoped<IFieldCommand, FieldCommand>();
            services.AddScoped<IProductCommand, ProductCommand>();
            services.AddScoped<IStoreCommand, StoreCommand>();
            services.AddScoped<ICategoryCommand, CategoryCommand>();
            services.AddScoped<IProductCharacteristicCommand, ProductCharacteristicCommand>();
            services.AddScoped<INutritionCommand, NutritionCommand>();
            services.AddScoped<IExportRequestCommand, ExportRequestCommand>();
            services.AddScoped<IErrorCommand, ErrorCommand>();
            services.AddScoped<IFieldProductStoreQuery, FieldProductStoreQuery>();
            services.AddScoped<IFieldProductCommand,FieldProductCommand>();
            services.AddScoped<IFieldProductStoreCommand, FieldProductStoreCommand>();
            services.AddScoped<ICollectionCommand, CollectionCommand>();

            services.AddScoped<IFieldsQuery, FieldsQuery>();
            services.AddScoped<IChannelsQuery, ChannelsQuery>();
            services.AddScoped<IProductsQuery, ProductsQuery>();
            services.AddScoped<IStoreQuery, StoreQuery> ();
            services.AddScoped<IDOMGroupsQuery, DOMGroupsQuery>();
            services.AddScoped<IProductStoreRepository, ProductStoreRepository>();
            services.AddScoped<IFieldProductQuery, FieldProductQuery>();
            services.AddScoped<IFieldProductStoreQuery, FieldProductStoreQuery>();
            services.AddScoped<ICategoriesQuery, CategoriesQuery>();
            services.AddScoped<INutritionQuery, NutritionQuery>();
            return services;
        }
        public static IServiceCollection AddCustomPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin"));
            });

            return services;
        }
        public static async Task<IServiceCollection> AddQuartzAsync(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("IntegrationService");
            var scheduler = await GetScheduler(connectionString);
            services.AddSingleton(scheduler);
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<QuartzHostedService>();
            services.AddSingleton<ImportJob>();
            services.AddScoped<ScheduleProcess>();
            //services.AddSingleton<HelloWorldJob>();


            return services;
        }

        static async Task<IScheduler> GetScheduler(string dbConnectionString)
        {
            var properties = new NameValueCollection
        {
            { "quartz.scheduler.instanceName", "QuartzWithCore" },
            { "quartz.scheduler.instanceId", "QuartzWithCore" },
            { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
            { "quartz.jobStore.useProperties", "true" },
            { "quartz.jobStore.dataSource", "default" },
            { "quartz.jobStore.tablePrefix", "QRTZ_" },
            {
                "quartz.dataSource.default.connectionString",
                $"{dbConnectionString};"
            },
            { "quartz.jobStore.performSchemaValidation", "false"},
            { "quartz.dataSource.default.provider", "SqlServer" },
            { "quartz.threadPool.threadCount", "1" },
            { "quartz.serializer.type", "json" },
        };
            var schedulerFactory = new StdSchedulerFactory(properties);
            var scheduler = await schedulerFactory.GetScheduler();
            return scheduler;
        }
    }
}
