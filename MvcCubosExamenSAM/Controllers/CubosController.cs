using Microsoft.AspNetCore.Mvc;
using MvcCubosExamenSAM.Models;
using MvcCubosExamenSAM.Services;

namespace MvcCubosExamenSAM.Controllers
{
    public class CubosController : Controller
    {
        ServiceCubos service;
        ServiceBlobs serviceBlobs;
        private string urlImages;
        public CubosController(ServiceCubos service, ServiceBlobs serviceBlobs)
        {
            this.service = service;
            this.serviceBlobs = serviceBlobs;
            this.urlImages = "https://storageaccountsamsergio.blob.core.windows.net/imagenescubo/";
        }

        public async Task<IActionResult> Cubos()
        {
            ViewData["MARCAS"] = await this.service.GetMarcasCuboAsync();
            List<Cubo> cubosbbdd = await this.service.GetCubosAsync();
            List<Cubo> cubosimage = new List<Cubo>();
            foreach(Cubo cubo in cubosbbdd)
            {
                cubo.Imagen = this.urlImages + cubo.Imagen;
                cubosimage.Add(cubo);
            }
            return View(cubosimage);
        }

        [HttpPost]
        public async Task<IActionResult> Cubos(string marca)
        {
            ViewData["MARCAS"] = await this.service.GetMarcasCuboAsync();
            List<Cubo> cubosbbdd = await this.service.GetCubosByMarcaAsync(marca);
            List<Cubo> cubosimage = new List<Cubo>();
            foreach (Cubo cubo in cubosbbdd)
            {
                cubo.Imagen = this.urlImages + cubo.Imagen;
                cubosimage.Add(cubo);
            }
            return View(cubosimage);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CuboModel model, IFormFile imagenfile)
        {
            using (Stream stream = imagenfile.OpenReadStream())
            {
                await this.serviceBlobs.UploadBlobAsync("imagenescubo", imagenfile.FileName, stream);
            }
            model.Imagen = imagenfile.FileName;
            await this.service.InsertCuboAsync(model);
            return RedirectToAction("Cubos");
        }
    }
}
