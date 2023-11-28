using IntegrationService.Models.Product;

namespace IntegrationService.Models.Nutritions
{
    public class NutritionProduct
    {
        public NutritionProduct()
        {
            
        }
        private NutritionProduct(
            Guid productId,
            Guid nutritionId,
            string sku,
            string title,
            string? cell1,
            string? cell2,
            string? cell3,
            string? cell4,
            bool isHiglight,
            bool bold,
            string? note)
        {
            Id = Guid.NewGuid();
            UpdateProductId(productId);
            UpdateNutrition(nutritionId);
            UpdateSku(sku);
            UpdateTitle(title);
            UpdateCell1(cell1);
            UpdateCell2(cell2);
            UpdateCell3(cell3);
            UpdateCell4(cell4);
            UpdateIsHighlight(isHiglight);
            UpdateBold(bold);
            UpdateNote(note);

        }

        public NutritionProduct Create(
            Guid productId,
            Guid nutritionId,
            string sku,
            string? title,
            string? cell1,
            string? cell2,
            string? cell3,
            string? cell4,
            bool isHighlight,
            bool bold,
            string? note)
        {
            return new NutritionProduct(productId, nutritionId, sku, title, cell1, cell2, cell3, cell4, isHighlight, bold, note);
        }

        private void UpdateNutrition(Guid nutritionId)
        {
            throw new NotImplementedException();
        }

        private void UpdateNote(string? note)
        {
            note = note ?? string.Empty;
        }

        private void UpdateBold(bool bold)
        {
            Bold = bold;
        }

        private void UpdateIsHighlight(bool isHiglight)
        {
            IsHighlight = isHiglight;
        }

        private void UpdateCell4(string? cell4)
        {
            cell4 = cell4 ?? string.Empty;
        }

        private void UpdateCell3(string? cell3)
        {
            Cell3 = cell3 ?? string.Empty;
        }

        private void UpdateCell2(string? cell2)
        {
            Cell2 = cell2 ?? string.Empty;
        }

        private void UpdateCell1(string? cell1)
        {
            Cell1 = cell1 ?? string.Empty;
        }

        private void UpdateTitle(string title)
        {
            Title = title ?? string.Empty;
        }

        private void UpdateSku(string sku)
        {
            Sku = sku ?? string.Empty;
        }

        private void UpdateProductId(Guid productId)
        {
            ProductId = productId;
        }

        public Guid Id { get; set; }
        public Guid NutritionId { get; set; }
        public virtual Nutrition Nutrition { get; set; }
        public Guid ProductId { get; set; }
        public virtual Products Product { get; set; }
        public string Sku { get; set; }
        public string Title { get; set; }
        public string Cell1 { get; set; }
        public string Cell2 { get; set; }
        public string Cell3 { get; set; }
        public string Cell4 { get; set; }
        public bool IsHighlight { get; set; }
        public bool Bold { get; set; }
        public string Note { get; set; }

    }
}
