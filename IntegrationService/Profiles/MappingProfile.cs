using AutoMapper;
using IntegrationService.Models.Categories;
using IntegrationService.Models.Fields;
using IntegrationService.Models.Imports;
using IntegrationService.Models.Nutritions;
using IntegrationService.Models.Product;
using IntegrationService.ViewModels.CategoryViewModels;
using IntegrationService.ViewModels.FieldProductStoresViewModels;
using IntegrationService.ViewModels.FieldProductsViewModels;
using IntegrationService.ViewModels.FieldViewModels;
using IntegrationService.ViewModels.ImportViewModels;
using IntegrationService.ViewModels.Nutrition;
using IntegrationService.ViewModels.ProductViewModels;

namespace IntegrationService.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //From ViewModel To DbEntity
            CreateMap<ImportViewModel, Import>()
                .ForMember(d => d.ImportDate, o => o.MapFrom(src => DateTime.Now))
                .ForMember(d => d.Efood, o => o.MapFrom(src => src.Efood))
                .ForMember(d => d.CreatedOn, o => o.MapFrom(src => DateTime.Now))
                .ForMember(d => d.Enabled, o => o.MapFrom(src => true))
                .ForMember(d => d.Sku, o => o.MapFrom(src => src.Sku))
                .ForMember(d => d.Status, o => o.MapFrom(src => src.Status))
                .ForMember(d => d.Step, o => o.MapFrom(src => src.Step))
                .ForMember(d => d.ModifiedOn, o => o.MapFrom(src => DateTime.Now))
                .ForMember(d => d.Description, o => o.MapFrom(src => src.Description))
                .ForMember(d => d.Digital, o => o.MapFrom(src => src.Digital))
                .ForMember(d => d.StoreCode, o => o.MapFrom(src => src.StoreCode));
            //.ForMember(d => d.ProductEnabled, o => o.MapFrom(src => true))


            //From Db Entity To ViewModel
            CreateMap<Import, ImportViewModel>()
                .ForMember(d => d.Sku, o => o.MapFrom(src => src.Sku))
                .ForMember(d => d.Status, o => o.MapFrom(src => src.Status))
                .ForMember(d => d.Step, o => o.MapFrom(src => src.Step))
                .ForMember(d => d.Description, o => o.MapFrom(src => src.Description))
                .ForMember(d => d.StoreCode, o => o.MapFrom(src => src.StoreCode.ToString()));

            CreateMap<FieldCreateViewModel, Field>()
                .ForMember(d => d.Name, o => o.MapFrom(src => src.Name))
                .ForMember(d => d.Type, o => o.MapFrom(src => src.Type));

            CreateMap<Field, FieldCreateViewModel>()
                .ForMember(d => d.Name, o => o.MapFrom(src => src.Name))
                .ForMember(d => d.Type, o => o.MapFrom(src => src.Type));

            CreateMap<CategoryViewModel, Category>()
                .ForMember(d => d.Code, o => o.MapFrom(src => src.Code))
                .ForMember(d => d.Description, o => o.MapFrom(src => src.Description))
                .ForMember(d => d.Level, o => o.MapFrom(src => src.Level));

            CreateMap<Category, CategoryViewModel>()
                .ForMember(d => d.Code, o => o.MapFrom(src => src.Code))
                .ForMember(d => d.Description, o => o.MapFrom(src => src.Description))
                .ForMember(d => d.Level, o => o.MapFrom(src => src.Level));

            CreateMap<NutritionViewModel, Nutrition>()
                .ForMember(d => d.Sku, o => o.MapFrom(src => src.Sku))
                .ForMember(d => d.Title, o => o.MapFrom(src => src.Title))
                .ForMember(d => d.FirstCell, o => o.MapFrom(src => src.Cell1))
                .ForMember(d => d.SecondCell, o => o.MapFrom(src => src.Cell2))
                .ForMember(d => d.ThirdCell, o => o.MapFrom(src => src.Cell3))
                .ForMember(d => d.FourthCell, o => o.MapFrom(src => src.Cell4))
                .ForMember(d => d.IsHighlight, o => o.MapFrom(src => src.IsHighlight))
                .ForMember(d => d.Order, o => o.MapFrom(src => src.Order))
                .ForMember(d => d.IsBold, o => o.MapFrom(src => src.IsBold))
                .ForMember(d => d.Note, o => o.MapFrom(src => src.Note))
                .ReverseMap();

            CreateMap<ProductsViewModel, Products>()
                .ForMember(m => m.Id, o => o.MapFrom(src => src.Id))
                .ForMember(m => m.Sku, o => o.MapFrom(src => src.Sku))
                .ForMember(m => m.Title, o => o.MapFrom(src => src.Title))
                .ForMember(m => m.Description_Category, o => o.MapFrom(src => src.Description_Category))
                .ForMember(m => m.Enabled, o => o.MapFrom(src => src.IsCompleted))
                //.ForMember(m => m.Description, o => o.MapFrom(src => src.De))
                .ReverseMap();
            //CreateMap<Nutrition, NutritionViewModel>()
            //    .ForMember(d => d.Sku, o => o.MapFrom(src => src.Sku))
            //    .ForMember(d => d.Title, o => o.MapFrom(src => src.Title))
            //    .ForMember(d => d.Cell1, o => o.MapFrom(src => src.Cell1))
            //    .ForMember(d => d.Cell2, o => o.MapFrom(src => src.Cell2))
            //    .ForMember(d => d.Cell3, o => o.MapFrom(src => src.Cell3))
            //    .ForMember(d => d.Cell4, o => o.MapFrom(src => src.Cell4))
            //    .ForMember(d => d.IsHighlight, o => o.MapFrom(src => src.IsHighlight))
            //    .ForMember(d => d.Order, o => o.MapFrom(src => src.Order))
            //    .ForMember(d => d.IsBold, o => o.MapFrom(src => src.IsBold))
            //    .ForMember(d => d.Note, o => o.MapFrom(src => src.Note));


            CreateMap<FieldsPerProductEditViewModel, FieldProducts>()
                .ForMember(d => d.ProductsId, o => o.MapFrom(src => src.ProductId))
                .ForMember(d => d.FieldId, o => o.MapFrom(src => src.FieldId))
                .ForMember(d => d.Value, o => o.MapFrom(src => src.Value));

            CreateMap<FieldProducts, FieldsPerProductEditViewModel>()
                .ForMember(d => d.ProductId, o => o.MapFrom(src => src.ProductsId))
                .ForMember(d => d.FieldId, o => o.MapFrom(src => src.FieldId))
                .ForMember(d => d.Value, o => o.MapFrom(src => src.Value));

            CreateMap<FieldsPerProductStoreEditViewModel, FieldProductStore>()
                .ForMember(d => d.ProductStoresId, o => o.MapFrom(src => src.ProductStoreId))
                .ForMember(d => d.FieldId, o => o.MapFrom(src => src.FieldId))
                .ForMember(d => d.Value, o => o.MapFrom(src => src.Value));

            CreateMap<FieldProductStore, FieldsPerProductStoreEditViewModel>()
                .ForMember(d => d.ProductStoreId, o => o.MapFrom(src => src.ProductStoresId))
                .ForMember(d => d.FieldId, o => o.MapFrom(src => src.FieldId))
                .ForMember(d => d.Value, o => o.MapFrom(src => src.Value));
        }
    }
}
