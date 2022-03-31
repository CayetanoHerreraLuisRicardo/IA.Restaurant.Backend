using System.ComponentModel.DataAnnotations;

namespace IA.Restaurant.Utils.Models
{
    public class ProductModel
    {
        public int IdProduct { get; set; }
        [Required]
        public string Sku { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        [Required]
        public int Stock { get; set; }
    }
}
