// <copyright file="InventarioController.cs" company="GrupoAnalisis">
// Copyright (c) GrupoAnalisis. All rights reserved.
// </copyright>

namespace GrupoNRJ.Cliente.GestionCafe.Controllers
{
    using System.Data;
    using System.Threading.Tasks;
    using GrupoNRJ.Cliente.GestionCafe.Utilidades;
    using GrupoNRJ.Modelos.GestionCafe;
    using GrupoNRJ.Modelos.GestionCafe.Respuestas;
    using GrupoNRJ.Modelos.GestionCafe.Solicitudes;
    using GrupoNRJ.Servicio.GestionCafe.Utilidades;
    using Microsoft.AspNetCore.Mvc;

    public class InventarioController : Controller
    {
        private readonly ClienteAPI clienteApi;

        public InventarioController(ClienteAPI clienteAPI)
        {
            this.clienteApi = clienteAPI;
        }

        public async Task<IActionResult> ManejoInventario()
        {
            try
            {
                var listadoProductos = await this.clienteApi.PostAsync<object, RespuestaBase<List<ProductoRespuesta>>>("Inventario/inventario", null);
                this.ViewData["listadoProductos"] = listadoProductos?.Datos ?? new List<ProductoRespuesta>();
                var alertas = await this.clienteApi.PostAsync<object, RespuestaBase<List<ObtenerAlertasRespuesta>>>("Inventario/ObtenerAlertas", null);
                this.ViewData["alertas"] = alertas?.Datos ?? new List<ObtenerAlertasRespuesta>();
                var catalogoGranos = await this.clienteApi.PostAsync<object, RespuestaBase<List<GranosRespuesta>>>("Catalogo/obtenerGranos", null);
                this.ViewData["catalogoGrano"] = catalogoGranos?.Datos ?? new List<GranosRespuesta>();
                var catalogoTostado = await this.clienteApi.PostAsync<object, RespuestaBase<List<NivelTostadoRespuesta>>>("Catalogo/obtenerNivelTostado", null);
                this.ViewData["catalogoTostado"] = catalogoTostado?.Datos ?? new List<NivelTostadoRespuesta>();
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
                var respuestaProducto = await this.clienteApi.PostAsync<AgregarProductoSolicitud, AgregarProductoRespuesta>("Inventario/agregarProducto", solicitud);
                if (respuestaProducto != null)
                {
                    mensaje = new { todoCorrecto = respuestaProducto.RegistroIngresadoCorrectamente };
                }
            }
            catch (Exception)
            {
                mensaje = new { todoCorrecto = false };
            }

            return this.Json(mensaje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarProducto([FromBody] EliminarProductoSolicitud solicitud)
        {
            // Aquí llamamos a un procedimiento almacenado
            var mensaje = new { todoCorrecto = false };
            try
            {
                var respuestaProducto = await this.clienteApi.PostAsync<EliminarProductoSolicitud, EliminarProductoRespuesta>("Inventario/eliminarProducto", solicitud);
                if (respuestaProducto != null)
                {
                    mensaje = new { todoCorrecto = respuestaProducto.RegistroEliminadoExitosamente };
                }
            }
            catch (Exception)
            {
                mensaje = new { todoCorrecto = false };
            }

            return this.Json(mensaje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObtenerInfoProducto([FromBody] ObtenerInfoProductoSolicitud solicitud)
        {
            // Aquí llamamos a un procedimiento almacenado
            var mensaje = new { todoCorrecto = false, idp = 0, grano = 0, nombre = string.Empty, cantidad = 0.0, nivel = 0 };
            try
            {
                var respuestaProducto = await this.clienteApi.PostAsync<ObtenerInfoProductoSolicitud, RespuestaBase<ObtenerInfoProductoRespuesta>>("Inventario/ObtenerInfoProducto", solicitud);
                if (respuestaProducto != null)
                {
                    if (respuestaProducto.Codigo == 0)
                    {
                        mensaje = new { todoCorrecto = true, idp = respuestaProducto.Datos.IdProducto, grano = respuestaProducto.Datos.GranoId, nombre = respuestaProducto.Datos.Nombre, cantidad = respuestaProducto.Datos.ValorMinimo, nivel = respuestaProducto.Datos.NivelTostado };
                    }
                    else
                    {
                        mensaje = new { todoCorrecto = false, idp = 0, grano = 0, nombre = string.Empty, cantidad = 0.0, nivel = 0 };
                    }
                }
            }
            catch (Exception)
            {
                mensaje = new { todoCorrecto = false, idp = 0, grano = 0, nombre = string.Empty, cantidad = 0.0,nivel = 0 };
            }

            return this.Json(mensaje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarProducto([FromBody] ModificarProductoSolicitud solicitud)
        {
            // Aquí llamamos a un procedimiento almacenado
            var mensaje = new { todoCorrecto = false };
            try
            {
                var respuestaProducto = await this.clienteApi.PostAsync<ModificarProductoSolicitud, ModificarProductoRespuesta>("Inventario/ModificarProducto", solicitud);
                if (respuestaProducto != null)
                {
                    mensaje = new { todoCorrecto = respuestaProducto.RegistroModificadoExitosamente };
                }
            }
            catch (Exception)
            {
                mensaje = new { todoCorrecto = false };
            }

            return this.Json(mensaje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObtenerMovProducto([FromBody] ConsultarMovimientosProductoSolicitud solicitud)
        {
            // Aquí llamamos a un procedimiento almacenado
            var mensaje = new { todoCorrecto = false, registros = new List<ConsultarMovimientosProductoRespuesta>() };
            try
            {
                var respuestaProducto = await this.clienteApi.PostAsync<ConsultarMovimientosProductoSolicitud, RespuestaBase<List<ConsultarMovimientosProductoRespuesta>>>("Inventario/ConsultarMovimientos", solicitud);
                if (respuestaProducto != null && respuestaProducto?.Codigo == 0)
                {
                    mensaje = new { todoCorrecto = true, registros = respuestaProducto.Datos };
                }
            }
            catch (Exception)
            {
                mensaje = new { todoCorrecto = false, registros = new List<ConsultarMovimientosProductoRespuesta>() };
            }

            return this.Json(mensaje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarMovProducto([FromBody] AgregarMovimientoSolicitud solicitud)
        {
            // Si es menor a 0 hace salida, sino es ingreso
            solicitud.TipoMovimiento = solicitud.Cantidad < 0 ? 2 : 1;
            solicitud.Cantidad = solicitud.Cantidad < 0 ? (solicitud.Cantidad * -1) : solicitud.Cantidad;

            // Aquí llamamos a un procedimiento almacenado
            var mensaje = new { todoCorrecto = false, mensaje = string.Empty };
            try
            {
                var respuestaProducto = await this.clienteApi.PostAsync<AgregarMovimientoSolicitud, AgregarMovimientoRespuesta>("Inventario/IngresarMovimiento", solicitud);
                if (respuestaProducto != null)
                {
                    switch (respuestaProducto?.Codigo)
                    {
                        case 1:

                            mensaje = new { todoCorrecto = true, mensaje = respuestaProducto?.Mensaje };
                            break;
                        case 2:

                            mensaje = new { todoCorrecto = true, mensaje = respuestaProducto?.Mensaje };
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                mensaje = new { todoCorrecto = false, mensaje = string.Empty };
            }

            return this.Json(mensaje);
        }
    }
}
