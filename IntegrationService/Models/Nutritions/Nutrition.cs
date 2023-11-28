using IntegrationService.Models.EntityBases;
using IntegrationService.Models.Product;
using IntegrationService.Models.User;
using System.Xml.Linq;

namespace IntegrationService.Models.Nutritions
{
    public class Nutrition : SimpleBaseEntity
    {
        public Nutrition()
        {

        }
        private Nutrition(
            string sku,
            string? title,
            string? firstCell,
            string? secondCell,
            string? thirdCell,
            string? fourthCell,
            bool isHighlight,
            int order,
            bool isBold,
            string? note,
            Guid productId)
        {
            UpdateSku(sku);
            UpdateTitle(title);
            UpdateFirstCell(firstCell);
            UpdateSecondCell(secondCell);
            UpdateThirdCell(thirdCell);
            UpdateFourthCell(fourthCell);
            UpdateIsHighlight(isHighlight);
            UpdateOrder(order);
            UpdateIsBold(isBold);
            UpdateNote(note);
            //UpdateProduct(product);
        }

        //private void UpdateProduct(Products product)
        //{
        //    if(product != null) 
        //    { 
        //        this.Product = product;
        //    }
        //}

        private void UpdateNote(string? note)
        {
            if (Note != note)
                Note = note;
        }
        private void UpdateIsBold(bool isBold)
        {
            if (IsBold != isBold)
                IsBold = isBold;
        }
        private void UpdateOrder(int order)
        {
            if (Order != order)
                Order = order;
        }
        private void UpdateIsHighlight(bool isHighlight)
        {
            if (IsHighlight != isHighlight)
                IsHighlight = isHighlight;
        }
        private void UpdateFourthCell(string? fourthCell)
        {
            if (FourthCell != fourthCell)
                FourthCell = fourthCell;
        }
        private void UpdateThirdCell(string? thirdCell)
        {
            if (ThirdCell != thirdCell)
                ThirdCell = thirdCell;
        }
        private void UpdateSecondCell(string? secondCell)
        {
            if (SecondCell != secondCell)
                SecondCell = secondCell;
        }
        private void UpdateFirstCell(string? firstCell)
        {

            if (FirstCell != firstCell)
                FirstCell = firstCell;
        }
        private void UpdateTitle(string? title)
        {
            if (Title != title)
                Title = title;
        }
        private void UpdateSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentNullException(nameof(Sku));
            if (Sku != sku)
                Sku = sku;
        }
        public bool IsValid(out string validationMessage)
        {
            bool isValid = true;
            validationMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.Sku))
            {
                isValid = false;
                validationMessage = "Sku cannot be empty";
            }
            if (this.ProductId == Guid.Empty)
            {
                isValid = false;
                validationMessage = "ProductId cannot be empty";
            }
            if (this.Order <= 0)
            {
                isValid = false;
                validationMessage = "Order has to be above 1";
            }

            return isValid;
        }


        public string Sku { get; set; }
        public string? Title { get; set; }
        public string? FirstCell { get; set; }
        public string? SecondCell { get; set; }
        public string? ThirdCell { get; set; }
        public string? FourthCell { get; set; }
        public bool IsHighlight { get; set; } = false;
        public int Order { get; set; } = 0;
        public bool IsBold { get; set; } = false;
        public string? Note { get; set; }
        public Guid ProductId { get; set; }

        //public virtual Products Product { get; set; }



        public static Nutrition Create(
            string sku,
            string? title,
            string? firstCell,
            string? secondCell,
            string? thirdCell,
            string? fourthCell,
            bool isHighlight,
            int order,
            bool isBold,
            string? note,
            Guid productId)
        {
            var nutrition = new Nutrition(
                sku,
                title,
                firstCell,
                secondCell,
                thirdCell,
                fourthCell,
                isHighlight,
                order,
                isBold,
                note,
                productId);
            return nutrition;
        }
        public void Update(
            Nutrition nutrition, Guid productId)
        {
            if (nutrition == null)
            {
                throw new ArgumentNullException(nameof(nutrition));
            }
            UpdateSku(nutrition.Sku);
            UpdateTitle(nutrition.Title);
            UpdateFirstCell(nutrition.FirstCell);
            UpdateSecondCell(nutrition.SecondCell);
            UpdateThirdCell(nutrition.ThirdCell);
            UpdateFourthCell(nutrition.FourthCell);
            UpdateIsHighlight(nutrition.IsHighlight);
            UpdateOrder(nutrition.Order);
            UpdateIsBold(nutrition.IsBold);
            UpdateNote(nutrition.Note);
            //this.Product = product;
            this.ProductId = productId;
            ModifiedOn = DateTime.Now;
        }
    }
}
