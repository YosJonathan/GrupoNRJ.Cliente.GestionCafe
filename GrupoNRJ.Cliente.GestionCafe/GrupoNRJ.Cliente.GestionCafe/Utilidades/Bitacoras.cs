using System.Runtime.CompilerServices;
using System.Text.Json;

namespace GrupoNRJ.Servicio.GestionCafe.Utilidades
{
    public class Bitacoras
    {

        /// <summary>
        /// Convierte un objeto genérico a JSON con formato.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto</typeparam>
        /// <param name="objeto">Objeto a serializar</param>
        /// <returns>Cadena JSON</returns>
        public static string ConvertirAJson<T>(T objeto)
        {
            try
            {
                return JsonSerializer.Serialize(objeto, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
            catch (Exception ex)
            {
                // En caso de error en la serialización, devolvemos un mensaje simple
                return $"Error al convertir a JSON: {ex.Message}";
            }
        }

        /// <summary>
        /// Guarda un mensaje de error en la consola incluyendo el nombre del método que llamó.
        /// </summary>
        /// <param name="mensaje">Mensaje de error</param>
        /// <param name="modelos">Modelo.</param>
        /// <param name="nombreMetodo">Nombre del método que llamó (se obtiene automáticamente si no se pasa)</param>
        public static void GuardarError<T>(string mensaje, T modelos,
            [CallerMemberName] string nombreMetodo = "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Metodo: {nombreMetodo} - Excepción: {mensaje} - Parametros: {modelos}");
            Console.ResetColor();
        }
    }
}
