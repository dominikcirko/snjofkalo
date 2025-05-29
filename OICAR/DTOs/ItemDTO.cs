using System.ComponentModel.DataAnnotations;

namespace OICAR.DTOs
{
    public class ItemDTO
    {
        public int IDItem { get; set; }
        public int ItemCategoryID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
    }
}
