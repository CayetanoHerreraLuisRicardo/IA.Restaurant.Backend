namespace IA.Restaurant.Entities
{
    /// <summary>
    /// Interface para implementar en todos los modelos que tienen este par de campos.
    /// Dichos campos se autoactualizarán por medio de RestaurantContext y nosotros ya no nos preocupamos por llenarlos.
    /// </summary>
    public interface IAutomaticModel
    {
        DateTime CreationDate { get; set; }
        DateTime ModificationDate { get; set; }
    }
}
