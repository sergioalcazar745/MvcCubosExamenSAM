using CallApi.Helpers;
using MvcCubosExamenSAM.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace MvcCubosExamenSAM.Services
{
    public class ServiceCubos
    {
        private HelperCallApi api;

        public ServiceCubos(HelperCallApi api)
        {
            this.api = api;
            this.api.Uri = "https://localhost:7196/";
            this.api.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            return await this.api.GetApiAsync<List<Cubo>>("/api/cubos", "response");
        }

        public async Task<List<Cubo>> GetCubosByMarcaAsync(string marca)
        {
            return await this.api.GetApiAsync<List<Cubo>>("/api/cubos/" + marca, "response");
        }

        public async Task<List<string>> GetMarcasCuboAsync()
        {
            return await this.api.GetApiAsync<List<string>>("/api/cubos/marcascubo", "response");
        }

        public async Task InsertCuboAsync(CuboModel model)
        {
            bool result = await this.api.PostApiAsync("/api/cubos", model);
        }
    }
}
