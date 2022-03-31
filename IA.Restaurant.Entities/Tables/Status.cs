using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IA.Restaurant.Entities.Tables
{
    [Table("status")]
    public partial class Status
    {
        public Status()
        {
            Orders = new HashSet<Orders>();
        }

        [Key]
        [Column("id_status")]
        public int IdStatus { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("creation_date", TypeName = "datetime")]
        public DateTime CreationDate { get; set; }
        [Column("modification_date", TypeName = "datetime")]
        public DateTime ModificationDate { get; set; }

        [InverseProperty(nameof(Tables.Orders.IdStatusNavigation))]
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
