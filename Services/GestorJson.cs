// Importación de System para manejo de excepciones básicas
// Importación de nuestros modelos para reconocer la clase base Reporte
using ComunidadAlertaApp.Models;
// Importación de la librería Newtonsoft.Json para la serialización y deserialización
using Newtonsoft.Json;
using System;
// Importación de System.Collections.Generic para trabajar con listas (List<T>)
using System.Collections.Generic;
// Importación de System.IO para manipulación de archivos y carpetas en disco
using System.IO;

namespace ComunidadAlertaApp.Services
{
    /// <summary>
    /// Implementación concreta de la interfaz IGestorPersistencia usando JSON.
    /// Administra el guardado y lectura de la lista de reportes conservando el Polimorfismo.
    /// </summary>
    public class GestorJson : IGestorPersistencia<List<Reporte>>
    {
        // Configuración especial de Newtonsoft.Json para preservar el tipo concreto en clases derivadas (Polimorfismo)
        private readonly JsonSerializerSettings _opcionesSerializacion;

        /// <summary>
        /// Constructor que inicializa las opciones de conversión JSON.
        /// </summary>
        public GestorJson()
        {
            // Creamos las configuraciones de serialización
            _opcionesSerializacion = new JsonSerializerSettings
            {
                // Formatea el archivo JSON con sangrías e indentación para que sea legible por humanos
                Formatting = Formatting.Indented,

                // TypeNameHandling.All incluye el tipo de clase completo ($type) en el JSON.
                // Esto es fundamental para que al reconstruir ReporteInfraestructura o ReporteMedioAmbiente
                // el deserealizador sepa exactamente a qué clase hija pertenece cada objeto.
                TypeNameHandling = TypeNameHandling.All
            };
        }

        /// <summary>
        /// Guarda la lista de reportes en un archivo en formato JSON.
        /// Aplica Manejo de Excepciones.
        /// </summary>
        /// <param name="datos">Lista de objetos tipo Reporte.</param>
        /// <param name="rutaArchivo">Ruta física del archivo .json.</param>
        public void Guardar(List<Reporte> datos, string rutaArchivo)
        {
            // Bloque Try-Catch para captura y gestión de errores de archivo/disco
            try
            {
                // Verificamos si la carpeta contenedora existe; si no, la creamos
                string directorio = Path.GetDirectoryName(rutaArchivo);
                if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                // Convertimos el objeto lista C# a una cadena de texto en formato JSON
                string jsonTexto = JsonConvert.SerializeObject(datos, _opcionesSerializacion);

                // Escribimos el texto JSON en el archivo (sobrescribe si ya existe)
                File.WriteAllText(rutaArchivo, jsonTexto);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Excepción cuando no se tienen permisos de escritura en la carpeta
                throw new InvalidOperationException($"No hay permisos de escritura en la ruta: {rutaArchivo}", ex);
            }
            catch (IOException ex)
            {
                // Excepción cuando el archivo está bloqueado por otro proceso o falla el disco
                throw new InvalidOperationException("Ocurrió un error de entrada/salida al guardar el archivo JSON.", ex);
            }
            catch (Exception ex)
            {
                // Captura general para cualquier otro tipo de error no contemplado
                throw new Exception($"Error inesperado al intentar guardar los datos: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lee y deserializa la lista de reportes desde un archivo JSON.
        /// Aplica Manejo de Excepciones.
        /// </summary>
        /// <param name="rutaArchivo">Ruta física del archivo .json a leer.</param>
        /// <returns>Regresa una lista de objetos Reporte recargados desde disco.</returns>
        public List<Reporte> Cargar(string rutaArchivo)
        {
            // Bloque Try-Catch para manejo seguro del archivo de lectura
            try
            {
                // Si el archivo no existe aún (ej. primera ejecución del programa), retornamos una lista vacía limpia
                if (!File.Exists(rutaArchivo))
                {
                    return new List<Reporte>();
                }

                // Leemos todo el contenido de texto del archivo JSON
                string jsonTexto = File.ReadAllText(rutaArchivo);

                // Si el archivo está completamente vacío, devolvemos una lista vacía para evitar nulos
                if (string.IsNullOrWhiteSpace(jsonTexto))
                {
                    return new List<Reporte>();
                }

                // Deserializamos el texto a una lista de objetos Reporte.
                // Newtonsoft recreará la instancia correcta (Infraestructura o MedioAmbiente) gracias a TypeNameHandling
                List<Reporte> reportesCargados = JsonConvert.DeserializeObject<List<Reporte>>(jsonTexto, _opcionesSerializacion);

                // Retornamos la lista deserializada o una lista nueva si la lectura regresó nulo
                return reportesCargados ?? new List<Reporte>();
            }
            catch (JsonException ex)
            {
                // Excepción disparada cuando el contenido del JSON está corrupto o mal formado
                throw new InvalidOperationException("El archivo de datos contiene un formato JSON no válido o dañado.", ex);
            }
            catch (IOException ex)
            {
                // Excepción disparada si el archivo no se pudo abrir para lectura
                throw new InvalidOperationException("No se pudo leer el archivo de persistencia en disco.", ex);
            }
            catch (Exception ex)
            {
                // Captura general de errores
                throw new Exception($"Error no controlado al cargar los datos: {ex.Message}", ex);
            }
        }
    }
}