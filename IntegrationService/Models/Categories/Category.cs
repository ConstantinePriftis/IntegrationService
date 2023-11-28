using IntegrationService.Models.EntityBases;
using IntegrationService.Models.User;
using IntegrationService.ViewModels.CategoryViewModels;
using IntegrationService.ViewModels.ChannelViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntegrationService.Models.Categories
{
    public class Category : BaseEntity
    {
        public Category()
        {

        }
        public string Code { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        private void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description?.Trim())) throw new ArgumentNullException(nameof(description));
            description = description.Trim();
            if (Description != description)
                Description = description;
        }
        private void UpdateModifier(ApplicationUser modifier)
        {
            ModifiedBy = modifier;
            ModifiedOn = DateTime.UtcNow;
        }
        public static Category Create(string code, int level, string description, ApplicationUser applicationUser)
        {
            return new(code, level, description, applicationUser);
        }
        public CategoryViewModel ToDto()
        {
            return new CategoryViewModel
            {
                Code = Code,
                Level = Level.ToString(),
                Description = Description
            };
        }
        public SelectListItem ToSelectListItem()
        {
            return new SelectListItem()
            {
                Text = Code,
                Value = Code
            };
        }
        private Category(string code, int level, string description, ApplicationUser user)
        {
            this.Description = description;
            this.Code = code;
            this.Level = level;
            this.CreatedOn = DateTime.Now;
            this.CreatedBy = user;
            this.Id = Guid.NewGuid();
        }
        public Category Update(Category category, ApplicationUser user)
        {
            this.Code = category.Code;
            this.Level = category.Level;
            this.Description = category.Description ?? string.Empty;
            this.ModifiedBy = user;
            this.ModifiedOn = DateTime.Now;
            this.Enabled = category.Enabled;
            return this;
        }
        public static Category Create(Category category, ApplicationUser user)
        {
            Category _category = new Category(category.Code, category.Level, category.Description, user);
            return _category;
        }

    }
}