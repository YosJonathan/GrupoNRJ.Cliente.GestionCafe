// <copyright file="PlanificacionController.cs" company="GrupoAnalisis">
// Copyright (c) GrupoAnalisis. All rights reserved.
// </copyright>

namespace GrupoNRJ.Cliente.GestionCafe.Controllers
{
    using GrupoNRJ.Cliente.GestionCafe.Utilidades;
    using GrupoNRJ.Modelos.GestionCafe;
    using GrupoNRJ.Modelos.GestionCafe.Respuestas;
    using GrupoNRJ.Modelos.GestionCafe.Solicitudes;
    using Microsoft.AspNetCore.Mvc;

    public class PlanificacionController : Controller
    {
        private readonly ClienteAPI clienteApi;

#pragma warning disable IDE0290 // Usar constructor principal
        public PlanificacionController(ClienteAPI clienteAPI)
#pragma warning restore IDE0290 // Usar constructor principal
        {
            this.clienteApi = clienteAPI;
        }

        public async Task<IActionResult> PlanificacionLotes()
        {
            var listadoProductos = await this.clienteApi.GetAsync<RespuestaBase<List<ObtenerPlanificacionRespuesta>>>("Planificacion/planificacion");
            this.ViewData["planificacion"] = listadoProductos?.Datos ?? new();
            var listadoEstados = await this.clienteApi.GetAsync<RespuestaBase<List<CatalogoRespuesta>>>("Catalogo/obtenerEstado");
            this.ViewData["estados"] = listadoEstados?.Datos ?? new();
            var listadoLotes = await this.clienteApi.GetAsync<RespuestaBase<List<CatalogoRespuesta>>>("Catalogo/obtenerLote");
            this.ViewData["lotes"] = listadoLotes?.Datos ?? new();
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarPlanificacion([FromBody] PlanificacionSolicitud solicitud)
        {
            // Aquí llamamos a un procedimiento almacenado
            var mensaje = new { todoCorrecto = false };
            try
            {
                var respuestaProducto = await this.clienteApi.PostAsync<PlanificacionSolicitud, RespuestaBase<List<ObtenerPlanificacionRespuesta>>>("Planificacion/nueva", solicitud);
                if (respuestaProducto != null)
                {
                    mensaje = new { todoCorrecto = respuestaProducto.Codigo == 0 };
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
        public async Task<IActionResult> InformacionPlanificacion([FromBody] ObtenerPlanificacionLoteSolicitud solicitud)
        {
            // Aquí llamamos a un procedimiento almacenado
            var mensaje = new { Listado = new ObtenerPlanificacionRespuesta() };
            try
            {
                var respuestaProducto = await this.clienteApi.PostAsync<ObtenerPlanificacionLoteSolicitud, RespuestaBase<List<ObtenerPlanificacionRespuesta>>>("Planificacion/planificacion/id", solicitud);
                if (respuestaProducto != null)
                {
                    mensaje = new { Listado = respuestaProducto.Datos[0] ?? new ObtenerPlanificacionRespuesta() };
                }
            }
            catch (Exception)
            {
                mensaje = new { Listado = new ObtenerPlanificacionRespuesta() };
            }

            return this.Json(mensaje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarPlanificacion([FromBody] PlanificacionSolicitud solicitud)
        {
            // Aquí llamamos a un procedimiento almacenado
            var mensaje = new { todoCorrecto = false };
            try
            {
                var respuestaProducto = await this.clienteApi.PostAsync<PlanificacionSolicitud, RespuestaBase<List<ObtenerPlanificacionRespuesta>>>("Planificacion/actualizar", solicitud);
                if (respuestaProducto != null)
                {
                    mensaje = new { todoCorrecto = respuestaProducto.Codigo == 0 };
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
        public async Task<IActionResult> CrearLote([FromBody] AgregarLoteSolicitud solicitud)
        {
            // Aquí llamamos a un procedimiento almacenado
            var mensaje = new { todoCorrecto = false };
            try
            {
                var respuestaProducto = await this.clienteApi.PostAsync<AgregarLoteSolicitud, RespuestaBase<bool>>("Planificacion/CrearLote", solicitud);
                if (respuestaProducto != null)
                {
                    mensaje = new { todoCorrecto = respuestaProducto.Datos };
                }
            }
            catch (Exception)
            {
                mensaje = new { todoCorrecto = false };
            }

            return this.Json(mensaje);
        }
    }
}
