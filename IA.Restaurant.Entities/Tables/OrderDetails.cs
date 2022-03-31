using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace IA.Restaurant.Entities.Tables
{
    [Keyless]
    [Table("order_details")]
    public partial class OrderDetails
    {
        [Column("unit_price")]
        public double UnitPrice { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("creation_date", TypeName = "datetime")]
        public DateTime CreationDate { get; set; }
        [Column("modification_date", TypeName = "datetime")]
        public DateTime ModificationDate { get; set; }
        [Column("id_order")]
        public int IdOrder { get; set; }
        [Column("id_product")]
        public int IdProduct { get; set; }

        [ForeignKey(nameof(IdOrder))]
        public virtual Orders IdOrderNavigation { get; set; }
        [ForeignKey(nameof(IdProduct))]
        public virtual Products IdProductNavigation { get; set; }
    }
}
