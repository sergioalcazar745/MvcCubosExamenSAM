using Microsoft.AspNetCore.Mvc;
using MvcCubosExamenSAM.Filters;
using MvcCubosExamenSAM.Models;
using MvcCubosExamenSAM.Services;

namespace MvcCubosExamenSAM.Controllers
{
    public class UsuariosController : Controller
    {
        ServiceUsuarios service;
        ServiceBlobs serviceBlobs;
        public UsuariosController(ServiceUsuarios service, ServiceBlobs serviceBlobs)
        {
            this.service = service;
            this.serviceBlobs = serviceBlobs;
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Perfil()
        {
            Usuario usuario = await this.service.PerfilAsync(HttpContext.Session.GetString("token"));
            string uri = await this.serviceBlobs.GetBlobUriPrivateAsync("imagenesusuarios", usuario.Imagen);
            usuario.Imagen = uri;
            return View(usuario);
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Pedidos()
        {
            return View(await this.service.PedidosUsuarioAsync(HttpContext.Session.GetString("token")));
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Comprar(int id)
        {
            await this.service.CrearPedidoAsync(new CompraCuboModel { IdCubo = id, FechaPedido = DateTime.Now }, HttpContext.Session.GetString("token"));
            return RedirectToAction("Pedidos");
        }

        public IActionResult CrearUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario(UsuarioModel model, IFormFile imagenusuario)
        {
            using (Stream stream = imagenusuario.OpenReadStream())
            {
                await this.serviceBlobs.UploadBlobAsync("imagenesusuarios", imagenusuario.FileName, stream);
            }
            model.Imagen = imagenusuario.FileName;
            await this.service.CrearUsuarioAsync(model);
            return RedirectToAction("Cubos", "Cubos");
        }
    }
}
