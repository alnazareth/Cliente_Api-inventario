using Api_SistemaInventario.Models;

namespace Api_SistemaInventario.Servicios
{
    public interface IServicio_Api
    {


            Task <List<Product>> GetProductListAsync(); // obtener todos los productos del stock

           Task<Product> GetProductByIdAsync( int id);  // obtener producto por id

           Task<bool> AddProductAsync(Product product); // agregar varios productos

        Task<bool> MarkAsDefectiveAsync(int id);  // marcamos como defectuoso

        Task<bool> DeleteProductAsync(int id);  // eliminamos los datos del producto

        Task<bool> ReduceStockAsync(int id, int cantidad);  // restamos las salidas


    }
}
