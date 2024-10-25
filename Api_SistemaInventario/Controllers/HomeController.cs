using Api_SistemaInventario.Models;
using Api_SistemaInventario.Servicios;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Api_SistemaInventario.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServicio_Api _servicio_Api;

        public HomeController(IServicio_Api servicioApi)
        {
            _servicio_Api = servicioApi;
        }

        public async Task<IActionResult> Index()
        {

            List<Product> Lista = await _servicio_Api.GetProductListAsync();
            if (Lista == null)
            {
                Lista = new List<Product>(); // Inicializa la lista vacía si es null
            }

            return View(Lista);
        }

        public async Task<IActionResult> Producto(int id)
        {
            Product modelo_producto = new Product();

            ViewBag.Accion = "Nuevo producto";

            if (id != 0) {
                modelo_producto = await _servicio_Api.GetProductByIdAsync(id);
                ViewBag.Accion = "Nuevo producto";
            }

           

            return View(modelo_producto);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarProducto(Product producto)
        {
          
            if (!ModelState.IsValid)
            {
                return View("Producto", producto); // validamos el formulario
            }
           
            /*
            var json = JsonConvert.SerializeObject(producto);
           
            Console.WriteLine(json);
                                                    aqui hicimos debug del json buscando errores en el formato
           */

         
            bool respuesta = await _servicio_Api.AddProductAsync(producto);

            if (respuesta)
            {
                return RedirectToAction("Index"); // Redirigir a la lista de productos
            }
            else
            {
                ModelState.AddModelError("", "Error al agregar el producto.");
                return View("Producto", producto); // Devuelve a la vista con un mensaje de error
            }
        }


        [HttpPost]
        public async Task<IActionResult> PonerDefectuoso(int id) {

            bool respuesta;
            if (id == 0)
            {
                return BadRequest("ID no pertenece a ningun producto");
            }
            
             respuesta = await _servicio_Api.MarkAsDefectiveAsync(id);
            

            if(respuesta)
                return RedirectToAction("Index");
            else
                return NoContent();

        }


        [HttpPost]
        public async Task<IActionResult> SacarProducto(int productId, int cantidad)
        {
            if (productId == 0 || cantidad <= 0)
            {
                return BadRequest("ID de producto o cantidad no válidos.");
            }


            // Llama al servicio para reducir el stock
            bool respuesta = await _servicio_Api.ReduceStockAsync(productId, cantidad);

            if (respuesta)
            {
                TempData["Success"] = "Producto reducido exitosamente.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al reducir el stock del producto.";
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Eliminar(int id) { 
        
            var respuesta = await _servicio_Api.DeleteProductAsync(id);

            if (respuesta)
                return RedirectToAction("Index");
            else
                return NoContent();

        }



       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
