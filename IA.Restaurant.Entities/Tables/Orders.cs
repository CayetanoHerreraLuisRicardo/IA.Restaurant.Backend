using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IA.Restaurant.Entities.Tables
{
    [Table("orders")]
    public partial class Orders
    {
        [Key]
        [Column("id_order")]
        public int IdOrder { get; set; }
        [Column("creation_date", TypeName = "datetime")]
        public DateTime CreationDate { get; set; }
        [Column("modification_date", TypeName = "datetime")]
        public DateTime ModificationDate { get; set; }
        [Column("id_status")]
        public int IdStatus { get; set; }

        [ForeignKey(nameof(IdStatus))]
        [InverseProperty(nameof(Status.Orders))]
        public virtual Status IdStatusNavigation { get; set; }
    }
}
