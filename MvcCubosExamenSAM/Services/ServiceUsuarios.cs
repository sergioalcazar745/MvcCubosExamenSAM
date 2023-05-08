using CallApi.Helpers;
using MvcCubosExamenSAM.Filters;
using MvcCubosExamenSAM.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace MvcCubosExamenSAM.Services
{
    public class ServiceUsuarios
    {
        private HelperCallApi api;

        public ServiceUsuarios(HelperCallApi api)
        {
            this.api = api;
            this.api.Uri = "https://localhost:7196/";
            this.api.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        private async Task<string> PostApiAsync(string request, object objeto, object key)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7196/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string json = JsonConvert.SerializeObject(objeto);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject jsonData = JObject.Parse(data);
                    return jsonData.GetValue(key.ToString()).ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            LoginModel model = new LoginModel { Username = username, Password = password };
            return await this.PostApiAsync("/api/auth", model, "response");
        }

        [AuthorizeUsuarios]
        public async Task<Usuario> PerfilAsync(string token)
        {
            Usuario usuario = await this.api.GetApiTokenAsync<Usuario>("/api/usuarios/perfil", token, "response");
            return usuario;
        }

        public async Task CrearUsuarioAsync(UsuarioModel model)
        {
            await this.api.PostApiAsync("/api/usuarios/insertusuario", model);
        }

        [AuthorizeUsuarios]
        public async Task CrearPedidoAsync(CompraCuboModel model, string token)
        {
            await this.api.PostApiTokenAsync("/api/usuarios/insertpedido", model, token);
        }

        [AuthorizeUsuarios]
        public async Task<List<CompraCubo>> PedidosUsuarioAsync(string token)
        {
            return await this.api.GetApiTokenAsync<List<CompraCubo>>("/api/usuarios/pedidosusuario", token, "response");
        }

        public async Task CrearUsuario(UsuarioModel model)
        {
            await this.api.PostApiAsync("/api/usuarios/insertusuario", model);
        }
    }
}
