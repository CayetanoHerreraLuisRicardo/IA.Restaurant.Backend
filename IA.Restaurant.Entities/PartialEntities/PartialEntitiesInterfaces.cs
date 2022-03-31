/* Este archivo es para desacoplar las entidades de interfaces tales como IAutomaticModel (para automatizar actualizacion
 *  de fechas), manteniendo la integridad de dichas entidades sin preocuparse por regenerarlas (manual o scaffold .
 */
namespace IA.Restaurant.Entities.Tables
{ 
    public partial class Products : IAutomaticModel { }
    public partial class Status : IAutomaticModel { }
    public partial class Orders : IAutomaticModel { }
    public partial class OrderDetails : IAutomaticModel { }

}
