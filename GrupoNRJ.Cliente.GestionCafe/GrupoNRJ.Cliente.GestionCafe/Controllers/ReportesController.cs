// <copyright file="ReportesController.cs" company="GrupoAnalisis">
// Copyright (c) GrupoAnalisis. All rights reserved.
// </copyright>

namespace GrupoNRJ.Cliente.GestionCafe.Controllers
{
    using GrupoNRJ.Cliente.GestionCafe.Utilidades;
    using GrupoNRJ.Modelos.GestionCafe;
    using GrupoNRJ.Modelos.GestionCafe.Respuestas;
    using GrupoNRJ.Servicio.GestionCafe.Utilidades;
    using Microsoft.AspNetCore.Mvc;

    public class ReportesController : Controller
    {
        private readonly ClienteAPI clienteApi;

        public ReportesController(ClienteAPI clienteAPI)
        {
            this.clienteApi = clienteAPI;
        }

        public async Task<IActionResult> ReportesGenerales()
        {
            try
            {
                var listadoProductos = await this.clienteApi.GetAsync<RespuestaBase<GeneracionReportesRespuesta>>("Informe/ObtenerReportes");
                if (listadoProductos?.Codigo != 0)
                {
                    this.ViewData["mensajeError"] = "Ha ocurrido un error por favor intente nuevamente.";
                }

                this.ViewData["Reportes"] = listadoProductos?.Datos ?? new();
            }
            catch (Exception ex)
            {
                Bitacoras.GuardarError(ex.ToString(), new { });
            }

            return this.View();
        }
    }
}
