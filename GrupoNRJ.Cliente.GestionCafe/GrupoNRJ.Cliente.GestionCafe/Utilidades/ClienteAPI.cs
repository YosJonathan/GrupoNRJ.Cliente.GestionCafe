// <copyright file="ClienteAPI.cs" company="GrupoAnalisis">
// Copyright (c) GrupoAnalisis. All rights reserved.
// </copyright>

namespace GrupoNRJ.Cliente.GestionCafe.Utilidades
{
    using GrupoNRJ.Modelos.GestionCafe.Solicitudes;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using static System.Net.WebRequestMethods;

    /// <summary>
    /// Clase para api de clientes.
    /// </summary>
    public class ClienteAPI
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonOptions;
        private string token;

        public ClienteAPI(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Realiza una petición GET al endpoint especificado y convierte la respuesta en el modelo indicado.
        /// </summary>
        /// <typeparam name="T">Tipo del modelo esperado en la respuesta.</typeparam>
        /// <param name="url">Ruta o endpoint de la API.</param>
        /// <returns>Instancia del modelo con los datos de la API.</returns>
        public async Task<T?> GetAsync<T>(string url)
        {
            if (string.IsNullOrEmpty(this.token))
            {
                this.token = await this.ObtenerTokenAsync("root", "root");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.token);

            var response = await this.httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(this.jsonOptions);
        }

        /// <summary>
        /// Realiza una petición POST al endpoint especificado enviando datos y convierte la respuesta en el modelo indicado.
        /// </summary>
        /// <typeparam name="TRequest">Tipo de los datos que se envían en la petición.</typeparam>
        /// <typeparam name="TResponse">Tipo del modelo esperado en la respuesta.</typeparam>
        /// <param name="url">Ruta o endpoint de la API.</param>
        /// <param name="datos">Objeto con los datos a enviar.</param>
        /// <returns>Instancia del modelo con los datos devueltos por la API.</returns>
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest? datos)
        {
            if (string.IsNullOrEmpty(this.token))
            {
                this.token = await this.ObtenerTokenAsync("root", "root");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.token);

            var json = JsonSerializer.Serialize(datos, this.jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>(this.jsonOptions);
        }

        /// <summary>
        /// Realiza una petición PUT al endpoint especificado enviando datos y convierte la respuesta en el modelo indicado.
        /// </summary>
        /// <typeparam name="TRequest">Tipo de los datos que se envían en la petición.</typeparam>
        /// <typeparam name="TResponse">Tipo del modelo esperado en la respuesta.</typeparam>
        /// <param name="url">Ruta o endpoint de la API.</param>
        /// <param name="datos">Objeto con los datos a actualizar.</param>
        /// <returns>Instancia del modelo con los datos actualizados devueltos por la API.</returns>
        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest datos)
        {
            var json = JsonSerializer.Serialize(datos, this.jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>(this.jsonOptions);
        }

        /// <summary>
        /// Realiza una petición DELETE al endpoint especificado.
        /// </summary>
        /// <param name="url">Ruta o endpoint de la API.</param>
        /// <returns>True si la eliminación fue exitosa, False en caso contrario.</returns>
        public async Task<bool> DeleteAsync(string url)
        {
            var response = await this.httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Obtiene token async
        /// </summary>
        /// <param name="usuario">Usuario.</param>
        /// <param name="clave">Contraseña.</param>
        /// <returns>token valido.</returns>
        /// <exception cref="Exception">No se pudo autenticar.</exception>
        private async Task<string> ObtenerTokenAsync(string usuario, string clave)
        {
#pragma warning disable SA1118 // Parameter must not span multiple lines
            var response = await this.httpClient.PostAsJsonAsync("/Sevicio.GestionCafe/api/Auth/login", new
            LoginSolicitud
            {
                Username = usuario,
                Password = clave
            });
#pragma warning restore SA1118 // Parameter must not span multiple lines

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("No se pudo autenticar");
            }

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return json.GetProperty("token")
                .GetString();
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }
    }
}
