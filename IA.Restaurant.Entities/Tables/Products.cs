using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IA.Restaurant.Entities.Tables
{
    [Table("products")]
    public partial class Products
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_product")]
        public int IdProduct { get; set; }
        [Required]
        [Column("sku")]
        [StringLength(20)]
        public string Sku { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("unit_price")]
        public double UnitPrice { get; set; }
        [Column("stock")]
        public int Stock { get; set; }
        [Column("deleted")]
        public bool Deleted { get; set; }
        [Column("creation_date", TypeName = "datetime")]
        public DateTime CreationDate { get; set; }
        [Column("modification_date", TypeName = "datetime")]
        public DateTime ModificationDate { get; set; }
    }
}
