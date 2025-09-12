using GrupoNRJ.Cliente.GestionCafe.Utilidades;
using GrupoNRJ.Modelos.GestionCafe.Respuestas;
using GrupoNRJ.Modelos.GestionCafe;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GrupoNRJ.Servicio.GestionCafe.Utilidades;
using System.Data;
using GrupoNRJ.Modelos.GestionCafe.Solicitudes;

namespace GrupoNRJ.Cliente.GestionCafe.Controllers
{
    public class InventarioController : Controller
    {
        private readonly ClienteAPI _clienteApi;
        public InventarioController(ClienteAPI clienteAPI)
        {
            this._clienteApi = clienteAPI;
        }

        public async Task<IActionResult> ManejoInventario()
        {
            try
            {
                var listadoProductos = await _clienteApi.PostAsync<object, RespuestaBase<List<ProductoRespuesta>>>("Inventario/inventario", null);
                this.ViewData["listadoProductos"] = listadoProductos?.Datos ?? new List<ProductoRespuesta>();
                var Alertas = await _clienteApi.PostAsync<object, RespuestaBase<List<ObtenerAlertasRespuesta>>>("Inventario/ObtenerAlertas", null);
                this.ViewData["alertas"] = Alertas?.Datos ?? new List<ObtenerAlertasRespuesta>();
                var CatalogoGranos = await _clienteApi.PostAsync<object, RespuestaBase<List<GranosRespuesta>>>("Catalogo/obtenerGranos", null);
                this.ViewData["catalogoGrano"] = CatalogoGranos?.Datos ?? new List<GranosRespuesta>();
            }
            catch (Exception ex)
            {
                Bitacoras.GuardarError(ex.ToString(), new { });
            }

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarProducto([FromBody] AgregarProductoSolicitud solicitud)
        {
            // Aquí llamamos a un procedimiento almacenado
            var mensaje = new { todoCorrecto = false };
            try
            {

                var respuestaProducto = await _clienteApi.PostAsync<AgregarProductoSolicitud, AgregarProductoRespuesta>("Inventario/agregarProducto", solicitud);
                if (respuestaProducto != null)
                {
                    mensaje = new { todoCorrecto = respuestaProducto.RegistroIngresadoCorrectamente };

                }
            }
            catch (Exception)
            {
                mensaje = new { todoCorrecto = false };
            }


            return Json(mensaje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarProducto([FromBody] EliminarProductoSolicitud solicitud)
        {
            // Aquí llamamos a un procedimiento almacenado
            var mensaje = new { todoCorrecto = false };
            try
            {

                var respuestaProducto = await _clienteApi.PostAsync<EliminarProductoSolicitud, EliminarProductoRespuesta>("Inventario/eliminarProducto", solicitud);
                if (respuestaProducto != null)
                {
                    mensaje = new { todoCorrecto = respuestaProducto.RegistroEliminadoExitosamente };

                }
            }
            catch (Exception)
            {
                mensaje = new { todoCorrecto = false };
            }


            return Json(mensaje);
        }
    }
}
