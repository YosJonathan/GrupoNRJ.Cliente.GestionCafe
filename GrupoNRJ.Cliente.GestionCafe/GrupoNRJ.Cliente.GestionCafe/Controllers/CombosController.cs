// <copyright file="CombosController.cs" company="GrupoAnalisis">
// Copyright (c) GrupoAnalisis. All rights reserved.
// </copyright>

namespace GrupoNRJ.Cliente.GestionCafe.Controllers
{
    using GrupoNRJ.Cliente.GestionCafe.Utilidades;
    using GrupoNRJ.Modelos.GestionCafe;
    using GrupoNRJ.Modelos.GestionCafe.Respuestas;
    using GrupoNRJ.Modelos.GestionCafe.Solicitudes;
    using GrupoNRJ.Servicio.GestionCafe.Utilidades;
    using Microsoft.AspNetCore.Mvc;

    public class CombosController : Controller
    {
        private readonly ClienteAPI clienteApi;

#pragma warning disable IDE0290 // Usar constructor principal
        public CombosController(ClienteAPI clienteAPI)
#pragma warning restore IDE0290 // Usar constructor principal
        {
            this.clienteApi = clienteAPI;
        }

        public async Task<ActionResult> GestionCombos()
        {
            try
            {
                var listadoProductos = await this.clienteApi.PostAsync<object, RespuestaBase<ListadoCatalogoProductosRespuesta>>("Catalogo/obtenerCatalogoCombo", null);
                this.ViewData["listadoProductos"] = listadoProductos?.Datos ?? new();
                var listadoCombos = await this.clienteApi.PostAsync<object, RespuestaBase<List<CombosResponse>>>("Combo/ListaCombos", null);
                this.ViewData["listadoCombos"] = listadoCombos?.Datos ?? new();
            }
            catch (Exception ex)
            {
                Bitacoras.GuardarError(ex.ToString(), new { });
            }

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarCombo([FromBody] AgregarComboSolicitud solicitud)
        {
            // Aquí llamamos a un procedimiento almacenado
            var mensaje = new { todoCorrecto = false };
            try
            {
                var respuestaProducto = await this.clienteApi.PostAsync<AgregarComboSolicitud, AgregarCombosRespuestas>("Combo/AgregarCombo", solicitud);
                if (respuestaProducto != null)
                {
                    mensaje = new { todoCorrecto = respuestaProducto.ComboAgregado };
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
        public async Task<IActionResult> EliminarCombo([FromBody] EliminarCombo solicitud)
        {
            // Aquí llamamos a un procedimiento almacenado
            var mensaje = new { todoCorrecto = false };
            try
            {
                var respuestaProducto = await this.clienteApi.PostAsync<EliminarCombo, EliminarComboRespuesta>("Combo/eliminarCombo", solicitud);
                if (respuestaProducto != null)
                {
                    mensaje = new { todoCorrecto = respuestaProducto.ComboEliminadoExitosamente };
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
