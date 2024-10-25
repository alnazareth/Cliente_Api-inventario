namespace Api_SistemaInventario.Models
{
    public class ResultadoApi
    {
        public Dictionary<string, List<Product>> CategorizedProducts { get; set; }
        public string mensaje { get; set; }

        public List<Product> products { get; set; }

        public Product objeto { get; set; }
    }
}
