using System.ComponentModel.DataAnnotations;

namespace Api_SistemaInventario.Models
{
    public class Product
    {

        public int Id { get; set; }
        
       public string Name { get; set; }

        public string PreparationType { get; set; }

      
        public string Status { get; set; }         
        


        public int InStock { get; set; } = 10;
    }
}
