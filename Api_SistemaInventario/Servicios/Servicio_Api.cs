using Api_SistemaInventario.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Api_SistemaInventario.Servicios
{
    public class Servicio_Api :IServicio_Api
    {
        private static string _username;
        private static string _password;
        private static string _baseurl;
        private static string _token;

        public Servicio_Api()
        {
            var builder =  new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            _username = builder.GetSection("ApiSettings:username").Value;
            _password = builder.GetSection("ApiSettings:password").Value;
            _baseurl = builder.GetSection("ApiSettings:baseUrl").Value;

        }

      public async Task<bool> AddProductAsync(Product product)
{
    bool respuesta = false;

    await Autenticar();

    var cliente = new HttpClient();
    cliente.BaseAddress = new Uri(_baseurl);
    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

    var content = new StringContent(JsonConvert.SerializeObject(new
    {
        name = product.Name,
        preparationType = product.PreparationType,
        status = product.Status,
        inStock = product.InStock 
    }), Encoding.UTF8, "application/json");

    var response = await cliente.PostAsync("api/Products/AddProduct", content);

    if (response.IsSuccessStatusCode)
    {
        respuesta = true;
    }

    return respuesta;
}


        public async Task Autenticar() {

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseurl);

            var credenciales = new LoginRequest() { username = _username, password = _password };

            var content = new StringContent(JsonConvert.SerializeObject(credenciales), Encoding.UTF8, "application/json");

            var response = await cliente.PostAsync("api/Auth/login",content);

            var json_respuesta = await response.Content.ReadAsStringAsync();

            var resultado = JsonConvert.DeserializeObject<ResultadoCredencial>(json_respuesta);

            _token = resultado.token;


        }


        public async Task<List<Product>> GetProductListAsync()
        {
            List<Product> products = new List<Product>();

            await Autenticar();

            var cliente = new HttpClient();
            cliente.BaseAddress= new Uri(_baseurl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await cliente.GetAsync("api/Products");

            if (response.IsSuccessStatusCode) {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Product>>(json_respuesta);
                products = resultado;
            }

            return products;

        }
        
        public async Task<bool> ReduceStockAsync(int productId, int cantidad)
        {
            List<Product> products = new List<Product>();

            await Autenticar();

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseurl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

           
            var jsonContent = new StringContent(JsonConvert.SerializeObject(cantidad), Encoding.UTF8, "application/json");

            var response = await cliente.PutAsync($"api/Products/SalidaProductos/{productId}", jsonContent);

            return response.IsSuccessStatusCode;
        }



        public async Task<Product> GetProductByIdAsync(int id)
        {

            Product objeto = new Product();

            await Autenticar();

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseurl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await cliente.GetAsync($"api/Products/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi>(json_respuesta);
                objeto = resultado.objeto;
            }

            return objeto;

        }


        public async Task<bool> MarkAsDefectiveAsync(int id)
        {
            bool respuesta = false;

            await Autenticar();

            using var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseurl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

         
            var response = await cliente.PutAsync($"api/Products/MarcarDefectuoso/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }

            return respuesta;
        }


        public async Task<bool> DeleteProductAsync(int id)
        {

            bool respuesta = false;

            await Autenticar();

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseurl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);


            var response = await cliente.DeleteAsync($"api/Products/{id}");

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }

            return respuesta;
        }
    }
}
