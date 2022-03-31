using System.ComponentModel.DataAnnotations;

namespace IA.Restaurant.Utils.Models
{
    public class OrderStatusModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int IdOrder { get; set; }
        [Required]
        [Range(1, 5)]
        public int IdStatus { get; set; }
    }
}
