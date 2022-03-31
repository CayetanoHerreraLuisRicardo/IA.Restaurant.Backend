namespace IA.Restaurant.Utils.Models
{
    public class OrderModel
    {
        public int IdOrder { get; set; }
        public string Status { get; set; }
        public int IdStatus { get; set; }
        public List<ProductModel> lstProduct { get; set; }
    }
}
